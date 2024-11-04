using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatScript : MonoBehaviour
{
    [SerializeField] PlayerController stats;
    [SerializeField] Button HP;
    [SerializeField] Button Meele;
    [SerializeField] Button Magic;
    [SerializeField] Button Shield;

    [SerializeField] TextMeshProUGUI UI_HP;
    [SerializeField] TextMeshProUGUI UI_Meele;
    [SerializeField] TextMeshProUGUI UI_Magic;
    [SerializeField] TextMeshProUGUI UI_Shield;

    // Update is called once per frame
    void Update()
    {
        if (stats.SP > 0)
        {
            HP.gameObject.SetActive(true);
            Meele.gameObject.SetActive(true);
            Magic.gameObject.SetActive(true);
            Shield.gameObject.SetActive(true);
        }
        else
        {
            HP.gameObject.SetActive(false);
            Meele.gameObject.SetActive(false);
            Magic.gameObject.SetActive(false);
            Shield.gameObject.SetActive(false);
        }

        UI_HP.text = "HP: " + stats.HP;
        UI_Meele.text = "Meele: " + stats.MeeleDamage;
        UI_Magic.text = "Magic: " + stats.MagicDamage;
        UI_Shield.text = "MaxShield: " + stats.MaxShield;

        if (Input.GetKeyDown(KeyCode.E)) gameObject.SetActive(true);
    }

    public void Add_HP()
    {
        stats.SP--;
        stats.HP += 2;
        Add_Lv();
    }

    public void Add_Meele()
    {
        stats.SP--;
        stats.MeeleDamage += 2;
        Add_Lv();
    }

    public void Add_Magic()
    {
        stats.SP--;
        stats.MagicDamage += 2;
        Add_Lv();
    }

    public void Add_Shield() {
        stats.SP--;
        stats.MaxShield += 2;
        Add_Lv();
    }

    public void Add_Lv()
    {
        stats.Lv++;
    }

}
