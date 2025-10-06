using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{
    
    public GameObject asteroid;
    public int poolSize = 15;
    private Queue<GameObject> asteroidPool;
    private Queue<GameObject> asteroidPoolSize2;
    private Queue<GameObject> asteroidPoolSize3;

    
    public float spawnInterval = 3f;
    public float boundaryOffset = 1.5f; 
    public float speed = 4f;

    
    private float minX, maxX, minY, maxY;

    private float time;

    public static EnemySpawnner Instance;   
    private void Awake()
    {
        Instance = this;
        InitializeEnemyPool();
    }

    private void Start()
    {
        CalculateScreenBounds();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > spawnInterval)
        {
            time = 0;
            SpawnAsteroid();
        }

    }

    private void InitializeEnemyPool()
    {
        asteroidPool = new Queue<GameObject>();
        asteroidPoolSize2 = new Queue<GameObject>();
        asteroidPoolSize3 = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(asteroid,this.transform);
            enemy.SetActive(false);
            asteroidPool.Enqueue(enemy);

            
        }

        for (int i = 0; i <= poolSize * 2; i++)
        {
            GameObject enemy = Instantiate(asteroid, this.transform);
            enemy.transform.localScale = asteroid.transform.localScale*0.5f;
            enemy.SetActive(false);
            asteroidPoolSize2.Enqueue(enemy);

        }
        for (int i = 0; i <= poolSize * 4; i++)
        {
            GameObject enemy = Instantiate(asteroid, this.transform);
            enemy.transform.localScale = asteroid.transform.localScale * 0.25f;
            enemy.SetActive(false);
            asteroidPoolSize3.Enqueue(enemy);

        }
    }

    private void CalculateScreenBounds()
    {
        float zDistance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, zDistance));
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        maxX = topRight.x + boundaryOffset;
        minX = bottomLeft.x - boundaryOffset;
        maxY = topRight.y + boundaryOffset;
        minY = bottomLeft.y - boundaryOffset;
    }


    public GameObject GetPooledAsteroid()
    {

        GameObject enemy = asteroidPool.Dequeue();
        enemy.SetActive(true);
        return enemy;

    }

    public GameObject GetPooledAsteroid2()
    {

        GameObject enemy = asteroidPoolSize2.Dequeue();
        enemy.SetActive(true);
        return enemy;

    }
    public GameObject GetPooledAsteroid3()
    {

        GameObject enemy = asteroidPoolSize3.Dequeue();
        enemy.SetActive(true);
        return enemy;

    }

    public void ReturnAsteroidToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        asteroidPool.Enqueue(enemy);
    }
    public void ReturnAsteroidToPool2(GameObject enemy)
    {
        enemy.SetActive(false);
        asteroidPoolSize2.Enqueue(enemy);
    }
    public void ReturnAsteroidToPool3(GameObject enemy)
    {
        enemy.SetActive(false);
        asteroidPoolSize3.Enqueue(enemy);
    }
    private void SpawnAsteroid()
    {
        GameObject astrd = GetPooledAsteroid();

        Vector3 spawnPosition = GetRandomOffScreenPosition();
        astrd.transform.position = spawnPosition;

        Vector3 directionToOrigin = (Vector3.zero - spawnPosition).normalized;

        float randomAngle = Random.Range(-15f, 15f);
        Vector3 finalDirection = Quaternion.Euler(0, 0, randomAngle) * directionToOrigin;
        Asteroid aster = astrd.GetComponent<Asteroid>();
        aster.SetMovementData(finalDirection,speed,1);
    }

    public void SpawnAsteroidRandom(Vector3 pos,int size)
    {
        float randomAngle = Random.Range(0f, 360f);
        float angleRad = randomAngle * Mathf.Deg2Rad;

        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);

        GameObject astrd1;
        GameObject astrd2;
        if(size == 2)
        {
             astrd1 = GetPooledAsteroid2();
             astrd2 = GetPooledAsteroid2();
        }
        else
        {
             astrd1 = GetPooledAsteroid3();
             astrd2 = GetPooledAsteroid3();
        }
        Vector3 randomDirection = new Vector3(x, y, 0).normalized;

        Vector3 inverseDirection = -randomDirection;
        Asteroid astroid1 = astrd1.GetComponent<Asteroid>();
        Asteroid astroid2 = astrd2.GetComponent<Asteroid>();
        astrd1.transform.position = pos;
        astrd2.transform.position = pos;
        astroid1.SetMovementData(randomDirection, speed*1.2f,size);
        astroid2.SetMovementData(inverseDirection, speed*1.2f, size);

    }
    private Vector3 GetRandomOffScreenPosition()
    {
        Vector3 position = Vector3.zero;
        int side = Random.Range(0, 4);

        if (side == 0) 
        {
            position = new Vector3(Random.Range(minX + boundaryOffset, maxX - boundaryOffset), maxY, 0);
        }
        else if (side == 1) 
        {
            position = new Vector3(Random.Range(minX + boundaryOffset, maxX - boundaryOffset), minY, 0);
        }
        else if (side == 2) 
        {
            position = new Vector3(minX, Random.Range(minY + boundaryOffset, maxY - boundaryOffset), 0);
        }
        else 
        {
            position = new Vector3(maxX, Random.Range(minY + boundaryOffset, maxY - boundaryOffset), 0);
        }

        return position;
    }


    public void DisableAll()
    {
        foreach(Transform t in this.transform)
        {
            t.gameObject.SetActive(false);
        }
    }
}
