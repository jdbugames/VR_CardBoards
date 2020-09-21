using UnityEngine;
using System.Collections;
//using EnemyAI;

public class EventListenerExample : MonoBehaviour
{
    public GameObject smoke;
    //public GUIText guiMessages;
    private string message;
    private int destroyedCounter = 0;
    void OnEnable()
    {
        AIController.onIsDying += onIsDying;
        AIController.onIsDead += onIsDead;
        AIController.onHit += onHit;       
        
    }

    void OnDisable()
    {
        AIController.onIsDying -= onIsDying;
        AIController.onIsDead -= onIsDead;
        AIController.onHit -= onHit;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onIsDying(AIEventArgs e)
    {
        message = e.name + " is Dying at position " + e.position + ".";
        Debug.Log(message);
        StartCoroutine(ShowMessage(message,1));

        if (e.name != "soldier")//2011-05-28-1
        {                       //2011-05-28-1
            GameObject smokeObject = Instantiate(smoke, e.position, e.rotation) as GameObject;
            Destroy(smokeObject, 4);
        }                       //2011-05-28-1

        destroyedCounter++;
    }

    void onHit(AIEventArgs e)
    {
        message = e.name + " is be hit. Current health is " + e.health + ".";
        Debug.Log(message);
        StartCoroutine(ShowMessage(message,1));
    }

    void onIsDead(AIEventArgs e)
    {
        message = e.name + " is dead.";
        Debug.Log(message);
        StartCoroutine(ShowMessage(message,2));
    }

    IEnumerator ShowMessage(string text, float seconds)
    {
        //guiMessages.text = text;
        yield return new WaitForSeconds(seconds);
        //guiMessages.text = "";
    }
    void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.Label("Destroyed: " + destroyedCounter);
    }
}
