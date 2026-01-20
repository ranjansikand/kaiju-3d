using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] Car carPrefab;

    public void TrySpawnCar()
    {
        // Get a random road network
        int networkId = PlayerData.cityMap.roadGraph.GetRandomNetworkId();
        if (networkId == -1) return; // No roads
        
        List<Cell> networkRoads = PlayerData.cityMap.roadGraph.GetNetworkCells(networkId);
        if (networkRoads == null || networkRoads.Count < 2) return;
        
        // Pick random start and end
        Cell start = networkRoads[Random.Range(0, networkRoads.Count)];
        Cell end = networkRoads[Random.Range(0, networkRoads.Count)];
        
        if (start == end) return; // Same cell
        
        // Find path
        List<Cell> path = PlayerData.cityMap.roadGraph.FindPath(start, end);
        if (path == null || path.Count < 2) return;
        
        // Spawn car
        Vector3 spawnPos = start.WorldPosition(PlayerData.grid.cellSize) + Data.buildingSpawnOffset;
        Car car = Instantiate(carPrefab, spawnPos, Quaternion.identity);
        car.StartJourney(path);
    }
}
