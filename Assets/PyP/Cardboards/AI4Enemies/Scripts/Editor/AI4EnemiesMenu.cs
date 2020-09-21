using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class AI4EnemiesMenu : MonoBehaviour
{

    [MenuItem("Component/AI 4 Enemies/AI Behaviour")]    
    static void AddAIController()
    {
        Selection.activeGameObject.AddComponent<AIAnimations>();
        Selection.activeGameObject.AddComponent<AIController>();
        AIEnemyWaypointEditor wpEditor = Selection.activeGameObject.AddComponent<AIEnemyWaypointEditor>() as AIEnemyWaypointEditor;
        wpEditor.wpFolderName = Selection.activeGameObject.name + " Waypoints";
        AddTargetPoint();
        Selection.activeGameObject.AddComponent<Rigidbody>();        
        Selection.activeGameObject.GetComponent<Rigidbody>().useGravity = false;
        Selection.activeGameObject.GetComponent<Rigidbody>().freezeRotation = true;        
        
    }


    [MenuItem("Component/AI 4 Enemies/Components/Spawn Point")]
    static void AddSpawnPointToObject()
    {

        AddSpawnPoint();

    }

    static void AddSpawnPoint()
    {
        //GameObject spawnPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject spawnPoint = new GameObject();
        //Destroy(spawnPoint.collider);
        spawnPoint.name = "SpawnPoint";
        spawnPoint.transform.parent = Selection.activeGameObject.transform;
        spawnPoint.transform.localPosition = new Vector3(0, 0, 1);
        spawnPoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        spawnPoint.transform.localRotation = Quaternion.identity;
        //spawnPoint.renderer.enabled = false;
        spawnPoint.AddComponent<SpawnPointBehaviour>();
    }

   
    //[MenuItem("Custom/Enemy AI/Waypoint")]
    //static void AddWaypointScript()
    //{        

    //    GameObject waypoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    Destroy(waypoint.collider);
    //    waypoint.name = "Waypoint";
      
    //    waypoint.AddComponent<WaypointBehaviour>();

    //    Ray ray = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        print("I'm looking at " + hit.transform.name);
    //        waypoint.transform.position = hit.point;
            
    //    }
    //    else
    //        print("I'm looking at nothing!");


    //}

    [MenuItem("Component/AI 4 Enemies/Components/Collision Damage Controller")]
    static void AddCollisionDamageController()
    {

        Selection.activeGameObject.AddComponent<CollisionDamageController>();

    }

    [MenuItem("Component/AI 4 Enemies/Components/Target Point")]
    static void AddTargetPointToObject()
    {

        AddTargetPoint();

    }

    static void AddTargetPoint()
    {
        //GameObject targetPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Destroy(targetPoint.collider);
        GameObject targetPoint = new GameObject();
        targetPoint.name = "TargetPoint";
        targetPoint.transform.parent = Selection.activeGameObject.transform;
        targetPoint.transform.localPosition = Vector3.zero;
        targetPoint.transform.localScale = new Vector3(0, 0, 0);
        targetPoint.transform.localRotation = Quaternion.identity;
        //targetPoint.renderer.enabled = false;
        targetPoint.AddComponent<TargetPointBehaviour>();
    }
	
}
