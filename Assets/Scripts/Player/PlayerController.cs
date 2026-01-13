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
        PlayerData.inputActions.Player.Click.performed += OnClick;
        PlayerData.inputActions.Player.Zoom.performed += OnMouseScroll;
        PlayerData.inputActions.Player.Move.performed += OnMove;
        PlayerData.inputActions.Player.Move.canceled += OnMove;
    }
    
    void OnDisable() {
        PlayerData.inputActions.Player.Point.performed -= OnMouseMove;
        PlayerData.inputActions.Player.Click.performed -= OnClick;
        PlayerData.inputActions.Player.Zoom.performed -= OnMouseScroll;
        PlayerData.inputActions.Player.Move.performed -= OnMove;
        PlayerData.inputActions.Player.Move.canceled -= OnMove;
        PlayerData.inputActions.Disable();
    }
    
     public void OnMouseMove(InputAction.CallbackContext context) {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        PlayerData.playerCellHandler.HandleMouseHover(mousePosition);
    }

    public void OnMouseScroll(InputAction.CallbackContext context) {
        float val = context.ReadValue<float>();
        PlayerData.cameraController.AdjustZoom(val);
    }
    
    public void OnClick(InputAction.CallbackContext context) {
        PlayerData.playerCellHandler.TryPlaceBuilding();
    }

    public void OnMove(InputAction.CallbackContext context) {
        Vector2 movementInput = (context.ReadValue<Vector2>()).normalized;
        Vector3 movementAdjusted = new Vector3(movementInput.x, 0, movementInput.y);
        
        PlayerData.rigidbody.velocity = movementAdjusted * Data.movementSpeed;
    }
}
