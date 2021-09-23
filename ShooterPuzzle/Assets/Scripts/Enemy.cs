using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    private int currentHP;

    private float tickDelay = .1f;
    private float tickCounter = 0;
    private bool isTicking = false;
    private void OnEnable()
    {
        currentHP = maxHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("PlayerAttack"))
        {
            DamagingObject damagingObject;
            if (collision.gameObject.TryGetComponent<DamagingObject>(out damagingObject))
            {
                if (damagingObject.damage > 0)
                    TakeDamage(damagingObject.damage);
            }
            else
            {
                Debug.LogError("Player attack " +collision.gameObject.name + " doesn't have DamagingObject component!");
            }
        }
    }

    public void TakeDamage(int amount)
    {
        Hit();
        currentHP -= amount;
        Debug.Log("hp left: " +currentHP);
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    public void TakeTickDamage() // for multiple damage like laser
    {
        if (!isTicking)
        {
            Hit();
            currentHP--;
            Debug.Log("hp left: " + currentHP);
            if (currentHP <= 0)
            {
                currentHP = 0;
                Die();
            }
            else
            {
                isTicking = true;
            }
        }
    }

    private void Hit()
    {
        PrefabPooler.Instance.Get("BombSplash", transform.position, Quaternion.identity);
    }

    private void Die()
    {
        PrefabPooler.Instance.ReturnToPool(gameObject);
    }

    private void Update()
    {
        if (isTicking)
        {
            tickCounter += Time.deltaTime;
            if (tickCounter >= tickDelay)
            {
                isTicking = false;
                tickCounter = 0;
            }
        }
    }
}
