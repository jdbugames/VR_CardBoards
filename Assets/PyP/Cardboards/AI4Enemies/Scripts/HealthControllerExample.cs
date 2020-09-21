using UnityEngine;
using System.Collections;

public class HealthControllerExample : MonoBehaviour
{   
    public float health = 100;
    //public GUIText guiHealth;

    protected void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        ApplyDamage(10);

    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Debug.Log("die");
        }
        else
        {
            //Debug.Log("hit: " + damage);            
        }
    }
    void OnGUI()
    {        
        GUILayout.Label("Player Health: " + health);
    }

}
