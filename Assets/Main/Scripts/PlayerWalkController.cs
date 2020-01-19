using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkController : MonoBehaviour
{
    [SerializeField]
    private GameObject obj_Player;

    [SerializeField]
    private int int_PlayerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Se mueve en todo momento hacia donde mire la cámara.
        //obj_Player.transform.position = obj_Player.transform.position + Camera.main.transform.forward * int_PlayerSpeed * Time.deltaTime;
        //Fin

        //Se mueve hacia donde mire la camara al tocar sobre la pantalla del celular,(para cardboards).
        if (Input.GetButton("Fire1"))
        {
            obj_Player.transform.position = obj_Player.transform.position + Camera.main.transform.forward * int_PlayerSpeed * Time.deltaTime;
        }
        //Fin


    }
}
