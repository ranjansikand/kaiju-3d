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
        float[,] falloffMap = new float[width, height];

        // Noise variables
        float scale = 0.1f;
        float xOffset = Random.Range(-10000, 10000);
        float yOffset = Random.Range(-10000, 10000);

        // Falloff variables
        float falloffStrength = 3f;
        float falloffStart = 2.2f;

        for (int x = 0; x < sizeX; x++) {
            for (int y = 0; y < sizeY; y++) {
                // Generate noise
                noisemap[x, y] = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);

                // Generate falloff map
                // Normalize coordinates to -1 to 1 range (center is 0, 0)
                float xv = x / (float)sizeX * 2 - 1;
                float yv = y / (float)sizeY * 2 - 1;
                
                // Distance to edge (square gradient)
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                
                // Apply smooth falloff curve
                // Higher falloffStrength = steeper transition
                // Higher falloffStart = more flat area in center
                float a = Mathf.Pow(v, falloffStrength);
                float b = Mathf.Pow(falloffStart - falloffStart * v, falloffStrength);
                falloffMap[x, y] = a / (a + b);

                float noise = noisemap[x, y] - falloffMap[x, y];

                // Create cells
                Cells[x, y] = new Cell(x, y);

                if (noise <= 0.35f) {
                    Cells[x, y].cellType = CellType.Water;
                    continue;  // Stop here if water
                }

                Cells[x, y].cellType = CellType.Empty;
                CreateCellVisual(Cells[x, y], gridCellVisuals);

                if ((noisemap[x, y] <= 0.65f) || (noisemap[x, y] <= 0.75f && Random.Range(0, 4) < 2)) 
                    continue;
                SpawnTrees(Cells[x, y], gridCellVisuals, noisemap[x, y]);
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
    void SpawnTrees(Cell cell, GridCellVisuals gridCellVisuals, float noise) {
        int treeCount = Mathf.RoundToInt(Mathf.Lerp(1, 3, Mathf.InverseLerp(0.35f, .75f, noise)));

        for (int i = 0; i < treeCount; i++) {
            Decor decor = gridCellVisuals.decor[Random.Range(0, gridCellVisuals.decor.Length)];
            Vector3 spawnPos = new Vector3(Random.Range(-0.75f, 0.75f), 0, Random.Range(-0.75f, 0.75f));

            SpawnedDecor spawnedDecor = GameManager.Instantiate(
                gridCellVisuals.spawnedDecor, 
                cell.visual.transform);
            spawnedDecor.transform.position += Data.buildingSpawnOffset + spawnPos;
            spawnedDecor.decor = decor;
            spawnedDecor.OnBuild();

            cell.decor.Add(spawnedDecor);
        }
        
        cell.cellType = CellType.Forest;
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