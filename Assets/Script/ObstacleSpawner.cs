using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    // Success return 1
    // Error return -1
    public int SpawnObstacles(int x, int y){
        GameObject newObstacle = Instantiate(obstaclePrefab);
        newObstacle.transform.position = new Vector2(x, y);
        return 1;
    }
    
}
