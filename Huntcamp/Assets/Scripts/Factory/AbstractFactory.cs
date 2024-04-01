using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFactory : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform SpawnLocation;
    public float SpawnTimer = 0.10f;
    public Transform EnemyDestination; 

    public abstract void CreateAgent();



}
