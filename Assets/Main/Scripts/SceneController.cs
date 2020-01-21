using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(string str_SceneName)
    {
        SceneManager.LoadScene(str_SceneName);
    }

    public void QuitAPP()
    {
        Application.Quit();
    }
}
