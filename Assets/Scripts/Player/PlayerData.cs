using System.Collections;
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
    public static Material hoverMaterial;
    public static Material normalMaterial;
    
    public static int selectedBuildingIndex = 0;
    public static Cell currentHoveredCell;
}
