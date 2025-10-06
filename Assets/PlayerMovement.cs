using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float speedRotate;

    private Vector3 screenBounds;

    void Start()
    {
        // Calculate the world space boundaries of the screen
        screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.transform.position.z));
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 nextPosition = this.transform.position + this.transform.up * vertical * Time.deltaTime * speed;

        float clampedX = screenBounds.x;
        float clampedY = screenBounds.y;

        if (Mathf.Abs(nextPosition.y) < clampedY && Mathf.Abs(nextPosition.x) <clampedX)
        {
            this.transform.position = nextPosition;
        }

        this.transform.Rotate(Vector3.back * horizontal * Time.deltaTime * speedRotate);
    }
}
