using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float laneOffset = 0.25f; // How far right to offset
    
    private List<Cell> path;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    
    public void StartJourney(List<Cell> roadPath)
    {
        path = roadPath;
        currentWaypointIndex = 0;
        isMoving = true;
    }
    
    void Update()
    {
        if (!isMoving || path == null || path.Count == 0) return;
        
        MoveAlongPath();
    }
    
    void MoveAlongPath()
    {
        if (currentWaypointIndex >= path.Count)
        {
            isMoving = false;
            Destroy(gameObject);
            return;
        }
        
        Cell currentCell = path[currentWaypointIndex];
        Vector3 baseTargetPos = currentCell.WorldPosition(PlayerData.grid.cellSize);
        
        // Calculate lane offset based on travel direction
        Vector3 targetPos = CalculateLanePosition(baseTargetPos, currentWaypointIndex);
        targetPos.y = transform.position.y;
        
        // Move toward target
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // Rotate to face direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Check if reached waypoint
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }
    
    Vector3 CalculateLanePosition(Vector3 basePosition, int waypointIndex)
    {
        // No offset for last waypoint (arrive at center)
        if (waypointIndex >= path.Count - 1)
            return basePosition;
        
        // Get direction to NEXT waypoint
        Cell nextCell = path[waypointIndex + 1];
        Vector3 nextPos = nextCell.WorldPosition(PlayerData.grid.cellSize);
        Vector3 forward = (nextPos - basePosition).normalized;
        
        // Calculate right vector (perpendicular, 90° clockwise in XZ plane)
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        
        // Offset to the right
        return basePosition + right * laneOffset;
    }
}