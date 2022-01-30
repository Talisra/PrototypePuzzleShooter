using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isPoolable;
    public event EventHandler OnEnemyDeath;

    [HideInInspector]
    public SpriteRenderer sr;
    public int maxHP;
    private int currentHP;

    private float tickDelay = .1f;
    private float tickCounter = 0;
    private bool isTicking = false;

    EnemyHPBar hpBar;

    public int GetCurrentHp()
    {
        return currentHP;
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        GameObject hpgo = Instantiate(Resources.Load("Prefabs/HPbar", typeof(GameObject)),
            transform.position, Quaternion.identity) as GameObject;
        hpBar = hpgo.GetComponent<EnemyHPBar>();
        hpBar.AttatchToEnemy(this);
    }

    private void OnEnable()
    {
        currentHP = maxHP;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
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
        OnEnemyDeath?.Invoke(this, new EventArgs()); // invoke enemy death event
        if (isPoolable)
        {
            PrefabPooler.Instance.ReturnToPool(gameObject);
        }else
        {
            gameObject.SetActive(false);
        }
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
