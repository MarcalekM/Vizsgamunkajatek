using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_UI_Manager : MonoBehaviour
{
    [SerializeField] Canvas NewPlayer;
    [SerializeField] Canvas Login;
    [SerializeField] Canvas LoggedIn;
    [SerializeField] Canvas Registration;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginActive(){
        NewPlayer.gameObject.SetActive(false);
        Login.gameObject.SetActive(true);
    }

    public void LoggedInActive(){
        if(Login.isActiveAndEnabled)Login.gameObject.SetActive(false);
        if(Registration.isActiveAndEnabled)Registration.gameObject.SetActive(false);
        LoggedIn.gameObject.SetActive(true);
    }
    public void RegistrationActive()
    {
       NewPlayer.gameObject.SetActive(false);
       Registration.gameObject.SetActive(true);
    }
    
}
