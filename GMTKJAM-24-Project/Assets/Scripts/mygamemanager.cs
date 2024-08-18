using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mygamemanager : MonoBehaviour
{
    public static mygamemanager instance { get; private set; }
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject EndScoreboard;
    private bool isGameOver = false;
    private float timeKeeper;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeRemainingText;

    private float TimeToBeat = 120f;
    private float remainingTime = 120f;

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
    }

    void Update()
    {
        timeKeeper += Time.deltaTime;

        remainingTime = TimeToBeat - timeKeeper;

        if (remainingTime <= 0f)
        {
            Debug.Log("you lose!");
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
                EndScoreboard.SetActive(true);

                //update the text on the scoreboard to the correct values.
                scoreText.text = "Score: "+ScoreManager.instance.GetPlayerScore();
                timeRemainingText.text = "Time Left: "+remainingTime;
            }
        }

    }

    public void EndGame()
    {
        isGameOver = true;
    }
}
