using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

class ApiUserLoginData
{
    public string felhasznalonev;
    public string jelszo;
}

class ApiUserLoginResponse
{

    public string access_token;
    public string token_type;

}

class ApiUserRegistrationData
{
    public string username;
    public string password;
}

public class Menu_UI_Manager : MonoBehaviour
{
    [SerializeField] Canvas NewPlayer;
    [SerializeField] Canvas Login;
    [SerializeField] Canvas LoggedIn;
    [SerializeField] Canvas Registration;

    [SerializeField] private TMP_InputField LoginUsernameText;
    [SerializeField] private TMP_InputField LoginPasswordText;
    
    [SerializeField] private TMP_InputField RegistrationUsernameText;
    [SerializeField] private TMP_InputField RegistrationPasswordText;
    
    [SerializeField] private TextMeshProUGUI MessageText;
    [SerializeField] private GameObject MessageBox;

    [SerializeField] private Animator BlackBG;

    public static string LoginToken;
    
    // Start is called before the first frame update
    void Start()
    {
        var btn = MessageBox.gameObject.GetComponentInChildren<Button>();
        btn.onClick.AddListener(DismissBox);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginActive(){
        NewPlayer.gameObject.SetActive(false);
        Login.gameObject.SetActive(true);
        ToggleBackground("ToFullTrigger");
    }

    public void LoggedInActive()
    {
        StartCoroutine(ApiPlayerLogin());
    }
    public void RegistrationActive()
    {
       NewPlayer.gameObject.SetActive(false);
       Registration.gameObject.SetActive(true);
       ToggleBackground("ToFullTrigger");
    }

    public void RegisterUser()
    {
        StartCoroutine(ApiPlayerRegister());
    }
    IEnumerator ApiPlayerRegister()
    {
        var loginData = new ApiUserRegistrationData();
        loginData.username = RegistrationUsernameText.text;
        loginData.password = RegistrationPasswordText.text;
        UnityWebRequest www = UnityWebRequest.Post("https://api.j4f.teamorange.hu/users/register", JsonUtility.ToJson(loginData), "application/json");
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            MessageText.text = "The specified username is most likely already taken.\n";
            MessageText.text += www.error;
            MessageBox.SetActive(true);
        }
        else {
            var res = JsonUtility.FromJson<ApiUserLoginResponse>(www.downloadHandler.text);
            LoginToken = res.access_token;
            Debug.Log(LoginToken);
            Registration.gameObject.SetActive(false);
            LoggedIn.gameObject.SetActive(true);
        }
    }
    IEnumerator ApiPlayerLogin()
    {
        var loginData = new ApiUserLoginData();
        loginData.felhasznalonev = LoginUsernameText.text;
        loginData.jelszo = LoginPasswordText.text;
        UnityWebRequest www = UnityWebRequest.Post("https://api.j4f.teamorange.hu/users/login", JsonUtility.ToJson(loginData), "application/json");
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            if (www.responseCode == 401)
            {
                MessageText.text = "The specified username or password is incorrect.";
            }
            else
            {
                MessageText.text = www.responseCode.ToString();
            }
            MessageBox.SetActive(true);
        }
        else {
            var res = JsonUtility.FromJson<ApiUserLoginResponse>(www.downloadHandler.text);
            LoginToken = res.access_token;
            Login.gameObject.SetActive(false);
            LoggedIn.gameObject.SetActive(true);
        }
    }

    void DismissBox()
    {
        MessageBox.SetActive(false);
    }

    private void ToggleBackground(string toggleMode)
    {
        BlackBG.SetTrigger(toggleMode);
    }
    
}
