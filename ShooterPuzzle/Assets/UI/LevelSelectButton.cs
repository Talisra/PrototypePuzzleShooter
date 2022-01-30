using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public Text levelText;

    public int sceneIndex;
    public int levelIndex;


    private void Awake()
    {
        levelText.text = "Level " +  levelIndex.ToString("00");
    }
    public void GoToLevel()
    {
        MenuManager.Instance.StartCoroutine(MenuManager.Instance.LoadAsynchronousely(sceneIndex));
    }
}
