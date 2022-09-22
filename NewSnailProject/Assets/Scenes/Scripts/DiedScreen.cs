using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiedScreen : MonoBehaviour
{
    public string startScreen;

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void GoToStartScreen()
    {
        SceneManager.LoadScene(startScreen);
    }

}
