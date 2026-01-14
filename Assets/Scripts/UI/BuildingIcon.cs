// A visual component to represent a building


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingIcon : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image icon;
    public Building building { get; set; }
    public int index { get; set; }

    public void Set(Building building, int index, Sprite sprite) {
        this.building = building;
        this.index = index;
        icon.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData data) {
        PlayerData.selectedBuildingIndex = index;
    }

    public void OnPointerEnter(PointerEventData data) {
        PlayerData.inUI = true;
        PlayerData.playerCellHandler.UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.normalMaterial);
    }

    public void OnPointerExit(PointerEventData data) {
        PlayerData.inUI = false;
        PlayerData.playerCellHandler.UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.hoverMaterial);
    }
}
