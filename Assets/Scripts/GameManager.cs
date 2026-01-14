// Creates the world


using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Grid grid;

    [SerializeField] GameObject gridCellVisual;

    void Start(){
        grid  = new Grid(25, 25, gridCellVisual);    
    }
}
