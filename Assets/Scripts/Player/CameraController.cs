// Handles camera systems


using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public void AdjustZoom(float val) {
        if (virtualCamera.m_Lens.Orthographic) AdjustOrthographicSize(val);
        else AdjustFOV(val);
    }

    private void AdjustFOV(float val) {
        float adjustment = Mathf.Clamp(val, -Data.zoomSpeed, Data.zoomSpeed);

        virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(
            virtualCamera.m_Lens.FieldOfView + adjustment,
            Data.fovRange.x, Data.fovRange.y);
    }

    private void AdjustOrthographicSize(float val) {
        float adjustment = Mathf.Clamp(val, -Data.zoomSpeed, Data.zoomSpeed) / 10;

        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(
            virtualCamera.m_Lens.OrthographicSize + adjustment,
            Data.orthographicSizeRange.x, 
            Data.orthographicSizeRange.y);
    }
}
