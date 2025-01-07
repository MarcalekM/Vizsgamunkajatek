using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{ 
    [SerializeField] private PlayerController player;
    [SerializeField] private Image healthbar;

    private float health;
    private float targetFillAmount;

    private void Start()
    {
        health = player.HP;
        targetFillAmount = health / player.MaxHp;
        healthbar.fillAmount = targetFillAmount;
    }

    private void Update()
    {
        if (health != player.HP)
        {
            health = player.HP;
        }
        if (targetFillAmount > health / player.MaxHp)
        {
            targetFillAmount -= .01f;
            healthbar.fillAmount = targetFillAmount;
        }

    }

}
