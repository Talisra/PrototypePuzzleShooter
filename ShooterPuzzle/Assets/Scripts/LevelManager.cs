using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    public Player player;
    public WeaponForm startingForm;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        if (!startingForm)
        {
            Debug.LogError("A starting form hasn't apllied in the Level Manager!");
        }
    }

    void Start()
    {
        player.ChangeForm(startingForm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
