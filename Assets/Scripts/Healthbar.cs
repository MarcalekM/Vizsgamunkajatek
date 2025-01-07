using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{ 
    [SerializeField] private PlayerController player;
    [SerializeField] private Image healthbar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI skillPointsText;

    private float health;
    private float targetFillAmount;

    private void Start()
    {
        health = player.HP;
        targetFillAmount = Normalize(health, 0,player.MaxHp, 0.322f, 1);
        healthbar.fillAmount = targetFillAmount;
    }

    private void Update()
    {
        if (health != player.HP)
        {
            health = player.HP;
        }
        targetFillAmount = Normalize(health, 0,player.MaxHp, 0.322f, 1);
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, targetFillAmount, Time.deltaTime);
        healthText.text = $"{player.HP}/{player.MaxHp}";
        skillPointsText.text = $"Skill Points: {player.SP}";

    }
    float Normalize(float val, float valmin, float valmax, float min, float max) 
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }

}
