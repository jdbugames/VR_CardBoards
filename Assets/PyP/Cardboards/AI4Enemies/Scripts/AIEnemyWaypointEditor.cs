using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AIEnemyWaypointEditor : MonoBehaviour
{
    //public string m_preName = "Waypoint";
    //public string m_folderName = "Waypoints";
    public string wpFolderName = "Waypoints";
    public string wpPreFix = "Waypoint";
    private AIController aiController;
    
    void Start()
    {
        //Waypoint.InitAll();
    }
    void Awake()
    {
        aiController = gameObject.GetComponentInChildren<AIController>() as AIController;
        //FillWaypointList();
    }

    //void FillWaypointList()
    //{
    //    bool found = true;
    //    int counter = 1;
    //    while (found)
    //    {
    //        GameObject go;
    //        string currentName;
    //        currentName = "/" + wpFolderName + "/" + wpPreFix + "_" + counter.ToString();            
    //        go = GameObject.Find(currentName);

    //        if (go != null)
    //        {               
    //            aiController.waypoints.Add(go.transform);
    //            counter++;
    //        }
    //        else
    //        {        
    //            found = false;
    //        }                      
    //    }
    //}

}
