// Handles basic functions like attaching scripts and assigning data
// The only Player script that should have inspector fields

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CameraController cameraController;

    [Header("Materials")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material hoverMaterial, buildableMaterial, notbuildableMaterial;
    
    
    [Header("Buildings")]
    [SerializeField] private SpawnedBuilding buildingPrefab;
    [SerializeField] private Building[] buildings;  // Drag your prefabs here
    private int selectedBuildingIndex = 0;

    void Awake() {
        // PlayerData.inputActions = new PlayerInputActions();

        PlayerData.player = this;
        PlayerData.playerCellHandler = GetComponent<PlayerCellHandler>();
        PlayerData.playerController = GetComponent<PlayerController>();
        PlayerData.rigidbody = GetComponent<Rigidbody>();

        PlayerData.gameManager = gameManager;
        PlayerData.cameraController = cameraController;

        PlayerData.inventory = new Resources(20, 20);

        PlayerData.hoverMaterial = hoverMaterial;
        PlayerData.buildableMaterial = buildableMaterial;
        PlayerData.notbuildableMaterial = notbuildableMaterial;
        PlayerData.normalMaterial = normalMaterial;

        PlayerData.buildings = buildings;
        PlayerData.buildingPrefab = buildingPrefab;
        PlayerData.selectedBuildingIndex = selectedBuildingIndex;
    }
}
