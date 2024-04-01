using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private List<AbstractFactory> _transportFactories;
    private AbstractFactory _factory;
    private bool StartSpawner = false;
    private int EnemyCount =0;
    [SerializeField] int EnemyLimit = 2;

    private void Start()
    {
        _factory = _transportFactories[0];
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        
            StartCoroutine(GenerateAgents());
        
        if(StartSpawner == true)
        {
            StopCoroutine(GenerateAgents());
        }
    }

    private IEnumerator GenerateAgents()
    {
        var time = new WaitForSeconds(_factory.SpawnTimer);
        while (true && EnemyCount < 2)
        {
            _factory.CreateAgent();
            StartSpawner = true;
            EnemyCount++;
            yield return time;
        }
    }
}
