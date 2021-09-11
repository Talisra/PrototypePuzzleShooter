using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlatform : MonoBehaviour
{
    private float disappearDelay = 5f;
    private float timeCounter = 0;

    public void AttachToWall()
    {

    }

    private void Disappear()
    {
        PrefabPooler.Instance.ReturnToPool(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter > disappearDelay)
        {
            timeCounter = 0;
            Disappear();
        }
    }
}
