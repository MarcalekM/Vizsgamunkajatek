using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BossStage2 : MonoBehaviour
{
    [SerializeField] public bool Stage2 = false;
    [SerializeField] GameObject Stage2Decor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Stage2) StageChange();
    }

    public void StageChange()
    {
        GameObject.FindGameObjectsWithTag("Torch").ToList().ForEach(t => t.SetActive(false));
        Stage2Decor.SetActive(true);
    }
}
