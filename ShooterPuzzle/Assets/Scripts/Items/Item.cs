using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Pickup(collision.gameObject.GetComponent<Player>());
        }
    }

    protected virtual void Pickup(Player player)
    {
    }

}
