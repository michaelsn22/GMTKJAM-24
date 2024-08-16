using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneShrinkManager : MonoBehaviour
{
    public static SceneShrinkManager Instance { get; private set; }

    private float scaleFactor = 0.9f;

    [SerializeField] private GameObject masterObject; // the main object everything in the scene will be a child of.

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) //singleton pattern
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void PerformSceneShrink()
    {
        Debug.Log("shrink everything!");
        //loop through all children of a master object, and change their scale values by a certain factor.'
        foreach (Transform child in masterObject.transform)
        {
            Vector3 currentScale = child.localScale;

            Vector3 newScale = currentScale * scaleFactor;

            child.localScale = newScale;
        }
    }
}
