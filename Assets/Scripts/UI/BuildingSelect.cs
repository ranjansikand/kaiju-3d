// UI tray that holds BuildingIcons


using System.Collections.Generic;
using UnityEngine;

public class BuildingSelect : MonoBehaviour
{
    [SerializeField] BuildingIcon buildingIcon;
    [SerializeField] BuildingIconGenerator buildingIconGenerator;
    [SerializeField] Transform iconFixPoint;

    static List<BuildingIcon> buildingIcons = new List<BuildingIcon>();
    static BuildingIcon selectedBuilding;

    private void Start() {
        for (int i = 0; i < PlayerData.buildings.Length; i++) {
            AddNewBuilding(PlayerData.buildings[i], i);
        }

        UpdateSelectedBuilding();
    }

    public static void UpdateSelectedBuilding() {
        if (PlayerData.selectedBuildingIndex > -1) {
            if (selectedBuilding != buildingIcons[PlayerData.selectedBuildingIndex]) {
                if (selectedBuilding != null) selectedBuilding.UpdateSelection(false);

                selectedBuilding = buildingIcons[PlayerData.selectedBuildingIndex];
                selectedBuilding.UpdateSelection(true);
            }
        } else {
            selectedBuilding.UpdateSelection(false);
            selectedBuilding = null;
        }
    }

    public void AddNewBuilding(Building building, int index) {
        BuildingIcon newBuildingIcon = Instantiate(buildingIcon, iconFixPoint);
        Sprite sprite = buildingIconGenerator.GetIcon(building);

        newBuildingIcon.Set(building, index, sprite);
        buildingIcons.Add(newBuildingIcon);
    }
}
