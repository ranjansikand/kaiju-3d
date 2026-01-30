using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] ResourceTracker gold, wood;

    private void OnEnable() {
        PlayerData.resourcesUpdated += UpdateResources;
    }

    private void OnDisable() {
        PlayerData.resourcesUpdated -= UpdateResources;
    }

    private void UpdateResources() {
        gold.UpdateValue(PlayerData.inventory.gold);
        wood.UpdateValue(PlayerData.inventory.wood);
    }
}
