using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGameController : MonoBehaviour
{
    [SerializeField]
    private string str_LevelName;

    private void OnTriggerEnter(Collider coll_Other)
    {
        if(coll_Other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(str_LevelName);
        }
    }
}
