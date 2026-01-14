// Handles receiving input


using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    void Awake() {
        PlayerData.inputActions = new PlayerInputActions();
    }

    void OnEnable() {
        PlayerData.inputActions.Enable();
        PlayerData.inputActions.Player.Point.performed += OnMouseMove;
        PlayerData.inputActions.Player.LeftClick.started += OnLeftClickStarted;
        PlayerData.inputActions.Player.LeftClick.canceled += OnLeftClickReleased;
        PlayerData.inputActions.Player.RightClick.performed += OnRightClick;
        PlayerData.inputActions.Player.Zoom.performed += OnMouseScroll;
        PlayerData.inputActions.Player.Move.performed += OnMove;
        PlayerData.inputActions.Player.Move.canceled += OnMove;
    }
    
    void OnDisable() {
        PlayerData.inputActions.Player.Point.performed -= OnMouseMove;
        PlayerData.inputActions.Player.LeftClick.started -= OnLeftClickStarted;
        PlayerData.inputActions.Player.LeftClick.canceled -= OnLeftClickReleased;
        PlayerData.inputActions.Player.RightClick.performed -= OnRightClick;
        PlayerData.inputActions.Player.Zoom.performed -= OnMouseScroll;
        PlayerData.inputActions.Player.Move.performed -= OnMove;
        PlayerData.inputActions.Player.Move.canceled -= OnMove;
        PlayerData.inputActions.Disable();
    }
    
     public void OnMouseMove(InputAction.CallbackContext context) {
        if (PlayerData.inUI) return;

        Vector2 mousePosition = context.ReadValue<Vector2>();
        PlayerData.playerCellHandler.HandleMouseHover(mousePosition);

        // Update drag preview if dragging
        if (PlayerData.isDragging) {
            PlayerData.playerCellHandler.UpdateDragPreview();
        }
    }

    public void OnLeftClickStarted(InputAction.CallbackContext context) {
        if (PlayerData.inUI || PlayerData.selectedBuildingIndex < 0) return;
        
        Building selectedBuilding = PlayerData.buildings[PlayerData.selectedBuildingIndex];
        
        if (selectedBuilding.dragToPlace) {
            PlayerData.isDragging = true;
            PlayerData.dragStartCell = PlayerData.currentHoveredCell;
        }
    }
    
    public void OnLeftClickReleased(InputAction.CallbackContext context) {
        if (PlayerData.inUI || PlayerData.selectedBuildingIndex < 0) {
            if (PlayerData.isDragging) {  // Stop drag
                PlayerData.playerCellHandler.ClearDragPreview();
                PlayerData.isDragging = false;
                PlayerData.dragStartCell = null;
            }

            return;
        }

        Building selectedBuilding = PlayerData.buildings[PlayerData.selectedBuildingIndex];
        
        if (selectedBuilding.dragToPlace && PlayerData.isDragging) {
            // Check if it was a click (same cell) or drag (different cells)
            if (PlayerData.dragStartCell == PlayerData.currentHoveredCell) {
                // Single click - place one
                PlayerData.playerCellHandler.TryPlaceBuilding(PlayerData.currentHoveredCell);
            } else {
                // Drag - place along path
                PlayerData.playerCellHandler.PlaceBuildingsAlongPath();
            }
            
            PlayerData.playerCellHandler.ClearDragPreview();
            PlayerData.isDragging = false;
            PlayerData.dragStartCell = null;
        } else {
            PlayerData.playerCellHandler.TryPlaceBuilding(PlayerData.currentHoveredCell);
        }
    }

    public void OnRightClick(InputAction.CallbackContext context) {
        if (!PlayerData.inUI) {  
            if (PlayerData.isDragging) {  // Stop drag
                PlayerData.playerCellHandler.ClearDragPreview();
                PlayerData.isDragging = false;
                PlayerData.dragStartCell = null;
            } else {  // Reset the selected building
                PlayerData.selectedBuildingIndex = -1;
                PlayerData.playerCellHandler.UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.hoverMaterial, true);
            }
        }
    }

    #region Camera Controls
    public void OnMove(InputAction.CallbackContext context) {
        Vector2 movementInput = (context.ReadValue<Vector2>()).normalized;
        Vector3 movementAdjusted = new Vector3(movementInput.x, 0, movementInput.y);
        
        PlayerData.rigidbody.velocity = movementAdjusted * Data.movementSpeed;
    }

    public void OnMouseScroll(InputAction.CallbackContext context) {
        float val = context.ReadValue<float>();
        PlayerData.cameraController.AdjustZoom(val);
    }
    #endregion
}
