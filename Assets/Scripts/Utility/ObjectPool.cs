// Generic Object Pool class
// Creates and stores a pool of Monobehavior objects


using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    List<T> pool = new List<T>();
    Transform parent;
    T prefab;

    // Constructor that stores relevant information
    public ObjectPool(T prefab, Transform transform) {
        this.prefab = prefab;

        parent = new GameObject(prefab.name + " Pool").transform;
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
        T created = MonoBehaviour.Instantiate(prefab, parent);
        pool.Add(created);
        return created;
    }
}