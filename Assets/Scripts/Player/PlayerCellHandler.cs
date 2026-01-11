using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerCellHandler : MonoBehaviour
{    
    public void HandleMouseHover(Vector2 screenPosition) {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit)) {
            Vector3 hitPoint = hit.point;
            Cell cell = PlayerData.gameManager.grid.WorldToCell(hitPoint);
            
            if (cell != PlayerData.currentHoveredCell) {
                // Unhighlight previous cell 
                if (PlayerData.currentHoveredCell != null && PlayerData.currentHoveredCell.visual != null) {
                    PlayerData.currentHoveredCell.visual.GetComponent<Renderer>().material = PlayerData.normalMaterial;
                }
                
                // Highlight new cell
                PlayerData.currentHoveredCell = cell;
                if (PlayerData.currentHoveredCell != null && PlayerData.currentHoveredCell.visual != null) {
                    PlayerData.currentHoveredCell.visual.GetComponent<Renderer>().material = PlayerData.hoverMaterial;
                }
            }
        } else {
            // Mouse not over grid - unhighlight
            if (PlayerData.currentHoveredCell != null && PlayerData.currentHoveredCell.visual != null) {
                PlayerData.currentHoveredCell.visual.GetComponent<Renderer>().material = PlayerData.normalMaterial;
            }
            PlayerData.currentHoveredCell = null;
        }
    }
    
    public void TryPlaceBuilding() {
        if (PlayerData.currentHoveredCell == null || PlayerData.currentHoveredCell.IsOccupied) {
            Debug.Log("Can't place building here!");
            return;
        }
        
        if (PlayerData.selectedBuildingIndex >= PlayerData.buildingPrefabs.Length) {
            Debug.LogWarning("No building selected!");
            return;
        }
        
        // Instantiate the building
        Building buildingPrefab = PlayerData.buildingPrefabs[PlayerData.selectedBuildingIndex];
        
        if (PlayerData.inventory < buildingPrefab.cost) {
            return;
        } else {
            PlayerData.inventory = PlayerData.inventory - buildingPrefab.cost;
        }

        Vector3 spawnPos = PlayerData.currentHoveredCell.WorldPosition(PlayerData.gameManager.grid.cellSize);
        
        Building newBuilding = Instantiate(buildingPrefab, spawnPos, Quaternion.identity);
        newBuilding.occupiedCell = PlayerData.currentHoveredCell;
        
        // Update cell
        PlayerData.currentHoveredCell.building = newBuilding;
        Debug.Log($"Placed {newBuilding.buildingName} at ({PlayerData.currentHoveredCell.x}, {PlayerData.currentHoveredCell.y})");
    }
}