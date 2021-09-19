using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    //Responsible for controlling UI elements in game

    //Serialized variables assigned in engine editor
    [SerializeField]
    TextMeshProUGUI scoreText, bouncesText, livesText,finalScoreText;
    [SerializeField]
    CanvasGroup pauseGroup;
    [SerializeField]
    CanvasGroup loseGroup,winGroup;

    GameController gc;
    bool isPaused = false;
    private void Awake()
    {
        gc = GameObject.Find("_GameController").GetComponent<GameController>();
    }
    private void Update()
    {
        if (gc.GetIsWin())
        {
            ActivateWinUI();
            return;
        }
        if (gc.GetIsLose())
        {
            ActivateLoseUI();
            return;
        }
        UpdateGamePanelUI();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    //Called every frame to update UI text
    void UpdateGamePanelUI()
    {
        scoreText.text = "Score: " + gc.GetScore();
        finalScoreText.text = "Score: " + gc.GetScore();
        bouncesText.text = "Safe Returns: " + gc.GetBounces();
        livesText.text = "Lives: " + gc.GetLives();
    }

    //Called when player hits escape
    //Toggles pause UI on and off
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            pauseGroup.alpha = 1;
            pauseGroup.interactable = true;
            pauseGroup.blocksRaycasts = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseGroup.alpha = 0;
            pauseGroup.interactable = false;
            pauseGroup.blocksRaycasts = false;
        }
    }

    //Methods for activating lose and win UI respectively
    //Called in update method when either bool is true
    void ActivateLoseUI()
    {
        loseGroup.alpha = 1;
        loseGroup.interactable = true;
        loseGroup.blocksRaycasts = true;
    }
    void ActivateWinUI()
    {
        winGroup.alpha = 1;
        winGroup.interactable = true;
        winGroup.blocksRaycasts = true;
    }
}
