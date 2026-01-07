// A Cell within the Grid


using UnityEngine;

public class Cell {
    public int x, y;
    public GameObject visual;
    public Building building;

    public bool IsOccupied => building != null;
    
    public Cell(int posX, int posY) {
        x = posX;
        y = posY;
    }

    public void PlaceBuilding(Building newBuilding) {
        building = newBuilding;
        newBuilding.occupiedCell = this;
    }
    
    public Vector3 WorldPosition(float cellSize) {
        return new Vector3(x * cellSize, 0, y * cellSize);
    }
}