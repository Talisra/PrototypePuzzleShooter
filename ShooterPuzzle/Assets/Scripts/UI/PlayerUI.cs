using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public Sprite lifeImage;
    public List<Image> lives;
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        for (int i = 0; i < lives.Count; i++)
        {
            lives[i].sprite = lifeImage;
        }
    }

    private void Update()
    {
        for (int i = 1; i <= player.maxLives; i++)
        {
            if (i > player.GetCurrentLives())
            {
                Color temp = lives[i-1].color;
                lives[i-1].color = new Color(temp.r, temp.g, temp.b, 0.2f);
            }
        }
    }
}
