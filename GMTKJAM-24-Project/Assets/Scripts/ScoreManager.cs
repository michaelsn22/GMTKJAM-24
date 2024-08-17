using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;
    public StartMovement thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if(thePlayer == null)
        {
            Debug.Log("Player doesn't exit");
        }

        thePlayer = player.GetComponent<StartMovement>();
        score = thePlayer.score;
    }

    // Update is called once per frame
    void Update()
    {
        score = thePlayer.score;
        scoreText.text = "Score: " + score.ToString();
    }
}
