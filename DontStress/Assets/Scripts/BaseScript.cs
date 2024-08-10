using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour, IInteractable
{
    public GameObject storeUI;

    public void Interact(GameObject g)
    {
        storeUI.SetActive(true);
    }
}
