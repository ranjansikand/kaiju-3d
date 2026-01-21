using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] Car carPrefab;

    private void Start() {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        while (true) {
            if (PlayerData.cityMap.roadNetworks.Count > 0) {
                TrySpawnCar();
            }
            
            yield return new WaitForSeconds(Random.Range(0f, 3f));
        }
    }

    public void TrySpawnCar()
    {
        int networkId = PlayerData.cityMap.roadGraph.GetRandomNetworkId();
        if (networkId == -1) return;

        if (!PlayerData.cityMap.roadNetworks.TryGetValue(networkId, out var buildingCells))
            return;

        if (buildingCells.Count < 2)
            return;

        // Pick two different buildings
        Cell startBuilding = buildingCells[Random.Range(0, buildingCells.Count)];
        Cell endBuilding;

        do
        {
            endBuilding = buildingCells[Random.Range(0, buildingCells.Count)];
        }
        while (endBuilding == startBuilding);

        // Find adjacent road cells
        Cell startRoad = GetAdjacentRoad(startBuilding, networkId);
        Cell endRoad   = GetAdjacentRoad(endBuilding, networkId);

        if (startRoad == null || endRoad == null)
            return;

        // Pathfind on roads
        List<Cell> path = PlayerData.cityMap.roadGraph.FindPath(startRoad, endRoad);
        if (path == null || path.Count < 2)
            return;

        Vector3 spawnPos =
            startRoad.WorldPosition(PlayerData.grid.cellSize) + Data.buildingSpawnOffset;

        Car car = Instantiate(carPrefab, spawnPos, Quaternion.identity);
        car.StartJourney(path);
    }

    private Cell GetAdjacentRoad(Cell buildingCell, int networkId)
    {
        foreach (Vector2Int dir in Data.directions)
        {
            Vector2Int pos = buildingCell.position + dir;
            if (!Utility.InBounds(pos)) continue;

            Cell cell = PlayerData.grid.Cells[pos.x, pos.y];

            if (cell.IsOccupied &&
                cell.building.building is Road &&
                cell.roadNetworkId == networkId)
            {
                return cell;
            }
        }

        return null;
    }
}
