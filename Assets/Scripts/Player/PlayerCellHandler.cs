// Handles direct cell interactions, like hover and click


using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerCellHandler : MonoBehaviour
{
    public void HandleMouseHover(Vector2 screenPosition) {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit)) {
            Vector3 hitPoint = hit.point;
            Cell cell = PlayerData.gameManager.grid.WorldToCell(hitPoint);
            
            if (cell != PlayerData.currentHoveredCell && cell != null) {
                // Don't update highlight if dragging (drag preview handles it)
                if (!PlayerData.isDragging) {
                    // Unhighlight
                    UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.normalMaterial);
                    PlayerData.currentHoveredCell = cell;

                    if (PlayerData.selectedBuildingIndex > -1) {
                        Building building = PlayerData.buildings[PlayerData.selectedBuildingIndex];
                        if (cell.IsBuildable() && Utility.IsSelectedBuildingAffordable())
                            UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.buildableMaterial, true);
                        else UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.notbuildableMaterial, true);
                    } else {
                        // Yellow highlight
                        UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.hoverMaterial, true);
                    }
                    

                    
                } else {
                    PlayerData.currentHoveredCell = cell;
                }
            }
        } else {
            if (!PlayerData.isDragging) {
                // Unhighlight
                UpdateHighlightedCell(PlayerData.currentHoveredCell, PlayerData.normalMaterial);
            }
            PlayerData.currentHoveredCell = null;
        }
    }

    public void UpdateHighlightedCell(Cell cell, Material material, bool lift = false) {
        if (cell != null && cell.visual != null) {
            cell.visual.GetComponent<Renderer>().material = material;
            
            Vector3 pos = Utility.GetCellPos(cell.x, cell.y);
            cell.visual.transform.DOKill();

            if (lift) cell.visual.transform.DOLocalMove(pos + new Vector3(0, 0.125f, 0), 0.25f);
            else cell.visual.transform.DOLocalMove(pos, 0.125f);
        }
    }
    
    public void UpdateDragPreview() {
        if (PlayerData.dragStartCell == null || PlayerData.currentHoveredCell == null) return;
        
        ClearDragPreview();
        
        // Get L-shaped path
        PlayerData.dragPathCells = GetOrthogonalPath(PlayerData.dragStartCell, PlayerData.currentHoveredCell);
        
        // Highlight cells
        Building building = PlayerData.buildings[PlayerData.selectedBuildingIndex];
        Resources sum = new Resources(0, 0);

        foreach (Cell cell in PlayerData.dragPathCells) {
            sum = sum + building.cost;

            if (cell.IsBuildable() && PlayerData.inventory > sum) 
                UpdateHighlightedCell(cell, PlayerData.buildableMaterial, true);
            else UpdateHighlightedCell(cell, PlayerData.notbuildableMaterial, true);
        }
    }
    
    public void PlaceBuildingsAlongPath() {
        Building selectedBuilding = PlayerData.buildings[PlayerData.selectedBuildingIndex];
               
        // Place all buildings
        foreach (Cell cell in PlayerData.dragPathCells) {
            TryPlaceBuilding(cell);
        }
    }
    
    public void ClearDragPreview() {
        foreach (Cell cell in PlayerData.dragPathCells) {
            UpdateHighlightedCell(cell, PlayerData.normalMaterial);
        }
        PlayerData.dragPathCells.Clear();
    }
    
    List<Cell> GetOrthogonalPath(Cell start, Cell end) {
        List<Cell> path = new List<Cell>();
        
        int currentX = start.x;
        int currentY = start.y;
        int targetX = end.x;
        int targetY = end.y;
        
        // Move horizontally first
        int xDir = targetX > currentX ? 1 : (targetX < currentX ? -1 : 0);
        while (currentX != targetX) {
            Vector2Int pos = new Vector2Int(currentX, currentY);
            if (Utility.InBounds(pos)) {
                path.Add(PlayerData.gameManager.grid.Cells[currentX, currentY]);
            }
            currentX += xDir;
        }
        
        // Then move vertically
        int yDir = targetY > currentY ? 1 : (targetY < currentY ? -1 : 0);
        while (currentY != targetY) {
            Vector2Int pos = new Vector2Int(currentX, currentY);
            if (Utility.InBounds(pos)) {
                path.Add(PlayerData.gameManager.grid.Cells[currentX, currentY]);
            }
            currentY += yDir;
        }
        
        // Add final cell
        if (Utility.InBounds(new Vector2Int(currentX, currentY))) {
            path.Add(PlayerData.gameManager.grid.Cells[currentX, currentY]);
        }
        
        return path;
    }
    
    public void TryPlaceBuilding(Cell cell) {
        if (cell == null || !cell.IsBuildable()) {
            Debug.Log("Can't place building here!");
            return;
        } else if (PlayerData.selectedBuildingIndex >= PlayerData.buildings.Length) {
            Debug.LogWarning("No building selected!");
            return;
        }
        
        Building building = PlayerData.buildings[PlayerData.selectedBuildingIndex];
        
        if (PlayerData.inventory < building.cost) return;
        else PlayerData.inventory = PlayerData.inventory - building.cost;
        
        SpawnedBuilding newBuilding = Instantiate(
            PlayerData.buildingPrefab,
            cell.visual.transform);
        newBuilding.transform.localPosition += new Vector3(0, 2, 0);

        newBuilding.building = building;
        cell.PlaceBuilding(newBuilding);

        Debug.Log($"Placed {building.buildingName} at ({cell.x}, {cell.y})");
    }
}