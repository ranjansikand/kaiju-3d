


using UnityEngine;

public class Grid
{
    public Cell[,] Cells;
    public float cellSize = 1.5f;  // size of each cell in world units

    public int width;
    public int height;
    
    private GameObject gridContainer;

    public Grid(int sizeX, int sizeY, GameObject gridCellVisual) {
        Cells = new Cell[sizeX, sizeY];
        gridContainer = new GameObject("Grid Visuals");

        width = sizeX;
        height = sizeY;

        for (int i = 0; i < sizeX; i++) {
            for (int j = 0; j < sizeY; j++) {
                Cells[i, j] = new Cell(i, j);
                CreateCellVisual(Cells[i, j], gridCellVisual);
            }
        }
    }

    void CreateCellVisual(Cell cell, GameObject gridCellVisual) {
        if (Random.Range(0, 100) <= 5) return;
        
        GameObject cellObj = GameManager.Instantiate(gridCellVisual, gridContainer.transform);
        cellObj.transform.localPosition = new Vector3(cell.x * cellSize, 0, cell.y * cellSize);
        cellObj.name = $"Cell ({cell.x},{cell.y})";
        
        // Store reference in Cell
        cell.visual = cellObj;
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