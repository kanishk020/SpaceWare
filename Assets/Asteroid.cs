using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector3 moveDirection;
    private float currentSpeed;

    private float topBound;
    private float bottomBound;
    private float rightBound;
    private float leftBound;

    float time,timeCollider;

    private Collider2D coll;

    public int sizeAsteroid = 0;
    void Awake()
    {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;

        Vector3 topCorner = new Vector3(0.5f, 1.0f, distanceToCamera);
        Vector3 bottomCorner = new Vector3(0.5f, 0.0f, distanceToCamera);
        Vector3 rightCorner = new Vector3(1.0f, 0.5f, distanceToCamera);
        Vector3 leftCorner = new Vector3(0.0f, 0.5f, distanceToCamera);

        topBound = Camera.main.ViewportToWorldPoint(topCorner).y;
        bottomBound = Camera.main.ViewportToWorldPoint(bottomCorner).y;
        rightBound = Camera.main.ViewportToWorldPoint(rightCorner).x;
        leftBound = Camera.main.ViewportToWorldPoint(leftCorner).x;
        coll = transform.GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        coll.enabled = false;
    }
    private void OnDisable()
    {
        sizeAsteroid = 0;
    }
    public void SetMovementData(Vector3 direction, float speed,int size)
    {
        sizeAsteroid = size;
        moveDirection = direction.normalized;
        currentSpeed = speed;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            time += Time.deltaTime;
            timeCollider += Time.deltaTime;
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
            if (time > 1f)
            {
                time = 0f;
                coll.enabled = true;
                Vector3 pos = transform.position;
                if (pos.y > topBound || pos.y < bottomBound || pos.x > rightBound || pos.x < leftBound)
                {
                    EnemySpawnner.Instance.ReturnAsteroidToPool(this.gameObject);
                }
            }
            if(sizeAsteroid>1 && timeCollider > 0.1f)
            {
                timeCollider = 0;
                coll.enabled = true;
            }
            
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if(sizeAsteroid == 1)
            {
                EnemySpawnner.Instance.ReturnAsteroidToPool(this.gameObject);
                EnemySpawnner.Instance.SpawnAsteroidRandom(transform.position, 2);
                GameManager.instance.UpdateScore(2);
            }
            else if (sizeAsteroid == 2)
            {
                EnemySpawnner.Instance.ReturnAsteroidToPool2(this.gameObject);
                EnemySpawnner.Instance.SpawnAsteroidRandom(transform.position, 3);
                GameManager.instance.UpdateScore(4);

            }
            else
            {
                sizeAsteroid = 0;
                EnemySpawnner.Instance.ReturnAsteroidToPool3(this.gameObject);
                GameManager.instance.UpdateScore(6);

            }
        }
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.Lifelost();
        }
        
    }



    
}
