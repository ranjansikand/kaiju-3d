// Holds public data
// Generally these are fields that would be adjusted in settings


using UnityEngine;

public static class Data {
    public static Vector2Int[] directions = new Vector2Int[] {
        new Vector2Int(1, 0),  // Up
        new Vector2Int(0, 1),  // Right
        new Vector2Int(-1, 0),  // Down
        new Vector2Int(0, -1)  // Left
    };

    // Camera settings
    public static float zoomSpeed = 3;
    public static float movementSpeed = 3;
}