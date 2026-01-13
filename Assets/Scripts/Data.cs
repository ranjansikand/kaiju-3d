// Holds public data
// Generally these are fields that would be adjusted in settings


using UnityEngine;

public static class Data {
    public static Vector2Int[] directions = new Vector2Int[] {
        new Vector2Int(0, 1),  // North
        new Vector2Int(1, 0),  // East
        new Vector2Int(0, -1),  // South
        new Vector2Int(-1, 0)  // West
    };

    // Camera settings
    public static float zoomSpeed = 3;
    public static float movementSpeed = 3;
}