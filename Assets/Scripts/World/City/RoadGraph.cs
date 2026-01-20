// Connectivity graph that stores which tiles are connected to one another


using System.Collections.Generic;
using UnityEngine;

public class RoadGraph
{
    // Road adjacency graph
    private Dictionary<Cell, List<Cell>> adjacency = new Dictionary<Cell, List<Cell>>();

    // Connected road networks
    private Dictionary<int, List<Cell>> networks = new Dictionary<int, List<Cell>>();
    private int nextNetworkId = 0;
    private bool networksDirty = false;

    public RoadGraph() {}

    #region Building

    public void AddRoad(Cell roadCell)
    {
        if (!adjacency.ContainsKey(roadCell))
            adjacency[roadCell] = new List<Cell>();

        foreach (Vector2Int dir in Data.directions)
        {
            Vector2Int neighborPos = roadCell.position + dir;
            if (!Utility.InBounds(neighborPos))
                continue;

            Cell neighbor = PlayerData.grid.Cells[neighborPos.x, neighborPos.y];

            if (neighbor.IsOccupied && neighbor.building.building is Road)
            {
                if (!adjacency.ContainsKey(neighbor))
                    adjacency[neighbor] = new List<Cell>();

                if (!adjacency[roadCell].Contains(neighbor))
                    adjacency[roadCell].Add(neighbor);

                if (!adjacency[neighbor].Contains(roadCell))
                    adjacency[neighbor].Add(roadCell);
            }
        }

        networksDirty = true;
        
        #if UNITY_EDITOR
        RebuildNetworksIfNeeded(); // For immediate debug feedback in editor
        DebugLogNetworkInfo(roadCell);
        #endif
    }

    public void RemoveRoad(Cell roadCell)
    {
        if (!adjacency.ContainsKey(roadCell))
            return;

        foreach (Cell neighbor in adjacency[roadCell])
            adjacency[neighbor].Remove(roadCell);

        adjacency.Remove(roadCell);
        networksDirty = true;
    }

    public List<Cell> GetNeighbors(Cell roadCell)
    {
        if (adjacency.ContainsKey(roadCell))
            return new List<Cell>(adjacency[roadCell]);
        
        return new List<Cell>();
    }

    #endregion

    #region Networks

    public void RebuildNetworksIfNeeded() {
        if (!networksDirty) return;
        
        RebuildNetworks();
        networksDirty = false;
    }

    void RebuildNetworks() {
        networks.Clear();
        nextNetworkId = 0;

        foreach (Cell cell in adjacency.Keys)
            cell.roadNetworkId = -1;

        foreach (Cell start in adjacency.Keys) {
            if (start.roadNetworkId != -1)
                continue;

            int networkId = nextNetworkId++;
            networks[networkId] = new List<Cell>();

            Queue<Cell> queue = new Queue<Cell>();
            start.roadNetworkId = networkId;
            queue.Enqueue(start);

            while (queue.Count > 0) {
                Cell current = queue.Dequeue();
                networks[networkId].Add(current);

                foreach (Cell neighbor in adjacency[current]) {
                    if (neighbor.roadNetworkId == -1) {
                        neighbor.roadNetworkId = networkId;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
    }

    public bool AreConnected(Cell a, Cell b) {
        RebuildNetworksIfNeeded();
        return a.roadNetworkId != -1 && a.roadNetworkId == b.roadNetworkId;
    }

    public List<Cell> GetNetworkCells(int networkId) {
        RebuildNetworksIfNeeded();
        
        if (networks.TryGetValue(networkId, out var list))
            return new List<Cell>(list);

        return null;
    }

    public int GetNetworkCount() {
        RebuildNetworksIfNeeded();
        return networks.Count;
    }

    public int GetRandomNetworkId() {
        RebuildNetworksIfNeeded();
        
        if (networks.Count == 0)
            return -1;

        int index = Random.Range(0, networks.Count);
        foreach (int id in networks.Keys) {
            if (index-- == 0)
                return id;
        }

        return -1;
    }

    #endregion

    #region Pathfinding
    public List<Cell> FindPath(Cell start, Cell end) {
        RebuildNetworksIfNeeded();
        
        if (!AreConnected(start, end))
            return null;

        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> gScore = new Dictionary<Cell, float>();
        Dictionary<Cell, float> fScore = new Dictionary<Cell, float>();

        HashSet<Cell> openSet = new HashSet<Cell>();
        HashSet<Cell> closedSet = new HashSet<Cell>();

        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);
        openSet.Add(start);

        while (openSet.Count > 0) {
            Cell current = null;
            float lowestF = float.MaxValue;

            foreach (Cell cell in openSet) {
                if (fScore[cell] < lowestF) {
                    lowestF = fScore[cell];
                    current = cell;
                }
            }

            if (current == end)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Cell neighbor in adjacency[current]) {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeG = gScore[current] + 1f;
                float existingG;

                if (!gScore.TryGetValue(neighbor, out existingG) || tentativeG < existingG) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, end);
                    openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    float Heuristic(Cell a, Cell b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell current) {
        List<Cell> path = new List<Cell>() { current };

        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }

    #endregion

    #region Debug
    
    public void DebugLogNetworkInfo(Cell roadCell) {
        #if UNITY_EDITOR
        int networkId = roadCell.roadNetworkId;

        if (networkId == -1) {
            Debug.LogWarning($"[RoadGraph] Road ({roadCell.x},{roadCell.y}) has no network ID");
            return;
        }

        int networkCount = networks.Count;
        int networkSize = networks.TryGetValue(networkId, out var list) ? list.Count : 0;

        Debug.Log(
            $"[RoadGraph] Road added at ({roadCell.x},{roadCell.y}) | " +
            $"Network {networkId} size = {networkSize} | " +
            $"Total networks = {networkCount}"
        );
        #endif
    }
    
    #endregion
}