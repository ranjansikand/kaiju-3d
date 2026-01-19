// A decoration to be spawned in an empty tile


using UnityEngine;

[CreateAssetMenu(fileName = "Decor", menuName = "Decor", order = 1)]
public class Decor : ScriptableObject
{
    public Mesh mesh;
    public Material material;
}
