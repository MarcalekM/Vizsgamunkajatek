using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ManagerScript : MonoBehaviour
{
    PlayerController player;

    [SerializeField] Canvas UI;
    [SerializeField] Canvas SubMenu;
    [SerializeField] Canvas Menu;
    [SerializeField] Canvas Stats;
    [SerializeField] Canvas Death;

    [SerializeField] Button MenuBtn;
    [SerializeField] Button StatsBtn;



    [SerializeField] Button SubMenuClose;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        MenuBtn.onClick.AddListener(OpenMenu);
        StatsBtn.onClick.AddListener(OpenStats);

        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", .5f);
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDeathScreen()
    {
        UI.gameObject.SetActive(false);
        Death.gameObject.SetActive(true);
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
    public void OpenMenu()
    {
        Menu.gameObject.SetActive(true);
        Stats.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    public void OpenStats()
    {
        Menu.gameObject.SetActive(false);
        Stats.gameObject.SetActive(true);
        Time.timeScale = 0;
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

    public void GoBackToMenu()
    {
        if (Menu_UI_Manager.UserData is not null)
        {
            if (player.inArena && player.kills > Menu_UI_Manager.UserData.high_score)
            {
                Menu_UI_Manager.UserData.high_score = player.kills;
                Menu_UI_Manager.SaveUserToDB(this);
            }
            else if (!player.inArena)
            {
                player.JsonSavePlayer(SceneManager.GetActiveScene().name);
            }
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
