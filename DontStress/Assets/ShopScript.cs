using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public GameObject shopItemDisplay;

    public List<GameObject> possibleItems;

    public int numItems;

    public int resetCost;

    private List<GameObject> remainingItems;
    private List<GameObject> itemsInShop;
    private int range;

    private void Awake()
    {
        itemsInShop = new List<GameObject>();
        remainingItems = new List<GameObject>(possibleItems);

        range = numItems;

        if (range > possibleItems.Count)
        {
            range = possibleItems.Count;
        }

        for (int i = 0; i < range; i++)
        {
            GameObject item = Instantiate(shopItemDisplay);
            item.transform.SetParent(gameObject.transform, false);

            DisplayItem(item);

            Button button = item.GetComponent<Button>();
            int index = i;
            GameObject itemDisplay = item;
            button.onClick.AddListener(() => BuyItem(index, itemDisplay));
        }
    }

    private void DisplayItem(GameObject item)
    {
        int rand = Random.Range(0, remainingItems.Count);
        GameObject randomItem = remainingItems[rand];
        remainingItems.RemoveAt(rand);

        itemsInShop.Add(randomItem);
        item.transform.Find("Image").GetComponentInChildren<Image>().sprite = randomItem.GetComponent<SpriteRenderer>().sprite;
        item.transform.Find("Image").GetComponentInChildren<Image>().color = randomItem.GetComponent<SpriteRenderer>().color;
    }

    public void ResetItems()
    {
        Debug.Log(resetCost);
        if (CoinManager.Instance.HasEnoughCoins(resetCost))
        {
            CoinManager.Instance.SpendCoins(resetCost);
            itemsInShop.Clear();
            remainingItems = new List<GameObject>(possibleItems);

            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Restock")
                {
                    child.gameObject.SetActive(true);
                    DisplayItem(child.gameObject);
                }
            }
        }
    }

    public void BuyItem(int index, GameObject itemDisplay)
    {
        IShopItem shopItem = itemsInShop[index].GetComponentInChildren<IShopItem>();

        Debug.Log(shopItem.price);

        if (CoinManager.Instance.HasEnoughCoins(shopItem.price))
        {
            CoinManager.Instance.SpendCoins(shopItem.price);
            // itemsInShop.RemoveAt(index);
            itemDisplay.SetActive(false);
            shopItem.Bought();
        }
    }
}
