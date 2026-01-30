// Updates a resource


using System.Collections;
using UnityEngine;
using TMPro;

public class ResourceTracker : MonoBehaviour
{
    [SerializeField] TMP_Text valueTracker;
    int displayedValue = -1;
    int targetValue = -1;

    Coroutine updateRoutine;

    public void UpdateValue(int newValue) {
        targetValue = newValue;

        // Only start coroutine if it's not already running
        if (updateRoutine == null && targetValue != displayedValue) {
            updateRoutine = StartCoroutine(GoToValue());
        }
    }

    IEnumerator GoToValue() {
        while (displayedValue != targetValue) {
            displayedValue = displayedValue > targetValue ? 
                displayedValue - 1 : 
                displayedValue + 1;
            valueTracker.text = displayedValue.ToString();

            yield return Data.carRefreshTime;
        }

        displayedValue = targetValue;
        updateRoutine = null; // Mark as finished
    }
}