using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class ShipController : MonoBehaviour
{

    public float moveSpeed = 1.5f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;

    public float timeBetweenShots = 2.0f;
    public float projectileSpeed = 8f;

    public GameObject projectilePrefab;
    public int poolSize = 10;

    private readonly Queue<Projectile> availableProjectiles = new Queue<Projectile>();
    public Transform poolParent;
    private Transform player;
    private float timeSinceLastShot;
    private Vector3 startPosition;
    private Vector3 initPosition;

    bool startFire;


    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        player = playerObject.transform;

        PrepopulatePool();
        initPosition = transform.position;
        startPosition = player.position;
        timeSinceLastShot = timeBetweenShots;
    }

    private void OnEnable()
    {
        startFire = false;
        Invoke(nameof(StartFire), 3f);
    }


    void StartFire()
    {
        startFire = true;
    }

    void MoveToArena()
    {
        Vector3 targetDirection = (player.position - transform.position).normalized;

        Vector3 perpendicularDirection = Vector3.Cross(targetDirection, Vector3.forward);

        float waveOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );

        newPosition += perpendicularDirection * waveOffset * Time.deltaTime;

        transform.position = newPosition;
    }



    void Update()
    {
        if (startFire)
        {
            HandleFloatingMovement();

            HandleShooting();
        }
        else
        {
            MoveToArena();
        }


    }

    void HandleFloatingMovement()
    {
        float newX = startPosition.x + Mathf.PingPong(Time.time * moveSpeed, 5f) - 2.5f;

        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        Vector3 targetPosition = new Vector3(newX, newY, startPosition.z);
         transform.position = Vector3.MoveTowards(transform.position,targetPosition, moveSpeed * Time.deltaTime);  

    }

    void HandleShooting()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= timeBetweenShots)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
    }

    void Shoot()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        Projectile projectileScript = GetProjectile();
        projectileScript.transform.position = transform.position + (Vector3)direction * 0.5f;
        projectileScript.Initialize(direction, projectileSpeed, this);

    }

    private void PrepopulatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewProjectile();
        }
    }

    private Projectile CreateNewProjectile()
    {
        GameObject newGO = Instantiate(projectilePrefab, poolParent);
        Projectile newProjectile = newGO.GetComponent<Projectile>();

        newGO.SetActive(false);
        availableProjectiles.Enqueue(newProjectile);

        return newProjectile;
    }

    public Projectile GetProjectile()
    {
        Projectile projectileToUse;
        projectileToUse = availableProjectiles.Dequeue();
        projectileToUse.gameObject.SetActive(true);
        return projectileToUse;
    }

    public void ReturnToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.transform.localPosition = Vector3.zero;
        availableProjectiles.Enqueue(projectile);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameManager.instance.UpdateScore(10);
            transform.position = initPosition;
            gameObject.SetActive(false);
            GameManager.instance.RespawnShip();

        }
    }
}