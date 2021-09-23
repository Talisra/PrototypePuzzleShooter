using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlatform : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private float disappearDelay = 5f;
    private float timeCounter = 0;

    private WoodTile anchorTile;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.flipX = false;
    }

    public void AttachToWall(ContactPoint2D cp, WoodTile tile) // tile anchor is the bottom left of the tile
    {
        Vector3 tileCenter = tile.transform.position;
        float leftRight = Mathf.Abs(cp.point.x - tileCenter.x);
        float topBot = Mathf.Abs(cp.point.y - tileCenter.y);
        float x;
        float xAdd = 0;
        float y;
        float yAdd = 0;
        if (leftRight >= topBot)
        {
            int dir = (int)Mathf.Sign(cp.point.x - tileCenter.x);
            x = tileCenter.x + (dir * 0.25f);
            y = tileCenter.y;
            xAdd = boxCollider.bounds.size.x/2 * dir;
            if (dir > 0)
                spriteRenderer.flipX = true;
        }
        else
        {
            int dir = (int)Mathf.Sign(cp.point.y - tileCenter.y);
            x = tileCenter.x;
            y = tileCenter.y + (dir * 0.25f);
            yAdd = boxCollider.bounds.size.y / 2 * dir;
            if (dir > 0)
                transform.localRotation = Quaternion.Euler(0,0, -90);
            else
                transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        Vector3 newPosition = new Vector3(x + xAdd, y + yAdd, 0);
        transform.position = newPosition;
        anchorTile = tile;
    }

    public void Disappear()
    {
        PrefabPooler.Instance.ReturnToPool(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter > disappearDelay)
        {
            timeCounter = 0;
            
            Disappear();
        }
    }
}
