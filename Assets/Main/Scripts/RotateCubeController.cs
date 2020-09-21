using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCubeController : MonoBehaviour
{
    [SerializeField]
    private float fl_SpinForce = 45f;

    [SerializeField]
    private bool bl_IsSpinning = false;

    // Update is called once per frame
    void Update()
    {
        if(bl_IsSpinning)
        {
            transform.Rotate(0, fl_SpinForce * Time.deltaTime, 0);
        }
        else if(!bl_IsSpinning)
        {
            transform.Rotate(0, 0, 0);
        }

    }

    public void ChangeSpin()
    {
        bl_IsSpinning = !bl_IsSpinning;
    }
}
