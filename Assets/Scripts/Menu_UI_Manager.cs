using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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

public class ApiUserUpdateData
{
    public long high_score;
    public string json_save;
    public string username;
}
public class ApiUserData : ApiUserUpdateData
{
    public int id;
}

public class Menu_UI_Manager : MonoBehaviour
{
    [SerializeField] Canvas NewPlayer;
    [SerializeField] Canvas Login;
    [SerializeField] Canvas LoggedIn;
    [SerializeField] Canvas Registration;
    [SerializeField] Canvas Settings;
    [SerializeField] Canvas Info;
    [SerializeField] Canvas ButtonHelp;

    private Canvas[] canvases;

    [SerializeField] private TMP_InputField LoginUsernameText;
    [SerializeField] private TMP_InputField LoginPasswordText;
    
    [SerializeField] private TMP_InputField RegistrationUsernameText;
    [SerializeField] private TMP_InputField RegistrationPasswordText;
    
    [SerializeField] private TextMeshProUGUI MessageText;
    [SerializeField] private GameObject MessageBox;

    [SerializeField] private Animator BlackBG;
    [SerializeField] private TMPro.TextMeshProUGUI LoggedInUsernameWelcome;
    
    public static ApiUserData UserData;

    public static void SaveUserToDB(MonoBehaviour instance)
    //azért kell az instance, hogy statikus környezetből elérjük a startcoroutinet
    {
        instance?.StartCoroutine(UpdateUserInfo(UserData));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetString("LoginToken"));
        var btn = MessageBox.gameObject.GetComponentInChildren<Button>();
        btn.onClick.AddListener(DismissBox);
        canvases = new Canvas[] { NewPlayer, Login, LoggedIn, Registration, Settings, Info, ButtonHelp };

        SetAllCanvasFalse();
        StartCoroutine(GetUserInfo(true));
        
        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", .5f);
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");


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
    public void LoggedInActive()
    {
        SetAllCanvasFalse();
        StartCoroutine(ApiPlayerLogin());
    }

    public void GoHome()
    {
        SetAllCanvasFalse();
        ToggleBackground("ToSmallTrigger");
        DelayedCanvasShow((PlayerPrefs.GetString("LoginToken") == string.Empty ? NewPlayer : LoggedIn));
    }

    

    public void NewPlayerAfterLogOut()
    {
        PlayerPrefs.SetString("LoginToken", string.Empty);
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
    public void SimpleCanvasChange(Canvas canvas)
    {
        SetAllCanvasFalse();
        canvas.gameObject.SetActive(true);
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
            MessageText.text = "A megadott felhasználónév valószínűleg már foglalt.\n";
            MessageText.text += www.error;
            MessageBox.SetActive(true);
        }
        else {
            LoggedInUsernameWelcome.text = $"Üdv, {loginData.username}!";
            var res = JsonUtility.FromJson<ApiUserLoginResponse>(www.downloadHandler.text);
            PlayerPrefs.SetString("LoginToken", res.access_token);
            AfterSuccessfulLogin();
        }
    }
    IEnumerator ApiPlayerLogin()
    {
        var loginData = new ApiUserLoginData();
        loginData.username = LoginUsernameText.text;
        loginData.password = LoginPasswordText.text;
        UnityWebRequest www = UnityWebRequest.Post("https://api.j4f.teamorange.hu/users/login", JsonUtility.ToJson(loginData), "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            if (www.responseCode == 401)
            {
                MessageText.text = "A megadott felhasználónév vagy jelszó helytelen.";
            }
            else
            {
                MessageText.text = www.responseCode.ToString();
            }
            MessageBox.SetActive(true);
        }
        else
        {
            LoggedInUsernameWelcome.text = $"Üdv, {loginData.username}!";
            var res = JsonUtility.FromJson<ApiUserLoginResponse>(www.downloadHandler.text);
            PlayerPrefs.SetString("LoginToken", res.access_token);
            AfterSuccessfulLogin();
        }
    }
    private void AfterSuccessfulLogin()
    {
        Debug.Log(PlayerPrefs.GetString("LoginToken"));
        StartCoroutine(GetUserInfo());
        GoHome();
    }

    void DismissBox()
    {
        MessageBox.SetActive(false);
    }

    private void ToggleBackground(string toggleMode)
    {
        BlackBG.SetTrigger(toggleMode);
    }
    
    private static IEnumerator UpdateUserInfo(ApiUserData newData)
    {
        if (PlayerPrefs.GetString("LoginToken") != string.Empty)
        {
            var updateData = new ApiUserUpdateData();
            updateData.username = newData.username;
            updateData.high_score = newData.high_score;
            updateData.json_save = newData.json_save;
            UnityWebRequest www  = new UnityWebRequest($"https://api.j4f.teamorange.hu/users/{UserData.id}", "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(updateData));
            Debug.Log(JsonUtility.ToJson(updateData));
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler =  new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("LoginToken"));
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
        }
    }
    
    private IEnumerator GetUserInfo(bool rerender = false)
    {
        if (PlayerPrefs.GetString("LoginToken") != string.Empty)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://api.j4f.teamorange.hu/users/me");
            www.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("LoginToken"));
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                UserData = null;
                PlayerPrefs.SetString("LoginToken", string.Empty);
            }
            else
            {
                UserData = JsonUtility.FromJson<ApiUserData>(www.downloadHandler.text);
                LoggedInUsernameWelcome.text = $"Üdv, {UserData.username}!";
            }
        }
        if (rerender)
        {
            if (PlayerPrefs.GetString("LoginToken") == string.Empty)
                NewPlayer.gameObject.SetActive(true);
            else
                LoggedIn.gameObject.SetActive(true);
        }
    }
    
    public void OpenLink(string link) => Application.OpenURL(link);

    public void StartStoryMode()
    {
        
    }
}
