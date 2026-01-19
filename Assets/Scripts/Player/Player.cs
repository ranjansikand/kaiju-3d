// Handles basic functions like attaching scripts and assigning data
// The only Player script that should have inspector fields


using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CameraController cameraController;

    [Header("Visuals")]
    [SerializeField] GridCellVisuals gridCellVisuals;

    [Header("Interactions")]
    [SerializeField, Range(-0.25f, 0.25f)] float hoverLiftHeight = .1f;
    [SerializeField, Range(-0.25f, 0.25f)] float buildableLiftHeight = .15f;
    [SerializeField, Range(-0.25f, 0.25f)] float notbuildableLiftHeight = .125f;
    [SerializeField] AudioClip select, deselect;
    
    [Header("Buildings")]
    [SerializeField] private Building[] buildings;  // Drag your prefabs here
    private int selectedBuildingIndex = 0;

    void Awake() {
        PlayerData.player = this;
        PlayerData.playerCellHandler = GetComponent<PlayerCellHandler>();
        PlayerData.playerController = GetComponent<PlayerController>();
        PlayerData.rigidbody = GetComponent<Rigidbody>();

        PlayerData.gameManager = gameManager;
        PlayerData.cameraController = cameraController;
        PlayerData.inventory = new Resources(200, 200);
        PlayerData.gridCellVisuals = gridCellVisuals;
        PlayerData.grid = new Grid(50, 50, gridCellVisuals);

        PlayerData.selectSound = select;
        PlayerData.deselectSound = deselect;

        PlayerData.hoverLiftHeight = hoverLiftHeight;
        PlayerData.buildableLiftHeight = buildableLiftHeight;
        PlayerData.notbuildableLiftHeight = notbuildableLiftHeight;
        PlayerData.cityMap = new CityMap();
        PlayerData.buildings = buildings;
        PlayerData.selectedBuildingIndex = selectedBuildingIndex;

        transform.position = new Vector3(PlayerData.grid.width / 2, 0, PlayerData.grid.height / 2);
    }
}