using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTile : MonoBehaviour
{
    // General
    public Sprite sprite_live;
    public Sprite sprite_burn;
    public Player player;

    public Sprite[] crackSprites;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D collider2d;

    private List<BombPlatform> platforms = new List<BombPlatform>();

    // Burn & Ticking
    private int ticksToDestroy = 5;
    private int currentTickCounter;

    private float tickTime = .2f;
    private float tickTimeCounter = 0;
    private bool isTickable = false;

    private bool isBurnt = false;
    private float respawnTime = 5;
    private float respawnCounter = 0;


    private void Awake()
    {
        currentTickCounter = ticksToDestroy;
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
    }



    private void Burn()
    {
        AudioManager.Instance.Play("destroy");
        ParticleSystem ps = PrefabPooler.Instance.Get("Wood-Destroy", transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ps.Stop();
        ps.Play();
        collider2d.enabled = false;
        spriteRenderer.sprite = sprite_burn;
        isBurnt = true;
        foreach (BombPlatform platform in platforms)
        {
            platform.Disappear();
        }
        platforms.Clear();
    }

    private void CheckRespawn()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 0.75f) // only respawn if player is not in the same position
        {
            respawnCounter = respawnTime - Time.deltaTime;
            return;
        }
        collider2d.enabled = true;
        spriteRenderer.sprite = sprite_live;
        currentTickCounter = ticksToDestroy;
        isBurnt = false;
        respawnCounter = 0;
    }

    public void RegisterBombPlatform(BombPlatform bombPlatform) // tell this tile that a new bombPlatform is attached
    {
        platforms.Add(bombPlatform);
    }
    public void UnregisterBombPlatform(BombPlatform bombPlatform) // tell this tile that a platform disappeared
    {
        platforms.Remove(bombPlatform);
    }
    public void Tick()
    {
        if (isTickable)
        {
            // animation
            AudioManager.Instance.Play("tick");
            currentTickCounter--;
            if (currentTickCounter > 0)
                spriteRenderer.sprite = crackSprites[crackSprites.Length - currentTickCounter];
            isTickable = false;
            if (currentTickCounter <= 0)
            {
                Burn();
            }
        }

    }

    private void Update()
    {
        if (!isTickable)
        {
            tickTimeCounter += Time.deltaTime;
            if (tickTimeCounter >= tickTime)
            {
                isTickable = true;
                tickTimeCounter = 0;
            }
        }
        if (isBurnt)
        {
            respawnCounter += Time.deltaTime;
            if (respawnCounter >= respawnTime)
            {
                CheckRespawn();
            }
        }
    }


}
