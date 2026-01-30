// Holds public data
// Generally these are fields that would be adjusted in settings


using UnityEngine;

public static class Data {
    // Variables
    public static Vector2Int[] directions = new Vector2Int[] {
        new Vector2Int(0, 1),  // North
        new Vector2Int(1, 0),  // East
        new Vector2Int(0, -1),  // South
        new Vector2Int(-1, 0)  // West
    };

    public static Vector3 buildingSpawnOffset = new Vector3(0, 2, 0);

    public const float carFramerate = 0.05f;
    public static WaitForSeconds carRefreshTime = new WaitForSeconds(carFramerate);

    public static WaitForSeconds oneSecondDelay = new WaitForSeconds(1.0f);

    // Camera settings
    public static float zoomSpeed = 3;
    public static float movementSpeed = 6;
    public static float deadzoneSize = 6f;
    public static Vector2 fovRange = new Vector2(6, 120);
    public static Vector2 orthographicSizeRange = new Vector2(0.75f, 10f);

    // Audio Settings
    public static float sfxVolume = 0.5f;
}