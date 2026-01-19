// Holds all existing buildings


using System.Collections.Generic;

public class CityMap 
{
    public List<SpawnedBuilding> spawnedBuildings;

    public CityMap() {
        spawnedBuildings = new List<SpawnedBuilding>();
    }

    public void AddBuilding(SpawnedBuilding spawnedBuilding) {
        spawnedBuildings.Add(spawnedBuilding);
    }

    public void RemoveBuilding(SpawnedBuilding spawnedBuilding) {
        spawnedBuildings.Remove(spawnedBuilding);
    }
}