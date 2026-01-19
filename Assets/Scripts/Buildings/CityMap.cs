// Holds all existing buildings


using System.Collections.Generic;
using UnityEngine;

public class CityMap 
{
    public List<SpawnedBuilding> spawnedBuildings;

    public RoadGraph roadGraph;

    public CityMap() {
        spawnedBuildings = new List<SpawnedBuilding>();
        roadGraph = new RoadGraph();
    }

    public void AddBuilding(SpawnedBuilding spawnedBuilding) {
        spawnedBuildings.Add(spawnedBuilding);

        if (spawnedBuilding.building is Road) {
            roadGraph.AddRoad(spawnedBuilding.occupiedCell);

            UpdateAdjacentCells(spawnedBuilding.occupiedCell);
        }
    }

    public void RemoveBuilding(SpawnedBuilding spawnedBuilding) {
        spawnedBuildings.Remove(spawnedBuilding);

        if (spawnedBuilding.building is Road) {
            roadGraph.RemoveRoad(spawnedBuilding.occupiedCell);
        }
    }

    private void UpdateAdjacentCells(Cell activeCell) {
        foreach (Vector2Int dir in Data.directions) {
            Vector2Int pos = activeCell.position + dir;

            if (Utility.InBounds(pos)) {
                Cell cell = PlayerData.grid.Cells[pos.x, pos.y];

                if (cell.IsOccupied && cell.building.building is Road) {
                    cell.building.accessRoads.Add(activeCell.roadNetworkId);
                    Debug.Log(activeCell.roadNetworkId);
                }
            }
        }
    }
}