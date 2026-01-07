using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material normalMaterial;
    
    [Header("Buildings")]
    [SerializeField] private Building[] buildingPrefabs;  // Drag your prefabs here
    private int selectedBuildingIndex = 0;
    
    private PlayerInputActions inputActions;
    private Cell currentHoveredCell;
    private Camera mainCamera;
    
    void Awake() {
        mainCamera = Camera.main;
        inputActions = new PlayerInputActions();
    }
    
    void OnEnable() {
        inputActions.Enable();
        inputActions.Player.Point.performed += OnMouseMove;
        inputActions.Player.Click.performed += OnClick;
    }
    
    void OnDisable() {
        inputActions.Player.Point.performed -= OnMouseMove;
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Disable();
    }
    
    void OnMouseMove(InputAction.CallbackContext context) {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        HandleMouseHover(mousePosition);
    }
    
    void OnClick(InputAction.CallbackContext context) {
        TryPlaceBuilding();
    }
    
    void HandleMouseHover(Vector2 screenPosition) {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit)) {
            Vector3 hitPoint = hit.point;
            Cell cell = gameManager.grid.WorldToCell(hitPoint);
            
            if (cell != currentHoveredCell) {
                // Unhighlight previous cell
                if (currentHoveredCell != null && currentHoveredCell.visual != null) {
                    currentHoveredCell.visual.GetComponent<Renderer>().material = normalMaterial;
                }
                
                // Highlight new cell
                currentHoveredCell = cell;
                if (currentHoveredCell != null && currentHoveredCell.visual != null) {
                    currentHoveredCell.visual.GetComponent<Renderer>().material = hoverMaterial;
                }
            }
        } else {
            // Mouse not over grid - unhighlight
            if (currentHoveredCell != null && currentHoveredCell.visual != null) {
                currentHoveredCell.visual.GetComponent<Renderer>().material = normalMaterial;
            }
            currentHoveredCell = null;
        }
    }
    
    void TryPlaceBuilding() {
        if (currentHoveredCell == null || currentHoveredCell.IsOccupied) {
            Debug.Log("Can't place building here!");
            return;
        }
        
        if (selectedBuildingIndex >= buildingPrefabs.Length) {
            Debug.LogWarning("No building selected!");
            return;
        }
        
        // Instantiate the building
        Building buildingPrefab = buildingPrefabs[selectedBuildingIndex];
        Vector3 spawnPos = currentHoveredCell.WorldPosition(gameManager.grid.cellSize);
        
        Building newBuilding = Instantiate(buildingPrefab, spawnPos, Quaternion.identity);
        newBuilding.occupiedCell = currentHoveredCell;
        
        // Update cell
        currentHoveredCell.building = newBuilding;
        Debug.Log($"Placed {newBuilding.buildingName} at ({currentHoveredCell.x}, {currentHoveredCell.y})");
    }
}