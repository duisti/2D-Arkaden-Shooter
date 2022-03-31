using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class UI_PlayerHeat : MonoBehaviour
{
    PlayerStats playerStats;
    Slider slider;
    // Start is called before the first frame update
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    void Start()
    {
        Init();
    }

    void Init()
    {
        if (playerStats == null)
        {
            if (GameMaster.instance.CurrentPlayerObject != null)
            {
                playerStats = GameMaster.instance.CurrentPlayerObject.GetComponent<PlayerStats>();
            }
            else
            {
                playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats == null)
        {
            Init();
            return;
        }
        if (slider.maxValue != playerStats.MaxHeat) slider.maxValue = playerStats.MaxHeat;
        slider.value = playerStats.Heat;
    }
}
