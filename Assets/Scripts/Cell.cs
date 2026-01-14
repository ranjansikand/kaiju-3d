// A Cell within the Grid


using UnityEngine;

public class Cell {
    public int x, y;
    public GameObject visual;
    public SpawnedBuilding building;

    public bool IsOccupied => building != null;
    public Vector2Int position => new Vector2Int(x, y);
    
    public Cell(int posX, int posY) {
        x = posX;
        y = posY;
    }

    public void PlaceBuilding(SpawnedBuilding newBuilding) {
        building = newBuilding;
        newBuilding.occupiedCell = this;

        newBuilding.OnBuild();
    }
    
    public Vector3 WorldPosition(float cellSize) {
        return new Vector3(x * cellSize, 0, y * cellSize);
    }

    public bool IsBuildable() {
        // Adding for future use
        return !IsOccupied && visual != null;
    }
}