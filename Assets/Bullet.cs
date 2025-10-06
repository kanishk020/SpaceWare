using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private float topBound;
    private float bottomBound;
    private float rightBound;
    private float leftBound;

    void Start()
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
    }

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        Vector3 pos = transform.position;
        if (pos.y > topBound || pos.y < bottomBound || pos.x > rightBound || pos.x < leftBound)
        {
            ShootContinous.Instance.ReturnBulletToPool(this.gameObject);
        }
    }
}