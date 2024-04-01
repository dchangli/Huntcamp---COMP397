using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BearSpawner : AbstractFactory
{
    public override void CreateAgent()
    {
        var agent = Instantiate(EnemyPrefab, SpawnLocation.position, SpawnLocation.rotation);
       // agent.GetComponent<Enemy>().UpdateMovement(); destinaition code
    }
}
