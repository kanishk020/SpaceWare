using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootContinous : MonoBehaviour
{
    public GameObject bullet,bulletPoolParent;

    float time;
    public int poolSize = 20;

    private Queue<GameObject> bulletPool;

    public static ShootContinous Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulet = Instantiate(bullet,bulletPoolParent.transform);
            bulet.SetActive(false); 
            bulletPool.Enqueue(bulet);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(time > 0.25f)
        {
            time = 0;
            ShootBullet();
        }
    }
    void ShootBullet()
    {
        GameObject blt = GetPooledBullet();
        blt.transform.position = transform.position;
        blt.transform.rotation = transform.rotation;
        blt.SetActive(true);
    }
    public GameObject GetPooledBullet()
    {
        GameObject bullet = bulletPool.Dequeue();
        return bullet;
    }
    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
