// A Cell within the Grid


using System.Collections.Generic;
using UnityEngine;

public class Cell {
    public int x, y;
    public GameObject visual;
    public SpawnedBuilding building;
    public List<SpawnedDecor> decor = new List<SpawnedDecor>();
    
    public int roadNetworkId = -1;

    public bool IsOccupied => building != null;
    public Vector2Int position => new Vector2Int(x, y);
    
    public Cell(int posX, int posY) {
        x = posX;
        y = posY;
    }

    public void PlaceBuilding(SpawnedBuilding newBuilding) {
        if (decor.Count > 0) {
            for (int i = 0; i < decor.Count; i++) 
                GameManager.Destroy(decor[i].gameObject);
            decor.Clear();
        }

        building = newBuilding;
    }
    
    public Vector3 WorldPosition(float cellSize) {
        return new Vector3(x * cellSize, 0, y * cellSize);
    }

    public bool IsBuildable() {
        // Adding for future use
        return !IsOccupied && visual != null;
    }
}