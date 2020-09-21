using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpawnPointBehaviour : MonoBehaviour
{
    public bool lookAtTargetPoint = false;
    private Transform otherTargetPoint;
    private AIController aiController;

    void Awake()
    {
        //renderer.enabled = false;

        if (Application.isPlaying)
        {
            if (gameObject.GetComponent<Collider>() != null)
            {
                Destroy(gameObject.GetComponent<Collider>());
            }

            if (lookAtTargetPoint)
            {
                
                aiController = transform.parent.gameObject.GetComponentInChildren<AIController>() as AIController;
                if (aiController.target.gameObject.GetComponentInChildren<TargetPointBehaviour>() != null)
                {
                    //TargetPoint-Transform des Ziels zuweisen (Ziel aller Raycasts)                
                    TargetPointBehaviour otherTpScript = aiController.target.gameObject.GetComponentInChildren<TargetPointBehaviour>();
                    otherTargetPoint = otherTpScript.gameObject.transform;
                }
            }
        }
        
    }

    public void Update()
    {
        if (Application.isPlaying)
        {
            if (lookAtTargetPoint)
            {
                gameObject.transform.LookAt(otherTargetPoint);
            }
        }        
    }

    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {            
            Gizmos.color = Color.blue;            
            Gizmos.DrawWireSphere(gameObject.transform.position, 0.1f);            
        }
    }	
}
