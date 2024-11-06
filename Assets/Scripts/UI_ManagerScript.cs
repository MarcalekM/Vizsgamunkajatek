using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManagerScript : MonoBehaviour
{
    PlayerController player;

    [SerializeField] Canvas UI;
    [SerializeField] Canvas Stats;
    [SerializeField] Canvas Game;

    [SerializeField] Button StatsClose;
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
            Stats.gameObject.SetActive(true);
            UI.gameObject.SetActive(false);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Game.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void CloseStats()
    {
        Stats.gameObject.SetActive(false);
        UI.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void ResumeGame()
    {
        Game.gameObject.SetActive(false);
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
