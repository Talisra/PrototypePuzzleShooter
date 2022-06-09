using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    private Rigidbody2D rb;
    public bool hasParent = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!hasParent)
        {
            Release();
        }     
    }

    public void Release()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    
    protected override void Pickup(Player player)
    {
        AudioManager.Instance.Play("key");
        LevelManager.Instance.AddKey();
    }
}
