using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private Collider ourCollider;

    void Start()
    {
        ourCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Testy");
        if (other.CompareTag("Player"))
        {
            mygamemanager.instance.fellOffMap = true;
        }
    }
}
