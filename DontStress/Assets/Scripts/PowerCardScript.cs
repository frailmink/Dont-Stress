using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerCardScript : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> powers;

    private List<GameObject> randomPowers = new List<GameObject>();

    // Start is called before the first frame update
    // private void OnEnable()
    // {
    //     // gameObject.SetActive(true);
    //     GlobalVariables.Paused = true;
    //     Time.timeScale = 0f;
    // }
    // 
    // private void OnDisable()
    // {
    //     // gameObject.SetActive(true);
    //     GlobalVariables.Paused = false;
    //     Time.timeScale = 1f;
    // }

    public void NewCards()
    {
        randomPowers.Clear();
        Transform parent = gameObject.transform;
        int numChildren = parent.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            // gets the actual card button that it is going to change
            Transform child = parent.transform.GetChild(i).transform.GetChild(0);

            // randomly selects a power from the list
            int randInt = Random.Range(0, powers.Count);
            GameObject power = powers[randInt];
            randomPowers.Add(power);

            // changes all the text and images of the card
            child.Find("Title").GetComponent<TMP_Text>().text = power.gameObject.name;
            child.Find("Description").GetComponent<TMP_Text>().text = power.gameObject.name;
            Image cardImage = child.Find("Image").GetComponent<Image>();
            SpriteRenderer towerRend = power.gameObject.GetComponent<SpriteRenderer>();
            cardImage.color = towerRend.color;
            cardImage.sprite = towerRend.sprite;
        }
    }

    public void ChangePowerToCard(int index)
    {
        PowerButtonPressScript script = player.GetComponent<PowerButtonPressScript>();

        // changes the power function in the player to the one in the random power
        script.SwitchPower(randomPowers[index].GetComponent<PowerScript>().Run);
    }
}
