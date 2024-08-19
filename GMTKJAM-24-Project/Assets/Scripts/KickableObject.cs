using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickableObject : MonoBehaviour
{
    [SerializeField] private Material SlimeMaterial;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Player") || collision.gameObject.name.Contains("Plane"))
        {
            Debug.Log("Ignore target");
            return;
        }
        else
        {
            GameObject.Find("Player").GetComponent<StartMovement>().score += 10;
            mygamemanager.instance.IncrementCollectedObjectCount();
        }

        Renderer collidedRenderer = collision.gameObject.GetComponent<Renderer>();
        if (collidedRenderer != null)
        {
            collidedRenderer.material = SlimeMaterial;
        }

        GetComponent<Collider>().enabled = false; //have to add this line or the object kicked can gain multiple points lol
    }
}
