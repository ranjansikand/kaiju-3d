// Holds player's resources


using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private void Awake() {
        inventory = new Resources(20, 20);
    }
    
    public static Resources inventory;
}
