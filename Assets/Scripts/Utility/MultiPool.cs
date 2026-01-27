// Generic Object Pool class
// Creates and stores a pool of Monobehavior objects


using System.Collections.Generic;
using UnityEngine;

public class MultiPool<T> where T : MonoBehaviour
{
    List<T> pool = new List<T>();
    Transform parent;
    T[] prefabs;

    // Constructor that stores relevant information
    public MultiPool(T[] prefabs, Transform transform) {
        this.prefabs = prefabs;

        parent = new GameObject("Multi Pool").transform;
        parent.SetParent(transform);
    }

    // Supply objects
    public T Get() {
        // Find an unused object
        if (pool.Count > 0) {
            for (int i = 0; i < pool.Count; i++) {
                if (pool[i].gameObject.activeSelf == false) {
                    pool[i].gameObject.SetActive(true);
                    return pool[i];
                }
            }
        }

        // Create a new object if none are available
        T toCreate = prefabs[Random.Range(0, prefabs.Length)];
        T created = MonoBehaviour.Instantiate(toCreate, parent);
        pool.Add(created);
        return created;
    }
}