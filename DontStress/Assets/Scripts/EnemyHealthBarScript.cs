using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealth(float currentValue, float maxValue)
    {
        slider.value = currentValue/maxValue;
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     slider.value;
    // }
}
