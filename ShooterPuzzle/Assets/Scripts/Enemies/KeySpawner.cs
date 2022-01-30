using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    private Enemy enemy;
    public Key key;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemy.OnEnemyDeath += DropKey;
    }

    private void DropKey(object sender, System.EventArgs e)
    {
        key.Release();
        key.transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
