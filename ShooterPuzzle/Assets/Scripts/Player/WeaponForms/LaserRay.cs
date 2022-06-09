using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRay : MonoBehaviour
{
    public LayerMask layersToHit;
    public ParticleSystem hitParticle;
    private LineRenderer lr;
    private float rayLength = 20;

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
        if (hit)
        {
            hitParticle.gameObject.SetActive(true);
            hitParticle.transform.position = hit.point;
        }
        else
        {
            hitParticle.gameObject.SetActive(false);
        }
        if (!hit.collider)
        {
            lr.SetPosition(1, laserStart + laserDirection * rayLength);
        }
        else if (hit.collider.gameObject.CompareTag("Enemy"))
        {
                    hitParticle.transform.position = hit.point;
            lr.SetPosition(1, hit.point);
            hit.collider.gameObject.GetComponent<Enemy>().TakeTickDamage();
        }
        else if (hit.collider.gameObject.CompareTag("Wood"))
        {
            lr.SetPosition(1, hit.point);
            hit.collider.gameObject.GetComponent<WoodTile>().Tick();
        }
        else if (hit.collider.gameObject.CompareTag("Steel"))
        {
            lr.SetPosition(1, hit.point);
            hit.collider.gameObject.GetComponent<SteelTile>().Tick(hit.point);
        }
        else
        {
            lr.SetPosition(1, hit.point);
        }
    }
}
