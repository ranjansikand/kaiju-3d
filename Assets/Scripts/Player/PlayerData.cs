// Holds all of the data for player interactions


using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static PlayerInputActions inputActions;

    // Scripts
    public static Player player;
    public static PlayerController playerController;
    public static PlayerCellHandler playerCellHandler;

    public static CameraController cameraController;
    public static Rigidbody rigidbody;

    public static Grid grid;
    public static GameManager gameManager;
    public static CityMap cityMap;
    public static Resources inventory;
    public static GridCellVisuals gridCellVisuals;
    public static Building[] buildings;
    
    public static SpawnedBuilding buildingPrefab => gridCellVisuals.spawnedBuilding;
    public static Material hoverMaterial => gridCellVisuals.hoverMaterial;
    public static Material buildableMaterial => gridCellVisuals.buildableMaterial;
    public static Material notbuildableMaterial => gridCellVisuals.notbuildableMaterial;
    public static Material normalMaterial => gridCellVisuals.normalMaterial;
    
    public static float hoverLiftHeight = 1f;
    public static float buildableLiftHeight = 1.5f;
    public static float notbuildableLiftHeight = 1f;
    public static AudioClip selectSound, deselectSound;

    public static bool inUI = false;
    public static bool isDragging = false;
    public static bool dragCanceled = false;
    public static int selectedBuildingIndex = 0;
    public static Cell currentHoveredCell; 
    public static Cell dragStartCell = null;
    public static List<Cell> dragPathCells = new List<Cell>();

    public static Vector3 position => player.transform.position;
    public static Vector2Int gridBounds => new Vector2Int(grid.width, grid.height);
}
