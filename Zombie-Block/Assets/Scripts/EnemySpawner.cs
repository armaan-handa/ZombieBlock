using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnInterval = 5f;
    public float maxDistance = 30f;
    public float minDistance = 20f;
    public Transform playerPos;
    float timeTillNextSpawn = 0f;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        timeTillNextSpawn -= Time.deltaTime;
        if(timeTillNextSpawn <= 0)
        {
            // Generate a random point in between two circles of radius minDistance and maxDistance;
            float x, z, r, t;
            r = Random.Range(minDistance, maxDistance);
            t = Random.Range(0f, 1f) * 2 * Mathf.PI;
            x = playerPos.position.x + r*Mathf.Sin(t);
            z = playerPos.position.z + r*Mathf.Cos(t);
            
            // spawn enemy at point
            Vector3 spawnPoint = new Vector3(x, 0, z);
            GameObject.Instantiate(enemy, spawnPoint, playerPos.rotation);

            timeTillNextSpawn = spawnInterval;
        }
    }   
}
