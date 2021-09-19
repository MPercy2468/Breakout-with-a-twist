using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    //Used to handle scene changes and close the game

    //Called to load scene at specified build index
    public void LoadScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    //Called to close application
    public void Quit()
    {
        Application.Quit();
    }
}
