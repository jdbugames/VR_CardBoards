using UnityEngine;
using System.Collections;

public class ShootingExample : MonoBehaviour {
    public GameObject projectile;
    public Transform spawnPoint;
    public SoundObject fightSound = new SoundObject();
    public SoundObject hitSound = new SoundObject();
    public Transform gun;
    public Transform ammo;
    public bool calculationDamage = false; //2011-05-26
    private Transform myTargetPoint;    //2011-05-26
	// Use this for initialization
	void Awake () {
        fightSound.gameObject = gameObject;
        fightSound.Init();
        hitSound.gameObject = gameObject;
        hitSound.Init();

        //2011-05-26 -B
        if (GetComponentInChildren<TargetPointBehaviour>() != null)
        {
            TargetPointBehaviour tpScript = GetComponentInChildren<TargetPointBehaviour>();
            myTargetPoint = tpScript.gameObject.transform;
        }
        //2011-05-26 -E
	}
	
	// Update is called once per frame
	void Update () {

        //2011-05-26 -B
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    StartCoroutine(Shoot());
        //}
       
        if (calculationDamage)
        {
            gun.gameObject.active = false;
            ammo.gameObject.active = false;
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(Hit());
            }
        }
        else
        {
            gun.gameObject.active = true;
            ammo.gameObject.active = true;
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(Shoot());
            }
        }
        //2011-05-26 -E
	
	}

    IEnumerator Shoot()
    {
        GameObject proj;
        yield return new WaitForSeconds(0.4f);
        proj = (GameObject)Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        fightSound.Play();
        proj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000); 
    }

    //2011-05-26 -B
    IEnumerator Hit()
    {
        RaycastHit hit;
        yield return new WaitForSeconds(0.2f);       
        hitSound.Play();
       
        if (Physics.Raycast(myTargetPoint.position, myTargetPoint.TransformDirection(Vector3.forward), out hit, 1.5f))
        {
            hit.transform.gameObject.SendMessage("ApplyDamage", 10, SendMessageOptions.DontRequireReceiver);           
        }       
    }

    void OnGUI()
    {
        GUILayout.Space(40);
        calculationDamage = GUILayout.Toggle(calculationDamage, "Beating (Calculation Damage)");        
       
    }
    //2011-05-26 -E

}
