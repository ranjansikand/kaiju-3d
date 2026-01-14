using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelect : MonoBehaviour
{
    [SerializeField] BuildingIcon buildingIcon;
    [SerializeField] BuildingIconGenerator buildingIconGenerator;
    List<BuildingIcon> buildingIcons = new List<BuildingIcon>();

    private void Start() {
        for (int i = 0; i < PlayerData.buildings.Length; i++) {
            BuildingIcon newBuildingIcon = Instantiate(buildingIcon, transform);
            Sprite sprite = buildingIconGenerator.GetIcon(PlayerData.buildings[i]);

            newBuildingIcon.Set(PlayerData.buildings[i], i, sprite);
            buildingIcons.Add(newBuildingIcon);
        }
    }
}
