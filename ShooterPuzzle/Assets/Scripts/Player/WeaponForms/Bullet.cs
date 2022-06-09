using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifetime = 5;
    private float lifetimeCounter = 0;
    private float speed = 500;
    private Vector3 lastVelocity;
    private Vector3 initialFirePos;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // SetDirection will set the direction relative to the AimPoint and not the cursor!
    // that will make sure the player can only shoot in the aim point bounds
    public void SetDirection(Vector3 aimPointPos)
    {
        initialFirePos = transform.position;
        Vector3 direction = (aimPointPos - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("BaseGround"))
        {
            ReturnToPool();
        }
        else if (collision.gameObject.tag.Equals("Steel"))
        {
            Reflect(collision);
        }
        else if (collision.gameObject.tag.Equals("Wood"))
        {
            ReturnToPool();
        }
        else if (collision.gameObject.tag.Equals("Enemy"))
        {
            ReturnToPool();
        }
    }

    private void Update()
    {
        lastVelocity = rb.velocity;
        if (Vector3.Distance(initialFirePos, transform.position) > 50)
        {
            ReturnToPool();
        }
        lifetimeCounter += Time.deltaTime;
        if (lifetimeCounter > lifetime)
        {
            ReturnToPool();
        }
    }

    private void Reflect(Collision2D collision)
    {
        float magnitude = lastVelocity.magnitude;
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.velocity = direction * Mathf.Max(magnitude, 0);
    }

    private void ReturnToPool()
    {
        AudioManager.Instance.Play("handgun_boom");
        PrefabPooler.Instance.Get("BulletSplash", transform.position, Quaternion.identity);// animation only!
        rb.velocity = Vector3.zero; // reset velocity 
        lifetimeCounter = 0; // reset lifetime
        PrefabPooler.Instance.ReturnToPool(gameObject);
    }
}
