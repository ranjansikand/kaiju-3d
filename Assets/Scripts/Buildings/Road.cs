using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building
{
    [Header("Road Types")]
    [SerializeField] Mesh noConnections;  // No neighbors
    [SerializeField] Mesh culdesac;  // One neighbor
    [SerializeField] Mesh connection, corner;  // Two neighbors
    [SerializeField] Mesh threeWayIntersection;  // Three neighbors
    [SerializeField] Mesh fourWayIntersection;  // Four neighbors

    private void CheckNeighbors(Cell cell) {
        bool[] neighbors = new bool[4];
        foreach (Vector2Int dir in Data.directions) {

        }
    }
}
