using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    private BoxCollider ourCollider;

    void Start()
    {
        ourCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //call a manager script that will shrink all objects in the scene including the player using some sort of lerp
            SceneShrinkManager.Instance.PerformSceneShrink();
        }
    }
}
