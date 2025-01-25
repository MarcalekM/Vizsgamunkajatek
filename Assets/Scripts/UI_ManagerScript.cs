using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManagerScript : MonoBehaviour
{
    PlayerController player;

    [SerializeField] Canvas UI;
    [SerializeField] Canvas SubMenu;

    [SerializeField] Button SubMenuClose;
    // Start is called before the first frame update
    void Start()
    {
         player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenSubMenu();
        }
    }
    public void OpenSubMenu()
    {
        SubMenu.gameObject.SetActive(true);
        UI.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    public void CloseSubMenu()
    {
        SubMenu.gameObject.SetActive(false);
        UI.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void SaveGame()
    {
        string filepath = Application.persistentDataPath + "/stats.txt";
        Debug.Log(filepath);
        using StreamWriter sw = new(
            path: filepath,
            append: false);
        sw.Write($"{player.Lv},{player.HP},{player.MeeleDamage},{player.MagicDamage},{player.kills},{player.SP},{player.MaxShield}");
    }
}
