using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    // Start is called before the first frame update

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void AddHealth(float health)
    {
        if (slider.value + health > slider.maxValue)
        {
            slider.value = slider.maxValue;

        }
    }


}
