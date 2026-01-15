// Prevents world raycasts while in UI


using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastPanel : MonoBehaviour
{
    public void OnPointerEnter(PointerEventData data) {
        PlayerData.inUI = true;
        PlayerData.playerCellHandler.UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.normalMaterial);
    }

    public void OnPointerExit(PointerEventData data) {
        PlayerData.inUI = false;
    }
}
