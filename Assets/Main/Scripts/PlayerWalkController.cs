using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkController : MonoBehaviour
{
    [SerializeField]
    private int int_PlayerSpeed;

    [SerializeField]
    public GameObject obj_Wall;

    [SerializeField]
    public int int_Count;

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
            transform.position = transform.position + Camera.main.transform.forward * int_PlayerSpeed * Time.deltaTime;
        }
        if(int_Count == 4)
        {
            obj_Wall.SetActive(false);
        }
        //Fin
    }

    private void OnTriggerEnter(Collider coll_Other)
    {
        if(coll_Other.gameObject.CompareTag("Coin"))
        {
            Destroy(coll_Other.gameObject);
            int_Count++;
        }
    }
}
