using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRay : MonoBehaviour
{
    public LayerMask layersToHit;
    private LineRenderer lr;
    private float rayLength = 10;

    private Vector2 laserStart;
    private Vector2 laserDirection;


    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    public void SetPositionAndDirection(Vector2 start, Vector2 direction)
    {
        laserStart = start;
        laserDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, laserStart);
        RaycastHit2D hit = Physics2D.Raycast(laserStart, laserDirection, rayLength, layersToHit);
        if (!hit.collider)
        {
            lr.SetPosition(1, laserStart + laserDirection * rayLength);
        }
        else
        {
            lr.SetPosition(1, hit.point);
        }
    }
}
