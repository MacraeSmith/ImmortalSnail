using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUI : MonoBehaviour
{

    public void Update()
    {

    }
    public string newGameScene;
    public GameObject controlsWindow;

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void OpenControlsWindow()
    {
        controlsWindow.SetActive(true);
    }

    public void CloseControlsWindow()
    {
        controlsWindow.SetActive(false);
    }

}
