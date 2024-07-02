using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject deathMessage;
    public GameObject resetButton;

    public void TriggerGameOver()
    {
        deathMessage.SetActive(true);
        resetButton.SetActive(true);
    }
        public void TriggerGameOverFalse()
    {
        deathMessage.SetActive(false);
        resetButton.SetActive(false);
    }
}
