﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTile : MonoBehaviour
{
    // General
    public Sprite sprite_live;
    public Sprite sprite_burn;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D collider2d;

    private List<BombPlatform> platforms = new List<BombPlatform>();

    // Burn & Ticking
    private int ticksToDestroy = 5;
    private int currentTickCounter;

    private float tickTime = .1f;
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
    }

    public void Tick()
    {
        if (isTickable)
        {         
            // animation
            currentTickCounter--;
            isTickable = false;
            if (currentTickCounter <= 0)
            {
                Burn();
            }
        }

    }

    private void Burn()
    {
        collider2d.enabled = false;
        spriteRenderer.sprite = sprite_burn;
        isBurnt = true;
        foreach (BombPlatform platform in platforms)
        {
            platform.Disappear();
        }
    }

    private void Respawn()
    {
        collider2d.enabled = true;
        spriteRenderer.sprite = sprite_live;
        currentTickCounter = ticksToDestroy;
        isBurnt = false;
    }

    public void RegisterBombPlatform(BombPlatform bombPlatform) // tell this tile that a new bombPlatform is attached
    {
        platforms.Add(bombPlatform);
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
                Respawn();
                respawnCounter = 0;
            }
        }
    }


}