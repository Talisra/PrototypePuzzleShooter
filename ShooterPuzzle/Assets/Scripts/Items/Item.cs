using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isPickable = true;
    private bool isPicked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (isPickable)
            {
                if (!isPicked)
                    Pickup(collision.gameObject.GetComponent<Player>());
                isPicked = true;
                Dispose();
            }
            else
                Pickup(collision.gameObject.GetComponent<Player>());

        }
    }

    protected virtual void Pickup(Player player)
    {
    }

    private void Dispose()
    {
        gameObject.SetActive(false);
    }

}
