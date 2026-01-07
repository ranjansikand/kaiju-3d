// 


using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Grid grid;

    void Start()
    {
        grid  = new Grid(25, 25);    
    }
}
