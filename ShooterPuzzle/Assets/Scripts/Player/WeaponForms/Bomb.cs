using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Rigidbody2D rb;
    public float stickDelay;
    private Vector3 initialFirePos;
    private float gravityScale;

    private Vector3 direction;

    private bool isStick = false;
    private float stickCounter = 0;

    private void Awake()
    {
        gravityScale = rb.gravityScale;
    }

    private void OnEnable()
    {
        isStick = false;
        stickCounter = 0;
        rb.gravityScale = gravityScale;
    }

    // SetDirectionAndPower will set the direction relative to the AimPoint and not the cursor!
    // that will make sure the player can only shoot in the aim point bounds
    public void SetDirectionAndPower(Vector3 aimPointPos, float power)
    {
        initialFirePos = transform.position;
        direction = (aimPointPos - transform.position).normalized;
        rb.AddForce(direction * power, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("BaseGround"))
        {
            ReturnToPool();
        }
        else if (collision.gameObject.tag.Equals("Steel"))
        {
            ReturnToPool();
        }
        else if (collision.gameObject.tag.Equals("Wood"))
        {
            Stick(collision.transform.position);
        }
    }

    private void Stick(Vector2 pos)
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        isStick = true;
    }

    private void Update()
    {
        if (isStick)
        {
            stickCounter += Time.deltaTime;
            if (stickCounter >= stickDelay)
            {
                ReturnToPool();
            }
        }
        if (Vector3.Distance(initialFirePos, transform.position) > 50)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        rb.velocity = Vector3.zero; // reset velocity 
        PrefabPooler.Instance.ReturnToPool(gameObject);
    }
}
