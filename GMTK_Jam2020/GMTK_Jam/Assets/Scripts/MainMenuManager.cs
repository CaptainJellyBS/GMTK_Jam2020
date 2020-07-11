using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public int firstScene;
    public void StartGame()
    {
        SceneManager.LoadScene(firstScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
