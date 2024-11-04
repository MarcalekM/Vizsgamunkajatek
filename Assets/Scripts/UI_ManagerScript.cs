using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManagerScript : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] Canvas Stats;
    [SerializeField] Button StatsClose;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
    public void CloseStats()
    {
        Stats.gameObject.SetActive(false);
        UI.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
}
