using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[CustomEditor(typeof(AIEnemyWaypointEditor))]
public class AIEnemyWaypointEditorEditor : Editor
{
    private static bool m_editMode = false;  
    private static string m_preName = "wp";
    private static string m_folderName = "wps";    
    private Material m_waypointMaterial;
    private GameObject m_container;
    public GameObject waypointFolder;
    
    //[MenuItem("GameObject/AI Driver Toolkit/AI Driver")]
    //static void CreateAIDPrototype()
    //{
    //    GameObject prefab = Resources.LoadAssetAtPath("Assets/AIDriverToolkit/Prefabs/AIDriverPrototype.prefab", typeof(GameObject)) as GameObject;                    
    //    GameObject newObject = Instantiate(prefab,Vector3.zero,Quaternion.identity) as GameObject;
    //    newObject.name = "AI Driver";
    //    //select new object
    //    UnityEngine.Object[] selectedObjects = new UnityEngine.Object[1];
    //    selectedObjects[0] = newObject;
    //    Selection.objects = selectedObjects;
    //}

    //[MenuItem("GameObject/AI Driver Toolkit/Buggy")]
    //static void CreateAIDBuggy()
    //{
    //    GameObject prefab = Resources.LoadAssetAtPath("Assets/AIDriverToolkit/Prefabs/AIBuggy.prefab", typeof(GameObject)) as GameObject;
    //    GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    //    newObject.name = "Buggy";
    //    //select new object
    //    UnityEngine.Object[] selectedObjects = new UnityEngine.Object[1];
    //    selectedObjects[0] = newObject;
    //    Selection.objects = selectedObjects;
    //}

    void OnSceneGUI()
    {
        
        if (m_editMode)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                           
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;               

                //2011-04-11 cse -B
                if (m_container == null)
                {
                    Debug.LogError("No container found. Please place waypoints in scenes directly after pressing the Waypoint Editor button.");
                    m_editMode = false;
                    Repaint();
                }
                //2011-04-11 cse -E

                if (m_editMode) //2011-04-11 cse
                {               //2011-04-11 cse
                    
                    if (Physics.Raycast(ray, out hit))
                    {
                        int counter = 1;
                        string fullPreName;
                        fullPreName = "/" + m_folderName + "/" + m_preName + "_";
                        while (GameObject.Find(fullPreName + counter.ToString()) != null)
                        {
                            counter++;
                        }

                        Undo.RegisterSceneUndo("Create new Waypoint");
                        //GameObject prefab = Resources.LoadAssetAtPath("Assets/AIDriverToolkit/Prefabs/Waypoint.prefab", typeof(GameObject)) as GameObject;
                        //GameObject waypoint = Instantiate(prefab) as GameObject;
                        GameObject waypoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Vector3 myPosition;
                        myPosition = hit.point;
                        myPosition.y = (float)myPosition.y + (float)(waypoint.transform.localScale.y / 2);

                        waypoint.transform.position = myPosition;
                        waypoint.name = m_preName + "_" + counter.ToString();
                        waypoint.transform.parent = m_container.transform;
                        waypoint.AddComponent<WaypointBehaviour>();                      
                        
                        EditorUtility.SetDirty(waypoint);
                        
                        AIEnemyWaypointEditor script = (AIEnemyWaypointEditor)target;                        
                        AIController aiController;
                        aiController = script.gameObject.GetComponentInChildren<AIController>() as AIController;                        

                        aiController.waypoints.Add(waypoint.transform);
                        EditorUtility.SetDirty(aiController);

                        int c = aiController.waypoints.Count;

                        for (int i= c-1; i>=0; i--)
                        {
                            if (aiController.waypoints[i] == null)
                            {
                                aiController.waypoints.RemoveAt(i);
                            }
                        }
                        //foreach (Transform tf in aiController.waypoints)
                        //{
                        //    if (tf == null)
                        //    {
                        //        aiController.waypoints.Remove(tf);
                        //    }
                        //}
                        EditorUtility.SetDirty(aiController);
                        ////rotate last WP 
                        //GameObject lastWP = GameObject.Find(fullPreName + (counter - 1).ToString());
                        //if (lastWP != null)
                        //{
                        //    lastWP.transform.LookAt(waypoint.transform);
                        //    EditorUtility.SetDirty(lastWP);
                        //}
                    }
                    m_editMode = false;
                }//2011-04-11 cse 
            }
        }
    }

    public override void OnInspectorGUI()
    {
        
        AIEnemyWaypointEditor script = (AIEnemyWaypointEditor)target;

        script.wpFolderName = EditorGUILayout.TextField("WP Parent", script.wpFolderName);
        script.wpPreFix = EditorGUILayout.TextField("WP Prefix", script.wpPreFix);      

        m_preName = script.wpPreFix;
        m_folderName = script.wpFolderName;      
        

        if (m_editMode)
        {
            if (GUILayout.Button("Right Click in Scene View"))
            {
                                
            }
        }
        else
        {
            if (GUILayout.Button("Press for new Waypoint"))
            {
                m_editMode = true;             
                                      
                m_container = GameObject.Find(m_folderName);                
                if (m_container == null)
                {
                    waypointFolder = new GameObject();
                    waypointFolder.name = m_folderName;
                    m_container = waypointFolder;                    
                }               
                
            }

        }     
        
    }
   
}
