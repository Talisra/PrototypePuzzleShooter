using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Rigidbody2D rb;
    public float stickDelay;
    private Vector3 initialFirePos;

    private Vector3 direction;



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
            Explode();
        }
        else if (collision.gameObject.tag.Equals("Steel"))
        {
            Explode();
        }
        else if (collision.gameObject.tag.Equals("Wood"))
        {
            Stick(collision.transform.position);
        }
    }

    private void Explode()
    {
        PrefabPooler.Instance.Get("BombSplash", transform.position, Quaternion.identity);
        ReturnToPool();
    }

    private void Stick(Vector2 pos)
    {
        PrefabPooler.Instance.Get("BombPlatform", transform.position, Quaternion.identity);
        ReturnToPool();
    }

    private void Update()
    {
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
