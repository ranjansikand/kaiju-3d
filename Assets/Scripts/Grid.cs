// Holds the information for the active grid


using UnityEngine;

public class Grid
{
    public Cell[,] Cells;
    public float cellSize = 1.5f;  // size of each cell in world units

    public int width;
    public int height;
    
    private GameObject gridContainer;

    public Grid(int sizeX, int sizeY, GridCellVisuals gridCellVisuals) {
        Cells = new Cell[sizeX, sizeY];
        gridContainer = new GameObject("Grid Visuals");

        width = sizeX;
        height = sizeY;

        // World generation
        float[,] noisemap = new float[width, height];
        float scale = 0.1f;
        float xOffset = Random.Range(-10000, 10000);
        float yOffset = Random.Range(-10000, 10000);

        for (int x = 0; x < sizeX; x++) {
            for (int y = 0; y < sizeY; y++) {
                // Generate noise
                noisemap[x, y] = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);

                // Create cells
                Cells[x, y] = new Cell(x, y);

                if (noisemap[x, y] <= 0.35f) continue;
                CreateCellVisual(Cells[x, y], gridCellVisuals);

                if ((noisemap[x, y] <= 0.65f) || (noisemap[x, y] <= 0.75f && Random.Range(0, 4) < 2)) 
                    continue;
                CreateDecorations(Cells[x, y], gridCellVisuals);
            }
        }
    }

    void CreateCellVisual(Cell cell, GridCellVisuals gridCellVisuals) {
        GameObject cellObj = GameManager.Instantiate(gridCellVisuals.gridCell, gridContainer.transform);
        cellObj.transform.localPosition = new Vector3(cell.x * cellSize, 0, cell.y * cellSize);
        cellObj.name = $"Cell ({cell.x},{cell.y})";
        
        // Store reference in Cell
        cell.visual = cellObj;
    }

    // Add decorations
    void CreateDecorations(Cell cell, GridCellVisuals gridCellVisuals) {
        Decor decor = gridCellVisuals.decor[Random.Range(0, gridCellVisuals.decor.Length)];
        Vector3 spawnPos = new Vector3(Random.Range(-0.75f, 0.75f), 0, Random.Range(-0.75f, 0.75f));

        SpawnedDecor spawnedDecor = GameManager.Instantiate(
            gridCellVisuals.spawnedDecor, 
            cell.visual.transform);
        spawnedDecor.transform.position += Data.buildingSpawnOffset + spawnPos;
        spawnedDecor.decor = decor;
        spawnedDecor.OnBuild();
        cell.decor = new SpawnedDecor[] { spawnedDecor };
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