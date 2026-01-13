// Global static functions


using UnityEngine;

public static class Utility{
    public static bool InBounds(Vector2Int var) {
        if (var.x >= 0 && var.y >= 0 && 
            var.x < PlayerData.gameManager.grid.width && 
            var.y < PlayerData.gameManager.grid.height) return true;

        return false;
    }
}