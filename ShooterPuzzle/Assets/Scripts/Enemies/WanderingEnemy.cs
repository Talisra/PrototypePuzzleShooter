using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private int direction = 1;
    public float moveSpeed;

    public LayerMask layersToHit;
    private float rayLength = 0.3f; // TODO: later - this should scale with enemy's sprite width/collider

    private float standDelay = 1f;
    private float standCounter = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void HandleMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), rayLength, layersToHit);
        if (hit.collider)
        {
            if (hit.collider.gameObject.CompareTag("BaseGround"))
            {
                direction *= -1;
            }
        }
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x == 0)
        {
            standCounter += Time.deltaTime;
            if (standCounter >= standDelay)
            {
                direction *= -1;
            }
        }
        else
        {
            standCounter = 0;
        }
    }
}
