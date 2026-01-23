// Steers the car from origin to destination


using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxAcceleration = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float laneOffset = 0.25f; // How far right to offset
    [SerializeField] private float safetyBuffer = 0.7f;

    [SerializeField] LayerMask layerMask;
    
    private List<Cell> path;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    Vector3 velocity = Vector3.zero;

    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void StartJourney(List<Cell> roadPath) {
        path = roadPath;
        currentWaypointIndex = 0;
        isMoving = true;

        StartCoroutine(UpdatePosition());
    }
    
    IEnumerator UpdatePosition() {
        while (!(!isMoving || path == null || path.Count == 0)) {
            MoveAlongPath();
            yield return null;
        }
    }
    
    void MoveAlongPath() {
        if (currentWaypointIndex >= path.Count) {
            isMoving = false;
            Destroy(gameObject);
            return;
        }
        
        Cell currentCell = path[currentWaypointIndex];
        Vector3 baseTargetPos = currentCell.WorldPosition(PlayerData.grid.cellSize);

        Vector3 targetPos = CalculateLanePosition(baseTargetPos, currentWaypointIndex);
        targetPos.y = transform.position.y;
        
        HandleMovement(targetPos);
        HandleRotation(targetPos);

        
        // Check if reached waypoint
        if (Vector3.Distance(transform.position, targetPos) < 0.25f) {
            currentWaypointIndex++;
        }
    }

    void HandleMovement(Vector3 targetPos) {        
        Vector3 direction = (targetPos - transform.position).normalized;
        
        // Determine target speed based on obstacles
        float targetSpeed = GetTargetSpeed(direction);
        
        Vector3 desiredVelocity = direction * targetSpeed;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        // Smoothly adjust velocity toward desired velocity
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange); 

        Vector3 displacement = velocity * Time.deltaTime;
        transform.localPosition += displacement;
    }

    float GetTargetSpeed(Vector3 direction) {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 10, layerMask);
        
        float minDistance = float.MaxValue;
        
        foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject != gameObject) {
                minDistance = Mathf.Min(minDistance, hit.distance);
            }
        }
        
        // No obstacles nearby
        if (minDistance > 9) {
            return maxSpeed;
        } else if (minDistance < 0.35f) return 0;
        
        // Calculate safe speed based on stopping distance
        // v² = 2 * a * d  →  v = sqrt(2 * a * d)
        float safeSpeed = Mathf.Sqrt(2 * maxAcceleration * minDistance);
        
        // Clamp to maxSpeed and add safety margin
        return Mathf.Min(safeSpeed * safetyBuffer, maxSpeed);
    }

    void HandleRotation(Vector3 targetPos) {
        Vector3 direction = (targetPos - transform.position).normalized;

        // Rotate to face direction
        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    Vector3 CalculateLanePosition(Vector3 basePosition, int waypointIndex) {
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