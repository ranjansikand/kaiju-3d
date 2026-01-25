using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxAcceleration = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float safetyBuffer = 0.7f;
    [SerializeField] private LayerMask layerMask;

    private List<Cell> path;
    private int currentWaypointIndex;
    private bool isMoving;

    private Vector3 velocity;
    private Vector3 currentDirection;
    private Vector3 lastDirection;

    private bool despawnCountdownRunning;

    private bool TooSlow => velocity.sqrMagnitude < 0.5f;

    public bool isTurning => currentDirection != lastDirection;

    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    #region Start / Stop

    public void StartJourney(List<Cell> roadPath) {
        path = roadPath;
        currentWaypointIndex = 0;
        velocity = Vector3.zero;
        currentDirection = lastDirection = Vector3.zero;
        despawnCountdownRunning = false;
        isMoving = true;

        StartCoroutine(SimulationTick());
        StartCoroutine(MovementTick());
    }

    private void EndJourney() {
        StopAllCoroutines();

        path = null;
        currentWaypointIndex = 0;
        isMoving = false;
        despawnCountdownRunning = false;
        velocity = Vector3.zero;
        currentDirection = lastDirection = Vector3.zero;

        gameObject.SetActive(false);
    }
    #endregion

    #region Coroutines
    // 20 FPS
    IEnumerator SimulationTick() {
        while (isMoving && path != null && path.Count > 0) {
            UpdateVelocityAndPath();
            yield return Data.carRefreshTime; // e.g. 0.05s
        }
    }

    // Every frame
    IEnumerator MovementTick() {
        while (isMoving) {
            transform.localPosition += velocity * Time.deltaTime;

            if (currentDirection != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            yield return null;
        }
    }

    IEnumerator CountdownToDespawn() {
        float timeElapsed = 0f;

        while (timeElapsed < 3f) {
            if (!TooSlow) {
                despawnCountdownRunning = false;
                yield break;
            }

            timeElapsed += Data.carFramerate;
            yield return Data.carRefreshTime;
        }

        EndJourney();
    }

    #endregion

    #region Core Logic

    void UpdateVelocityAndPath() {
        if (currentWaypointIndex >= path.Count) {
            EndJourney();
            return;
        }

        Cell currentCell = path[currentWaypointIndex];
        Vector3 baseTargetPos = currentCell.WorldPosition(PlayerData.grid.cellSize);

        Vector3 targetPos = CarAI.CalculateLanePosition(
            baseTargetPos,
            currentWaypointIndex,
            path
        );
        targetPos.y = transform.position.y;

        lastDirection = currentDirection;
        currentDirection = (targetPos - transform.position).normalized;

        float targetSpeed = GetTargetSpeed(currentDirection);
        Vector3 desiredVelocity = currentDirection * targetSpeed;

        float maxSpeedChange = maxAcceleration * Data.carFramerate;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (velocity.sqrMagnitude < 0.0001f)
            velocity = Vector3.zero;

        if (Vector3.Distance(transform.position, targetPos) < 0.25f)
            currentWaypointIndex++;

        if (TooSlow && !despawnCountdownRunning) {
            despawnCountdownRunning = true;
            StartCoroutine(CountdownToDespawn());
        }
    }

    float GetTargetSpeed(Vector3 direction) {
        RaycastHit[] hits = Physics.RaycastAll(
            transform.position,
            direction,
            10f,
            layerMask
        );

        float minDistance = float.MaxValue;

        foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject == gameObject)
                continue;

            Car otherCar = hit.collider.GetComponent<Car>();
            if (!otherCar)
                continue;

            if (CarAI.ShouldYield(
                currentDirection,
                otherCar.currentDirection,
                hit.distance,
                otherCar.velocity.magnitude
            )) {
                return 0f;
            }

            minDistance = Mathf.Min(minDistance, hit.distance);
        }

        if (minDistance > 9f)
            return maxSpeed;

        if (minDistance < 0.35f)
            return 0f;

        float safeSpeed = Mathf.Sqrt(2f * maxAcceleration * minDistance);
        return Mathf.Min(safeSpeed * safetyBuffer, maxSpeed);
    }
    #endregion
}
