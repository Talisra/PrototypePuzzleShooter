using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Bomb : MonoBehaviour
{
    public Rigidbody2D rb;
    public float stickDelay;
    private Vector3 initialFirePos;

    private Vector3 direction;
    private Vector2 velocityBeforeCollision;

    private float splashDmgRadius = 1;
    private int splashDamage = 5; // not a damaging object

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
        Vector2 cp = collision.GetContact(0).point;
        if (collision.gameObject.tag.Equals("BaseGround"))
        {
            Explode(cp);
        }
        else if (collision.gameObject.tag.Equals("Steel"))
        {
            Explode(cp);
        }
        else if (collision.gameObject.tag.Equals("Wood"))
        {
            Stick(collision);
        }
        else if (collision.gameObject.tag.Equals("PlayerPlatform"))
        {
            Explode(cp);
        }
        else if (collision.gameObject.tag.Equals("Enemy"))
        {
            Explode(cp);
        }
    }

    private void Explode(Vector3 position)
    {
        PrefabPooler.Instance.Get("BombSplash", transform.position, Quaternion.identity);// animation only!
        ExplosionDamage(position);
        ReturnToPool();
    }

    void ExplosionDamage(Vector2 center)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, splashDmgRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag.Equals("Enemy"))
            {
                hitCollider.gameObject.GetComponent<Enemy>().TakeDamage(splashDamage);
            }
        }
    }
    /*
    private void Stick(Collision2D wallCollision)
    {
        BombPlatform platform =  
            PrefabPooler.Instance.Get("BombPlatform", transform.position, Quaternion.identity)
            .GetComponent<BombPlatform>();
        Tilemap tilemap = wallCollision.collider.GetComponent<Tilemap>();
        ContactPoint2D cp = wallCollision.GetContact(0);
        Vector3Int tileGridVector =
            tilemap.WorldToCell(cp.point - (cp.normal * 0.15f));
        platform.AttachToWall(wallCollision.GetContact(0), tilemap.CellToWorld(tileGridVector) + new Vector3(0.25f,0.25f,0));
        ReturnToPool();
    }*/

    private void Stick(Collision2D wallCollision)
    {
        BombPlatform platform =
            PrefabPooler.Instance.Get("BombPlatform", transform.position, Quaternion.identity)
            .GetComponent<BombPlatform>();
        platform.AttachToWall(
            wallCollision.GetContact(0),
            wallCollision.gameObject.GetComponent<WoodTile>()
            );
        ReturnToPool();
    }

    private void Update()
    {
        velocityBeforeCollision = rb.velocity.Equals(Vector3.zero) ? velocityBeforeCollision : rb.velocity ;
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
