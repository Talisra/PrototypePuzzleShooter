using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public Sprite open;
    private SpriteRenderer sr;
    private int totalKeysToOpen;
    private int collectedKeys = 0;

    private bool isOpen = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void AddKey()
    {
        collectedKeys++;
        if (collectedKeys == totalKeysToOpen)
        {
            sr.sprite = open;
            isOpen = true;
        }
    }

    public void SetNumberOfKeys(int amount)
    {
        totalKeysToOpen = amount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen)
        {
            LevelManager.Instance.FinishLevel();
        }
    }
}
