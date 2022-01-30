using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player player;
    public WeaponForm startingForm;
    private Transform startingPoint;
    private ExitDoor exit;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        player = FindObjectOfType<Player>();
        ValidateMap();
    }

    public void AddKey()
    {
        exit.AddKey();
    }

    public void FinishLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    private void ValidateMap()
    {
        // validate starting form
        if (!startingForm)
        {
            Debug.LogError("A starting form hasn't apllied in the Level Manager!");
        }
        // validate start 
        StartingPoint[] tempFindStart = FindObjectsOfType<StartingPoint>();
        if (tempFindStart.Length == 0)
        {
            Debug.LogError("The level has no starting point for the player!");
        }
        else if (tempFindStart.Length > 1)
        {
            Debug.LogError("Only one starting point can be at a level!");
        }
        else
            startingPoint = tempFindStart[0].transform;
        // validate exit
        ExitDoor[] tempFindExit = FindObjectsOfType<ExitDoor>();
        if (tempFindExit.Length == 0)
        {
            Debug.LogError("The level has no exit!");
        }
        else if (tempFindExit.Length > 1)
        {
            Debug.LogError("Only one exit can be at a level!");
        }
        else
            exit = tempFindExit[0];
        // set exit's key number
        exit.SetNumberOfKeys(FindObjectsOfType<Key>().Length);
        Debug.Log("Total Keys: " + (FindObjectsOfType<Key>().Length));
    }

    void Start()
    {
        player.ChangeForm(startingForm);
        player.transform.position = startingPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
