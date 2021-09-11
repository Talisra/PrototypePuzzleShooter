using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSplash : MonoBehaviour
{
    private float scale;
    private float maxSize = 1;
    private float scaleSpeed = .02f;
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.localScale = new Vector3(.1f, .1f, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
        if (transform.localScale.x > maxSize)
        {
            PrefabPooler.Instance.ReturnToPool(gameObject);
        }
    }
}
