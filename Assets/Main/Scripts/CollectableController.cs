using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    private Vector3 vec_Spin;

    // Update is called once per frame
    void Update()
    {
        vec_Spin = new Vector3(5f, 5f, 0);

        transform.Rotate(vec_Spin);
    }
}
