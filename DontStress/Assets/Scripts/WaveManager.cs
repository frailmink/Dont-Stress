using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public TextMeshProUGUI waveText;
    private int waveCount = 1;

    private void Awake()
    {
        UpdateUI();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddWave()
    {
        waveCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        waveText.text = "Waves: " + waveCount.ToString();
    }
}
