using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavHandler : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
    }
    public void GoToLevelOne()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PullUpSettings()
    {
        Debug.Log("pulling up settings");
    }

    public void QuitGame()
    {
        Debug.Log("closing game");
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void RetrySameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
