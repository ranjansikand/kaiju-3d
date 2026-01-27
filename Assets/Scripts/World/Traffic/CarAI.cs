// "Brain" that tells cars when to yield


using System.Collections.Generic;
using UnityEngine;

public static class CarAI
{
    // Tunables
    public static float sameLaneDot = -0.7f;   // Facing mostly toward me
    public static float leftTurnDot = 0.3f;    // Turning threshold
    private static float intersectionTime = 1.2f; // Seconds to clear turn
    public static float laneOffset = 0.35f; // How far right to offset

    public static bool ShouldYield(
        Vector3 myDirection,
        Vector3 otherDirection, 
        float distanceToOther,
        float otherSpeed
    )
    {
        myDirection.y = 0;
        otherDirection.y = 0;

        myDirection.Normalize();
        otherDirection.Normalize();

        // 1. Am I actually turning left?
        float turnSign = Vector3.Cross(myDirection, otherDirection).y;
        bool turningLeft = turnSign > leftTurnDot;

        if (!turningLeft)
            return false;

        // 2. Is the other car coming toward the intersection?
        float facingDot = Vector3.Dot(myDirection, otherDirection);

        if (facingDot > sameLaneDot)
            return false; // same direction or irrelevant

        // 3. Time-based gap check
        float timeToReach = distanceToOther / Mathf.Max(otherSpeed, 0.01f);

        return timeToReach < intersectionTime;
    }

    public static Vector3 CalculateLanePosition(Vector3 basePosition, int waypointIndex, List<Cell> path) {
        // No offset for last waypoint
        if (waypointIndex >= path.Count - 1)
            return basePosition;
        
        // Get the actual direction the car needs to travel
        // (from current waypoint to next waypoint)
        Cell currentCell = path[waypointIndex];
        Cell nextCell = path[waypointIndex + 1];
        
        Vector3 currentPos = currentCell.WorldPosition(PlayerData.grid.cellSize);
        Vector3 nextPos = nextCell.WorldPosition(PlayerData.grid.cellSize);
        Vector3 travelDirection = (nextPos - currentPos).normalized;
        
        // Right side is 90° clockwise from travel direction
        Vector3 right = new Vector3(travelDirection.z, 0, -travelDirection.x);
        
        // Offset the waypoint position to the right
        return basePosition + right * laneOffset;
    }
}
