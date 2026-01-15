using UnityEngine;
using System.Linq;

public class Road : Building
{
    [Header("Road Types")]
    [SerializeField] Mesh noConnections;  // No neighbors
    [SerializeField] Mesh culdesac;  // One neighbor
    [SerializeField] Mesh straight, corner;  // Two neighbors
    [SerializeField] Mesh threeWayIntersection;  // Three neighbors
    [SerializeField] Mesh fourWayIntersection;  // Four neighbors

    public override bool dragToPlace => true;

    public override void Built(SpawnedBuilding spawnedBuilding) {
        bool[] neighbors = CheckNeighbors(spawnedBuilding.occupiedCell);
        Mesh meshToSet = GetMesh(neighbors);
        Quaternion rotation = GetRotation(neighbors);

        spawnedBuilding.meshFilter.mesh = meshToSet;
        spawnedBuilding.transform.rotation = rotation;
        
        UpdateNeighbors(spawnedBuilding.occupiedCell);
    }

    private bool[] CheckNeighbors(Cell cell) {
        bool[] results = new bool[4];

        for (int i = 0; i < Data.directions.Length; i++) {
            Vector2Int dir = Data.directions[i];
            Vector2Int target = cell.position + dir;

            if (Utility.InBounds(target)) {
                Cell pointedCell = PlayerData.gameManager.grid.Cells[target.x, target.y];

                if (pointedCell.IsOccupied && pointedCell.building.building is Road)
                    results[i] = true;
                else results[i] = false;
            }
        }

        return results;
    }

    private Mesh GetMesh(bool[] results) {
        int number = results.Count(n => n);

        switch (number) {
            case (0): return noConnections;
            case (1): return culdesac;
            case (2): 
                // Check if connections are opposite (straight) or adjacent (corner)
                bool northSouth = results[0] && results[2];  // North and South
                bool eastWest = results[1] && results[3];     // East and West
                
                if (northSouth || eastWest) {
                    return straight;
                } else {
                    return corner;
                }
            case (3): return threeWayIntersection;
            case (4): return fourWayIntersection;
            default: return noConnections;
        }
    }

    private void UpdateNeighbors(Cell cell) {
        for (int i = 0; i < Data.directions.Length; i++) {
            Vector2Int dir = Data.directions[i];
            Vector2Int target = cell.position + dir;

            if (Utility.InBounds(target)) {
                Cell neighborCell = PlayerData.gameManager.grid.Cells[target.x, target.y];

                if (neighborCell.IsOccupied && neighborCell.building.building is Road) {
                    bool[] neighborNeighbors = CheckNeighbors(neighborCell);
                    Mesh neighborMesh = GetMesh(neighborNeighbors);
                    Quaternion neighborRotation = GetRotation(neighborNeighbors);
                    
                    neighborCell.building.meshFilter.mesh = neighborMesh;
                    neighborCell.building.transform.rotation = neighborRotation;
                }
            }
        }
    }

    private Quaternion GetRotation(bool[] neighbors) {
        bool north = neighbors[0];
        bool east = neighbors[1];
        bool south = neighbors[2];
        bool west = neighbors[3];
        
        int connectionCount = neighbors.Count(n => n);
        
        switch (connectionCount) {
            case 1: // Culdesac - default opens West
                if (north) return Quaternion.Euler(0, 90, 0);
                if (east) return Quaternion.Euler(0, 180, 0);
                if (south) return Quaternion.Euler(0, 270, 0);
                if (west) return Quaternion.Euler(0, 0, 0);
                break;
                
            case 2:
                // Straight - default runs East-West
                if (north && south) return Quaternion.Euler(0, 90, 0);
                if (east && west) return Quaternion.Euler(0, 0, 0);
                
                // Corners (old working version)
                if (north && east) return Quaternion.Euler(0, 0, 0);
                if (east && south) return Quaternion.Euler(0, 90, 0);
                if (south && west) return Quaternion.Euler(0, 180, 0);
                if (west && north) return Quaternion.Euler(0, 270, 0);
                break;
                
            case 3: // Three-way - default opens North, East, West (missing South)
                if (!north) return Quaternion.Euler(0, 180, 0);
                if (!east) return Quaternion.Euler(0, 270, 0);
                if (!south) return Quaternion.Euler(0, 0, 0);
                if (!west) return Quaternion.Euler(0, 90, 0);
                break;
        }
        
        return Quaternion.identity;
    }
}