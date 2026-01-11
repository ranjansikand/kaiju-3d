


using UnityEngine;

public class Grid
{
    public Cell[,] Cells;
    public float cellSize = 1.5f;  // size of each cell in world units
    
    private GameObject gridContainer;

    public Grid(int sizeX, int sizeY) {
        Cells = new Cell[sizeX, sizeY];
        gridContainer = new GameObject("Grid Visuals");

        for (int i = 0; i < sizeX; i++) {
            for (int j = 0; j < sizeY; j++) {
                Cells[i, j] = new Cell(i, j);
                CreateCellVisual(i, j);
            }
        }
    }

    void CreateCellVisual(int x, int y) {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
        plane.transform.localScale = Vector3.one * cellSize * 0.1f; // planes are 10x10 by default
        plane.transform.SetParent(gridContainer.transform);
        plane.name = $"Cell ({x},{y})";
        
        // Store reference in Cell
        Cells[x, y].visual = plane;
    }
    
    // Convert world position to grid coordinates
    public Cell WorldToCell(Vector3 worldPos) {
        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.z / cellSize);
        
        if (x >= 0 && x < Cells.GetLength(0) && y >= 0 && y < Cells.GetLength(1)) {
            return Cells[x, y];
        }
        return null;
    }
}