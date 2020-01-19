using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabController : MonoBehaviour
{
    [SerializeField]
    private GameObject obj_Ball;

    [SerializeField]
    private GameObject obj_PlayerHand;

    [SerializeField]
    private float fl_HandPower;

    private bool bl_BallInHand = false;
    private Collider coll_Ball;
    private Rigidbody rb_Ball;
    private Camera cam_MainCamera;
    private GrabController gc_GrabControlleR;
    


    // Start is called before the first frame update
    void Start()
    {
        coll_Ball = obj_Ball.GetComponent<SphereCollider>();
        rb_Ball = obj_Ball.GetComponent<Rigidbody>();
        cam_MainCamera = obj_PlayerHand.GetComponentInParent<Camera>();
        gc_GrabControlleR = obj_PlayerHand.GetComponent<GrabController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(!bl_BallInHand && gc_GrabControlleR.bl_CanGrab)
            {
                coll_Ball.isTrigger = true;
                obj_Ball.transform.SetParent(obj_PlayerHand.transform);
                obj_Ball.transform.localPosition = new Vector3(0f, -0.672f, 0f);
                rb_Ball.velocity = Vector3.zero;
                rb_Ball.useGravity = false;
                bl_BallInHand = true;
            }
            else if(bl_BallInHand)
            {
                coll_Ball.isTrigger = false;
                rb_Ball.useGravity = true;
                this.GetComponent<PlayerGrabController>().enabled = false;
                obj_Ball.transform.SetParent(null);
                rb_Ball.velocity = cam_MainCamera.transform.rotation * Vector3.forward * fl_HandPower;
                bl_BallInHand = false;
            }
        }
    }
}
