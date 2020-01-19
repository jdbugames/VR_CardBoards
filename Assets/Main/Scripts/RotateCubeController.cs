using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCubeController : MonoBehaviour
{
    [SerializeField]
    private float fl_SpinForce;

    [SerializeField]
    private GameObject obj_Cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj_Cube.transform.Rotate(0, fl_SpinForce * Time.deltaTime, 0);
    }

    public void ChangeSpin()
    {
        fl_SpinForce = -fl_SpinForce;
    }
}
