using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarScript : MonoBehaviour
{
    private Image barImage;
    private Mana mana;

    public int manaLoss = 20;

    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();
        barImage.fillAmount = .3f;

        mana = new Mana();
    }

    private void Update()
    {
        mana.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mana.SpendMana(manaLoss); // Adjust the amount of mana spent as needed
        }

        barImage.fillAmount = mana.GetManaNormalized();
    }

}

public class Mana
{

    public const int manaMax = 100;

    private float manaAmount;
    private float manaRegen;

    public Mana()
    {
        manaAmount = 0;
        manaRegen = 10f;
    }

    public void Update()
    {
        manaAmount += manaRegen * Time.deltaTime;
        if (manaAmount > manaMax)
        {
            manaAmount = manaMax;
        }
    }

    public void SpendMana(int amount)
    {
        if(manaAmount >= amount)
        {
            manaAmount -= amount;
        }
        else
        {
            manaAmount = 0;
        }
    }

    public float GetManaNormalized()
    {
        return manaAmount/manaMax;
    }
}
