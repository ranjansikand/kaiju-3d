// A visual component to represent a building in UI


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BuildingIcon : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] Image frame;

    public Building building { get; set; }
    public int index { get; set; }

    public void Set(Building building, int index, Sprite sprite) {
        this.building = building;
        this.index = index;
        icon.sprite = sprite;
    }

    public void UpdateSelection(bool selected) {
        Color color = selected ? Color.yellow : Color.white;
        float scale = selected ? 1.125f : 1f;

        frame.transform.DOScale(scale, 0.25f);
        frame.DOBlendableColor(color, 0.25f);
    }

    public void OnPointerClick(PointerEventData data) {
        PlayerData.selectedBuildingIndex = index;

        // Effects
        BuildingSelect.UpdateSelectedBuilding();
        UISFX.Play(PlayerData.selectSound, true);
        transform.DOScale(1.1f, 0.125f).OnComplete(
            () => transform.DOScale(1f, 0.25f)
        );
    }

    public void OnPointerEnter(PointerEventData data) {
        PlayerData.inUI = true;
        PlayerData.playerCellHandler.UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.normalMaterial);
        transform.DOScale(0.9f, 0.25f);
    }

    public void OnPointerExit(PointerEventData data) {
        PlayerData.inUI = false;
        transform.DOScale(1f, 0.25f);
    }
}
