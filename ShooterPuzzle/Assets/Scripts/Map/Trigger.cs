using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Sprite sprite_off;
    public Sprite sprite_on;

    private SpriteRenderer sr;
    private LineRenderer lr;

    private bool isOn = false;
    public TriggerObject trigObj;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, trigObj.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        if (!isOn)
        {
            lr.enabled = false;
            isOn = true;
            sr.sprite = sprite_on;
            trigObj.TurnOn();
        }
    }

    private void Update()
    {

    }
}
