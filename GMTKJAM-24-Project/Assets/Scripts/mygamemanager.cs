using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class mygamemanager : MonoBehaviour
{
    public static mygamemanager instance { get; private set; }
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject EndScoreboard;
    [SerializeField] private TextMeshProUGUI scoreBoardTitle;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject retryButton;
    private bool isGameOver = false;
    private float timeKeeper;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeRemainingText;

    private float TimeToBeat = 120f;
    private float remainingTime = 120f;

    private int TotalObjectsToCollect = 0; //total obj to turn to slime for a given level
    private int currentCollectedCount = 0;

    //GUI stuff
    [SerializeField] private TextMeshProUGUI TimeCountdownUI;
    public bool fellOffMap = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Make the cursor invisible
        Cursor.visible = false;
        MapSelector();

    }

    void Update()
    {
        timeKeeper += Time.deltaTime;

        remainingTime = TimeToBeat - timeKeeper;

        //TimeCountdownUI.text = "Time Remaining: "+remainingTime;
        TimeCountdownUI.text = string.Format("Remaining Time: {0:0.00}", remainingTime);

        if (remainingTime <= 0f || fellOffMap)
        {
            Debug.Log("you lose!");
            nextLevelButton.SetActive(false);
            retryButton.SetActive(true);
            remainingTime = 0f;
            scoreBoardTitle.text = "You Lose!";
            EndScoreboard.SetActive(true);
            //Unlock the cursor
            Cursor.lockState = CursorLockMode.None;

            //Make the cursor visible again
            Cursor.visible = true;
            
            scoreText.text = "Score: "+ScoreManager.instance.GetPlayerScore();
            timeRemainingText.text = string.Format("Time Left: {0:0.00}s", remainingTime);
            Time.timeScale = 0f;
        }

        if (isGameOver)
        {
            //Unlock the cursor
            Cursor.lockState = CursorLockMode.None;

            //Make the cursor visible again
            Cursor.visible = true;

            //freeze time
            Time.timeScale = 0f;

            if (!EndScoreboard.activeInHierarchy)
            {
                nextLevelButton.SetActive(true);
                retryButton.SetActive(false);

                EndScoreboard.SetActive(true);
                scoreBoardTitle.text = "You Won!";

                //update the text on the scoreboard to the correct values.
                scoreText.text = "Score: "+ScoreManager.instance.GetPlayerScore();
                timeRemainingText.text = string.Format("Time Left: {0:0.00}s", remainingTime);
            }
        }

    }

    public void EndGame()
    {
        isGameOver = true;
    }

    public void IncrementCollectedObjectCount()
    {
        currentCollectedCount++;
        //Debug.Log(currentCollectedCount);

        if (currentCollectedCount >= TotalObjectsToCollect)
        {
            EndGame();
        }
    }

    private void MapSelector()
    {

        switch(SceneManager.GetActiveScene().name)
        {
            case "SampleScene":
                TimeToBeat = 30f;
                TotalObjectsToCollect = 6;
                break;

            case "Map3":
                TimeToBeat = 15f;
                TotalObjectsToCollect = 6;
                break;
            
            case "Map4":
                TimeToBeat = 22f;
                TotalObjectsToCollect = 9;
                break;

            case "Map5":
                TimeToBeat = 16f;
                TotalObjectsToCollect = 11;
                break;

            case "Map6":
                TimeToBeat = 19f;
                TotalObjectsToCollect = 13;
                break;
        }
    }
}
