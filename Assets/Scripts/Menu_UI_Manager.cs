using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

class ApiUserLoginData
{
    public string username;
    public string password;
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

public class ApiUserData
{
    public string username;
    public int id;
    public long high_score;
    public string json_save;
}

public class Menu_UI_Manager : MonoBehaviour
{
    [SerializeField] Canvas NewPlayer;
    [SerializeField] Canvas Login;
    [SerializeField] Canvas LoggedIn;
    [SerializeField] Canvas Registration;
    [SerializeField] Canvas Settings;
    [SerializeField] Canvas Info;

    private Canvas[] canvases;

    [SerializeField] private TMP_InputField LoginUsernameText;
    [SerializeField] private TMP_InputField LoginPasswordText;
    
    [SerializeField] private TMP_InputField RegistrationUsernameText;
    [SerializeField] private TMP_InputField RegistrationPasswordText;
    
    [SerializeField] private TextMeshProUGUI MessageText;
    [SerializeField] private GameObject MessageBox;

    [SerializeField] private Animator BlackBG;
    [SerializeField] private TMPro.TextMeshProUGUI LoggedInUsernameWelcome;

    public static string LoginToken;
    public static ApiUserData UserData;
    
    // Start is called before the first frame update
    void Start()
    {
        var btn = MessageBox.gameObject.GetComponentInChildren<Button>();
        btn.onClick.AddListener(DismissBox);
        canvases = new Canvas[] { NewPlayer, Login, LoggedIn, Registration, Settings, Info };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetAllCanvasFalse()
    {
        foreach (Canvas c in canvases)
            c.gameObject.SetActive(false);
    }

    private void DelayedCanvasShow(Canvas canvas)
    {
        Task.Delay(900).ContinueWith((task) => canvas.gameObject.SetActive(true), TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void NewPlayerActive()
    {
        SetAllCanvasFalse();
        ToggleBackground("ToSmallTrigger");

        DelayedCanvasShow(NewPlayer);
    }
    public void LoggedInActive()
    {
        SetAllCanvasFalse();
        StartCoroutine(ApiPlayerLogin());
    }

    public void GoHome()
    {
        SetAllCanvasFalse();
        if (LoginToken == null)
        {
            NewPlayerActive();
        }
        else
        {
            ToggleBackground("ToSmallTrigger");
            DelayedCanvasShow(LoggedIn);
        }
    }

    

    public void NewPlayerAfterLogOut()
    {
        LoginToken = null;
        SetAllCanvasFalse();
        ToggleBackground("ToFullTrigger");
        Task.Delay(900).ContinueWith((task) => ToggleBackground("ToSmallTrigger"), TaskScheduler.FromCurrentSynchronizationContext());
        DelayedCanvasShow(NewPlayer);
    }
    public void SetMenuActive(Canvas canvas)
    {
        SetAllCanvasFalse();
        ToggleBackground("ToFullTrigger");
        DelayedCanvasShow(canvas);
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
            LoggedInUsernameWelcome.text = $"�dv, {loginData.username}!";
            var res = JsonUtility.FromJson<ApiUserLoginResponse>(www.downloadHandler.text);
            LoginToken = res.access_token;
            Debug.Log(LoginToken);
            Registration.gameObject.SetActive(false);
            ToggleBackground("ToSmallTrigger");
            DelayedCanvasShow(LoggedIn);
        }
    }
    IEnumerator ApiPlayerLogin()
    {
        var loginData = new ApiUserLoginData();
        loginData.username = LoginUsernameText.text;
        loginData.password = LoginPasswordText.text;
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
            LoggedInUsernameWelcome.text = $"�dv, {loginData.username}!";
            var res = JsonUtility.FromJson<ApiUserLoginResponse>(www.downloadHandler.text);
            LoginToken = res.access_token;
            Debug.Log(LoginToken);
            StartCoroutine(GetUserInfo());
            Login.gameObject.SetActive(false);
            ToggleBackground("ToSmallTrigger");
            DelayedCanvasShow(LoggedIn);
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
    
    private IEnumerator GetUserInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.j4f.teamorange.hu/users/me");
        www.SetRequestHeader("Authorization", "Bearer " + LoginToken);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            UserData = null;
        }
        else
        {
            UserData = JsonUtility.FromJson<ApiUserData>(www.downloadHandler.text);
            Debug.Log(UserData.username);
        }
    }
    
}
