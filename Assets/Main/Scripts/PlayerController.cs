using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float fl_Speed = 3.5f;

    [SerializeField]
    private int int_DistanceOfRaycast;

    private float fl_Gravity = 10f;
    private RaycastHit rh_Hit;
    private CharacterController cc_Controller;

    // Start is called before the first frame update
    void Start()
    {
        cc_Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float fl_Horizontal = Input.GetAxis("Horizontal");
        float fl_Vertical = Input.GetAxis("Vertical");
        Vector3 vec_Direction = new Vector3(fl_Horizontal, 0, fl_Vertical);
        Vector3 vec_Velocity = vec_Direction * fl_Speed;
        vec_Velocity = Camera.main.transform.TransformDirection(vec_Velocity);
        vec_Velocity.y -= fl_Gravity;
        cc_Controller.Move(vec_Velocity * Time.deltaTime);
    }
}
