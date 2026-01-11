// Handles basic functions like attaching scripts and assigning data
// The only Player script that should have inspector fields

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CameraController cameraController;

    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material normalMaterial;
    
    [Header("Buildings")]
    [SerializeField] private Building[] buildingPrefabs;  // Drag your prefabs here
    private int selectedBuildingIndex = 0;

    void Awake() {
        // PlayerData.inputActions = new PlayerInputActions();

        PlayerData.player = this;
        PlayerData.playerCellHandler = GetComponent<PlayerCellHandler>();
        PlayerData.playerController = GetComponent<PlayerController>();
        PlayerData.rigidbody2D = GetComponent<Rigidbody2D>();

        PlayerData.gameManager = gameManager;
        PlayerData.cameraController = cameraController;

        PlayerData.inventory = new Resources(20, 20);

        
        PlayerData.hoverMaterial = hoverMaterial;
        PlayerData.normalMaterial = normalMaterial;

        PlayerData.buildingPrefabs = buildingPrefabs;
        PlayerData.selectedBuildingIndex = selectedBuildingIndex;
    }
}
