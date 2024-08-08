using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncreaseScript : MonoBehaviour, IShopItem
{
    // SerializeField allows editting of value in unity inspector
    [SerializeField]
    public float movementIncrease = 5;

    public int price
    {
        get => 1;
    }

    public void Bought()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerScript>().MoveSpeed += movementIncrease;
    }
}
