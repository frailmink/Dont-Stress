using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour, IInteractable
{
    public void Interact(GameObject g)
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
