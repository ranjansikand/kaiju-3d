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
    public static GameManager gameManager;
    public static Rigidbody rigidbody;

    public static Resources inventory;
    public static Building[] buildings;
    
    public static SpawnedBuilding buildingPrefab;
    public static Material hoverMaterial, buildableMaterial, notbuildableMaterial;
    public static Material normalMaterial;

    public static bool inUI = false;
    public static bool isDragging = false;
    public static int selectedBuildingIndex = 0;
    public static Cell currentHoveredCell; 
    public static Cell dragStartCell = null;
    public static List<Cell> dragPathCells = new List<Cell>();

    public static Vector3 position => player.transform.position;
}
