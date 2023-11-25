using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preload : MonoBehaviour
{
    [SerializeField] private Image startButtonVisual;
    [SerializeField] private GameObject tosHolder;

    [SerializeField] private Sprite normalStartButtonTexture;
    [SerializeField] private Sprite startAndAcceptButtonTexture;

    private void Start()
    {
        CheckToS();
    }

    private void CheckToS()
    {
        if (PlayerPrefs.GetInt("tos", 0) == 1)
        {
            tosHolder.SetActive(false);
            startButtonVisual.sprite = normalStartButtonTexture;
        }
        else
        {
            tosHolder.SetActive(true);
            startButtonVisual.sprite = startAndAcceptButtonTexture;
        }
    }

    public void OnStartButtonPressed()
    {
        if (PlayerPrefs.GetInt("tos") == 0)
        {
            PlayerPrefs.SetInt("tos", 1);
        }

        if (ApplicationManager.Instance.OnInitialized != null)
        {
            ApplicationManager.Instance.OnInitialized += LoadNextScene;
        }
        else
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
    }

    public void OpenToSPage()
    {
        Application.OpenURL("http://www.google.com/");
    }
}