using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

enum DayNight
{
    Day, Night
}

public class WorldLightScript : MonoBehaviour
{
    [SerializeField] DayNight dayNight;
    [SerializeField] Color dayColor;
    [SerializeField] Color nightColor;

    [SerializeField] GameObject nightGO;

    Light2D light2D;

    public int StartDayNight(int x)
    {
        if (x == 0)
        {
            ActivateDay();
        }
        else if (x == 1)
        {
            ActivateNight();
        }
        return 1;
    }

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (dayNight == DayNight.Day)
        {
            ActivateDay();
        }
        else if (dayNight == DayNight.Night)
        {
            ActivateNight();
        }
    }
    void ActivateDay()
    {
        light2D.color = dayColor;
        nightGO.SetActive(false);
        dayNight = DayNight.Day;
    }
    void ActivateNight()
    {
        light2D.color = nightColor;
        nightGO.SetActive(true);
        dayNight = DayNight.Night;
    }

}
