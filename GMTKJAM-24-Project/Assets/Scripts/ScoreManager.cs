using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }
    public TextMeshProUGUI scoreText;
    public int score;
    public StartMovement thePlayer;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if(thePlayer == null)
        {
            Debug.Log("Player doesn't exit");
        }

        GameObject player = GameObject.Find("Player");
        thePlayer = player.GetComponent<StartMovement>();
        score = thePlayer.score;
    }

    // Update is called once per frame
    void Update()
    {
        score = thePlayer.score;
        scoreText.text = "Score: " + score.ToString();
    }

    public int GetPlayerScore()
    {
        return score;
    }
}
