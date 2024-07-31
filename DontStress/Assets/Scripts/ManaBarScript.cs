using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarScript : MonoBehaviour 
{
    private Image barImage;
    private Mana mana;

    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();
        barImage.fillAmount = .3f;

        mana = new Mana();
    }

    private void Update()
    {
        mana.Update();
        UpdateManaBar();
    }

    private void UpdateManaBar()
    {
        barImage.fillAmount = mana.GetManaNormalized();
    }

    public bool HasEnoughMana(int amount)
    {
        return mana.GetManaAmount() >= amount;
    }

    public void SpendMana(int amount)
    {
        mana.SpendMana(amount);
        UpdateManaBar();
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
        if (manaAmount >= amount)
        {
            manaAmount -= amount;
        }
        else
        {
            manaAmount = 0;
        }
    }

    public float GetManaAmount()
    {
        return manaAmount;
    }

    public float GetManaNormalized()
    {
        return manaAmount / manaMax;
    }
}
