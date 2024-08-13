using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wardrobe : MonoBehaviour, IInteractable
{
    public GameObject wardrobeUI;
    public Sprite playerSprite;

    public void Interact(GameObject g)
    {
        wardrobeUI.SetActive(true);
        wardrobeUI.transform.Find("Image").GetComponent<Image>().sprite = playerSprite;
    }
}
