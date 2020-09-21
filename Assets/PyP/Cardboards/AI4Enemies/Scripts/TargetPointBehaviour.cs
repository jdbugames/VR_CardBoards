using UnityEngine;
using System.Collections;
//using EnemyAI;

[ExecuteInEditMode]
public class TargetPointBehaviour : MonoBehaviour
{
    void Awake()
    {
        if (Application.isPlaying)
        {
            if (gameObject.GetComponent<Collider>() != null)
            {
                Destroy(gameObject.GetComponent<Collider>());
            }
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (gameObject.transform.parent.GetComponent<AIController>() != null)
            {
                Gizmos.color = Color.green;

                AIController aiController = gameObject.transform.parent.GetComponent<AIController>() as AIController;
                Debug.DrawRay(transform.position, -Vector3.up * aiController.targetPointHeight, Color.red);


            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere(gameObject.transform.position, 0.1f);
        }
    }
}
