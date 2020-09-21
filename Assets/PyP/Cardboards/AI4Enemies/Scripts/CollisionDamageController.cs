using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionDamageController : MonoBehaviour
{

    public List<Damage> CollisionDamages;

    protected void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        foreach (Damage cd in CollisionDamages)
        {
            if (cd.tag == tag)
            {
                int damage = Random.Range(cd.minDamage, cd.maxDamage);
                gameObject.SendMessage("ApplyDamage", damage);
            }
        }
    }

}
