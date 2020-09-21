using UnityEngine;
using System.Collections;

public class WaypointBehaviour : MonoBehaviour
{
  
    void Awake()
    {
        GetComponent<Renderer>().enabled = false;

        if (gameObject.GetComponent<Collider>() != null)
        {
            Destroy(gameObject.GetComponent<Collider>());
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}   

