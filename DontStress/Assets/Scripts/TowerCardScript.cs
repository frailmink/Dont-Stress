using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerCardScript : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> towers;

    private List<GameObject> randomTowers = new List<GameObject>();

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
        randomTowers.Clear();
        Transform parent = gameObject.transform;
        int numChildren = parent.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            Transform child = parent.transform.GetChild(i).transform.GetChild(0);
            int randInt = Random.Range(0, towers.Count);
            GameObject tower = towers[randInt];
            randomTowers.Add(tower);
            child.Find("Title").GetComponent<TMP_Text>().text = tower.gameObject.name;
            child.Find("Description").GetComponent<TMP_Text>().text = tower.gameObject.name;
            Image cardImage = child.Find("Image").GetComponent<Image>();
            SpriteRenderer towerRend = tower.gameObject.GetComponent<SpriteRenderer>();
            cardImage.color = towerRend.color;
            cardImage.sprite = towerRend.sprite;
        }
    }

    public void AddCardToPlayer(int index)
    {
        PlayerScript script = player.GetComponent<PlayerScript>();
        script.towers.Add(randomTowers[index]);
    }
}
