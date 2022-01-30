using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public Image loadingBar;
    public GameObject menu;
    public GameObject levelSelector;
    public GameObject loading;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        loadingBar.fillAmount = 0;
    }

    public void ShowLevels()
    {
        levelSelector.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
    }

    public IEnumerator LoadAsynchronousely(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        levelSelector.gameObject.SetActive(false);
        loading.gameObject.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.fillAmount = progress;
            yield return null;
        }
    }
}
