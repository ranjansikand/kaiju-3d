// Holds all existing buildings

using System.Collections.Generic;
using UnityEngine;

public class CityMap 
{
    public List<SpawnedBuilding> spawnedBuildings;
    public Dictionary<int, List<Cell>> roadNetworks;

    public RoadGraph roadGraph;

    public CityMap() {
        spawnedBuildings = new List<SpawnedBuilding>();
        roadGraph = new RoadGraph();
        roadNetworks = new Dictionary<int, List<Cell>>();
    }

    public void AddBuilding(SpawnedBuilding spawnedBuilding) {
        if (spawnedBuilding.building is Road) {
            roadGraph.AddRoad(spawnedBuilding.occupiedCell);
            int id = spawnedBuilding.occupiedCell.roadNetworkId;

            if (!roadNetworks.ContainsKey(id))
                roadNetworks[id] = new List<Cell>();
            
            UpdateAdjacentCells(spawnedBuilding.occupiedCell);
        } else {
            spawnedBuildings.Add(spawnedBuilding);
            spawnedBuilding.CheckAccessRoads();

            foreach (int i in spawnedBuilding.accessRoads) {
                if (roadNetworks.TryGetValue(i, out var val)) {
                    if (!val.Contains(spawnedBuilding.occupiedCell))
                        val.Add(spawnedBuilding.occupiedCell);
                }
            }
        }
    }

    public void RemoveBuilding(SpawnedBuilding spawnedBuilding) {
        if (spawnedBuilding.building is Road) {
            roadGraph.RemoveRoad(spawnedBuilding.occupiedCell);
        } else spawnedBuildings.Remove(spawnedBuilding);
    }

    private void UpdateAdjacentCells(Cell activeCell) {
        foreach (Vector2Int dir in Data.directions) {
            Vector2Int pos = activeCell.position + dir;

            if (Utility.InBounds(activeCell.position + pos)) {
                Cell cell = PlayerData.grid.Cells[pos.x, pos.y];

                if (cell.IsOccupied && !(cell.building.building is Road)) {
                    SpawnedBuilding spawnedBuilding = cell.building;
                    spawnedBuilding.accessRoads.Add(activeCell.roadNetworkId);

                    Debug.Log("Building " + spawnedBuilding + " access roads updated");

                    foreach (int i in spawnedBuilding.accessRoads) {
                        if (roadNetworks.TryGetValue(i, out var val)) {
                            if (!val.Contains(spawnedBuilding.occupiedCell))
                                val.Add(spawnedBuilding.occupiedCell);
                        }
                    }
                }

                
            }
        }
    }
}
