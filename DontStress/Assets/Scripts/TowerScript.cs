using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerScript : MonoBehaviour
{
    public int strength = 1;
    public int speed = 1;

    public virtual void EnableScript()
    {
        this.enabled = true;
    }

    public virtual void DisableScript()
    {
        this.enabled = false;
    }

    public void IncreaseStrength(int amount)
    {
        strength += amount;
    }

    public void IncreaseSpeed(int amount)
    {
        speed += amount;
    }

    public void OpenUpgradeUI(GameObject upgradeUI)
    {
        Button[] childButtons = upgradeUI.GetComponentsInChildren<Button>();
        foreach (Button btn in childButtons)
        {
            if (btn.gameObject.name == "Strength")
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => IncreaseStrength(10));
            } else if (btn.gameObject.name == "Speed")
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => IncreaseSpeed(10));
            }
        }
        upgradeUI.SetActive(true);
    }
}
