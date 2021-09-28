﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelTile : MonoBehaviour
{
    // General
    private Rigidbody2D rb;
    private Collider2D collider2d;

    // Ticking
    private int ticksToMove = 5;
    private int currentTickCounter;

    private float tickTime = .1f;
    private float tickTimeCounter = 0;
    private bool isTickable = false;

    // Moving
    private int state; // 0: static, 1: moving, 2: stop, 3: back
    private float moveSpeed = 1.3f;
    private float moveBackTime = 5;
    private float moveBackCounter = 0;

    private Vector3 moveDirection = Vector2.zero;
    private Vector3 startPoint;
    private Vector3 tempPoint;
    private Vector3 endPoint;

    // Raycast
    private float rayLength = 1;
    public LayerMask layersToHit;

    private void Awake()
    {
        startPoint = transform.position;
        currentTickCounter = ticksToMove;
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    private void StartMovingTile(Vector2 hitOrigin)
    {
        float horizontal = transform.position.x - hitOrigin.x;
        float vertical = transform.position.y - hitOrigin.y;
        Vector3 direction = new Vector3(
            Mathf.Abs(horizontal) > Mathf.Abs(vertical) ? Mathf.Sign(horizontal) : 0,
            Mathf.Abs(vertical) > Mathf.Abs(horizontal) ? Mathf.Sign(vertical) : 0,
            0
            );
        state = 1;
        moveDirection = direction;
    }

    private void CheckBeforeMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, layersToHit);
        if (!hit.collider)
        {
            endPoint = transform.position + (Vector3)direction;
        } else
            endPoint = hit.point - direction * collider2d.bounds.size.x / 2;
    }

    private void CheckBeforeMoveBack(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, layersToHit);
        if (!hit.collider)
        {
            tempPoint = transform.position + (Vector3)direction;
        }
        else
            tempPoint = hit.point - direction * collider2d.bounds.size.x / 2;
    }

    private void Stop()
    {
        transform.position = endPoint;
        state = 2;
    }

    private void Reset()
    {
        state = 0;
        isTickable = true;
        currentTickCounter = ticksToMove;
        tickTimeCounter = 0;
    }

    public void Tick(Vector2 hitOrigin)
    {
        if (isTickable)
        {
            // animation
            currentTickCounter--;
            isTickable = false;
            if (currentTickCounter <= 0)
            {
                StartMovingTile(hitOrigin);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0: // static
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
                    break;
                }
            case 1: // move
                {
                    CheckBeforeMove(moveDirection);
                    transform.position = Vector3.MoveTowards(transform.position, endPoint, Time.deltaTime * moveSpeed);
                    if (Vector2.Distance(transform.position, endPoint) < 0.1f)
                    {
                        Stop();
                    }
                    break;
                }
            case 2: // stop
                {
                    moveBackCounter += Time.deltaTime;
                    if (moveBackCounter >= moveBackTime)
                    {
                        moveBackCounter = 0;
                        state = 3;
                        moveDirection *= -1;
                    }
                    break;
                }
            case 3:
                {
                    CheckBeforeMoveBack(moveDirection);
                    transform.position = Vector3.MoveTowards(transform.position, tempPoint, Time.deltaTime * moveSpeed);
                    if (Vector2.Distance(transform.position, startPoint) < 0.1f)
                    {
                        Reset();
                    }
                    break;
                }
            default:
                break;
        }
    }
}
