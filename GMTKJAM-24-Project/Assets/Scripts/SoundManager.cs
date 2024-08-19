using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioSource[] audioSources;
    public float crossfadeDuration = 1f;

    private int currentTrack = 0;
    private float currentActionCounter = 0f;

    //dynamic decay system
    public float decayRate = 0.5f; // How much the counter decreases per second
    public float decayDelay = 2f; // How long to wait before starting decay
    public float actionThreshold = 0.5f; // Minimum time between actions to reset decay timer

    private float lastActionTime = 0f;
    private float lastDecayTime = 0f;

    //attempt to solve double blending coroutine bug.
    private bool isBlending = false;
    private Coroutine blendCoroutine;

    void Start()
    {
        // Start playing the first track
        audioSources[0].Play();
    }

     private void Update()
    {
        float currentTime = Time.time;

        // Check if it's time to decay the counter
        if (currentTime - lastActionTime > decayDelay)
        {
            DecayCounter();
        }

        // Update the music based on the current action counter
        UpdateMusic();
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BlendToTrack(int newTrackIndex)
    {
        // If we're already blending or the new track is the same as the current, do nothing
        if (isBlending || newTrackIndex == currentTrack) return;

        float currentTime = audioSources[currentTrack].time;
        
        // Set the new track to the same time as the current track
        audioSources[newTrackIndex].time = currentTime;
        audioSources[newTrackIndex].Play();

        // Stop any existing blend coroutine
        if (blendCoroutine != null)
        {
            StopCoroutine(blendCoroutine);
        }

        // Start a new blend coroutine
        blendCoroutine = StartCoroutine(CrossfadeTracks(currentTrack, newTrackIndex));
        
        currentTrack = newTrackIndex;
    }

    private IEnumerator CrossfadeTracks(int oldTrackIndex, int newTrackIndex)
    {
        isBlending = true;
        float timeElapsed = 0f;

        while (timeElapsed < crossfadeDuration)
        {
            float t = timeElapsed / crossfadeDuration;
            audioSources[oldTrackIndex].volume = 1 - t;
            audioSources[newTrackIndex].volume = t;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSources[oldTrackIndex].Stop();
        audioSources[oldTrackIndex].volume = 0;
        audioSources[newTrackIndex].volume = 1;

        isBlending = false;
        blendCoroutine = null;
    }

    public void IncrementCounter(float incrementAmount = 1f)
    {
        float currentTime = Time.time;

        // Reset decay timer if actions are frequent enough
        if (currentTime - lastActionTime < actionThreshold)
        {
            lastActionTime = currentTime;
        }

        if (currentActionCounter <= 21)
        {
            currentActionCounter += incrementAmount;
        }
        
        lastActionTime = currentTime;

        // Clamp the counter to a maximum value if needed
        // currentActionCounter = Mathf.Min(currentActionCounter, 30f);

        Debug.Log($"Action occurred. Counter: {currentActionCounter}");
        UpdateMusic();
    }

    private void DecayCounter()
    {
        float oldCounter = currentActionCounter;
        currentActionCounter = Mathf.Max(0f, currentActionCounter - decayRate * Time.deltaTime);
        
        if (oldCounter != currentActionCounter)
        {
            Debug.Log($"Decay occurred. Counter: {currentActionCounter}");
        }
    }

    private void UpdateMusic()
    {
        if (currentActionCounter >= 0 && currentActionCounter < 2)
        {
            BlendToTrack(0); // Assuming track 0 is your base track
        }
        else if (currentActionCounter >= 2 && currentActionCounter < 8)
        {
            BlendToTrack(1);
        }
        else if (currentActionCounter >= 8 && currentActionCounter < 14)
        {
            BlendToTrack(2);
        }
        else if (currentActionCounter >= 14 && currentActionCounter < 20)
        {
            BlendToTrack(3);
        }
        else if (currentActionCounter >= 20)
        {
            BlendToTrack(4);
        }
    }
/*
    public void IncrementCounter()
    {
        currentActionCounter++;

        if (currentActionCounter >= 3 && currentActionCounter <= 5)
        {
            Debug.Log("blending to 1");
            BlendToTrack(1);
        }
        else if(currentActionCounter > 5 && currentActionCounter <= 10)
        {
            Debug.Log("blending to 2");
            BlendToTrack(2);
        }
        else if(currentActionCounter > 10 && currentActionCounter <= 14)
        {
            Debug.Log("blending to 3");
            BlendToTrack(3);
        }
        else if(currentActionCounter > 14 && currentActionCounter <= 30)
        {
            Debug.Log("blending to 4");
            BlendToTrack(4);
        }
    }
*/
}
