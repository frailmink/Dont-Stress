using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsScript : MonoBehaviour
{
    public GameObject choiceCards;

    private void OnEnable()
    {
        // gameObject.SetActive(true);
        GlobalVariables.Paused = true;
        choiceCards.SetActive(true);
        Time.timeScale = 0f;
    }
    
    private void OnDisable()
    {
        // gameObject.SetActive(true);
        GlobalVariables.Paused = false;
        Time.timeScale = 1f;
    }
}
