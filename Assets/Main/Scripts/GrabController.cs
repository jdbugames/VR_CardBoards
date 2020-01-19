using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    public bool bl_CanGrab = false;

    private void OnTriggerEnter(Collider coll_Other)
    {
        if(coll_Other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("I Can Grab");
            bl_CanGrab = true;
        }
    }

    private void OnTriggerExit(Collider coll_Other)
    {
        Debug.Log("I Can not Grab");
        bl_CanGrab = false;
    }
}
