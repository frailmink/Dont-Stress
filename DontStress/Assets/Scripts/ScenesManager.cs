using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScenesManager : MonoBehaviour
{
    void Start()
    {
        if(SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            SceneManager.UnloadSceneAsync("MainMenu");
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
