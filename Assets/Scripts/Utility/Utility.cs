// Global static functions


using UnityEngine;

public static class Utility{
    public static bool InBounds(Vector2Int var) {
        if (var.x >= 0 && var.y >= 0 && 
            var.x < PlayerData.gameManager.grid.width && 
            var.y < PlayerData.gameManager.grid.height) return true;

        return false;
    }

    public static Vector3 GetCellPos(int x, int y) {
        return new Vector3(
            x * PlayerData.gameManager.grid.cellSize, 0,
            y * PlayerData.gameManager.grid.cellSize);
    }

    public static bool IsSelectedBuildingAffordable() {
        return PlayerData.inventory >= PlayerData.buildings[PlayerData.selectedBuildingIndex].cost;
    }

    public static Cell GetNearestCell(Vector3 pos) {
        Vector2Int convertedPos = new Vector2Int(
            (int)Mathf.Clamp(pos.x, 0, PlayerData.gridBounds.x - 1),
            (int)Mathf.Clamp(pos.z, 0, PlayerData.gridBounds.y - 1)
        );

        return PlayerData.gameManager.grid.Cells[convertedPos.x, convertedPos.y];
    }
}