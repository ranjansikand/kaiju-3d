// Global static functions


using UnityEngine;

public static class Utility{
    public static bool InBounds(Vector2Int var) {
        if (var.x >= 0 && var.y >= 0 && 
            var.x < PlayerData.grid.width && 
            var.y < PlayerData.grid.height) return true;

        return false;
    }

    public static Vector3 GetCellPos(int x, int y) {
        return new Vector3(
            x * PlayerData.grid.cellSize, 0,
            y * PlayerData.grid.cellSize);
    }

    public static bool IsSelectedBuildingAffordable() {
        return PlayerData.inventory >= PlayerData.buildings[PlayerData.selectedBuildingIndex].cost;
    }

    public static Cell GetNearestCell(Vector3 pos) {
        Vector2Int convertedPos = new Vector2Int(
            (int)Mathf.Clamp(pos.x, 0, PlayerData.gridBounds.x - 1),
            (int)Mathf.Clamp(pos.z, 0, PlayerData.gridBounds.y - 1)
        );

        return PlayerData.grid.Cells[convertedPos.x, convertedPos.y];
    }
}