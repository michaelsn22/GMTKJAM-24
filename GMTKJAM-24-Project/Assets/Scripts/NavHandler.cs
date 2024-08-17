using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavHandler : MonoBehaviour
{
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
}
