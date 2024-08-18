using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatHandler : MonoBehaviour
{
    private float TimeSinceSpacebarPressed = 0f;
    private float TimeSinceBeatOccured = 0f;
    private float gameTimer = 0f;
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioClip defaultBeatSound;
    void Start()
    {
        InvokeRepeating("BeatChecker", 0.5f, 1f);
    }

    void Update()
    {
        gameTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && TimeSinceSpacebarPressed - TimeSinceBeatOccured <= 0.2f)
        {
            Debug.Log("we hit the space bar on time!");
            TimeSinceSpacebarPressed = Time.time;
        }
    }

    private void BeatChecker()
    {
        TimeSinceBeatOccured = Time.time;
        mainAudioSource.clip = defaultBeatSound;
        mainAudioSource.PlayOneShot(defaultBeatSound);
        Debug.Log("beat call!");
        /*
        if (gameTimer >= 0.5f && gameTimer - TimeSinceSpacebarPressed <= 0.5f)
        {
            Debug.Log("we hit the beat!");
        }
        */
    }
}
