using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public int numberOfEnemies;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemy);
        numberOfEnemies++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
