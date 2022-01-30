using UnityEngine;

public class Block : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        col.size = sr.size;
    }
}
