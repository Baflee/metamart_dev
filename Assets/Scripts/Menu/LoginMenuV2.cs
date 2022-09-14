using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginMenuV2 : MonoBehaviour
{
    [SerializeField]
    GameObject welcomePanel;

    [SerializeField]
    Text user;

    [Space]
    [SerializeField]
    InputField email;

    [SerializeField]
    InputField password;

    [SerializeField]
    Text errorMessages;

    [SerializeField]
    GameObject idTable;

    [SerializeField]
    Button loginButton;

    [SerializeField]
    string Information;

    [SerializeField]
    string url;

    [SerializeField]
    public class Account
    {
        public string email;

        public string password;
    }

    [SerializeField]
    public class Info
    {
        public string message;

        public bool success;

        public Details details;
    }

    [SerializeField]
    public class Details
    {
        public Data data;
    }

    [SerializeField]
    public class Data
    {
        public Session session;

        public string name;

        public string last_name;
    }

    [SerializeField]
    public class Session
    {
        public string token;
    }

    public void Start()
    {
        loginButton.onClick.AddListener (Login);
    }

    void Login() => StartCoroutine(Login_Coroutine());

    IEnumerator Login_Coroutine()
    {
        Account account = new Account();
        account.email = email.text;
        account.password = password.text;

        string data = JsonUtility.ToJson(account);

        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        var jsonBytes = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        errorMessages.text = "test1";
        if (request.isNetworkError || request.isHttpError)
        {
            Info Info =
                JsonConvert
                    .DeserializeObject<Info>(request.downloadHandler.text);
            if (Info.success == false)
            {
                errorMessages.text = Info.message;
            }
        }
        else
        {
            errorMessages.text = "test2";
            Debug.Log("Form upload complete!");

            if (request.downloadHandler.text != null)
            {
                errorMessages.text = "test3";
                errorMessages.text = request.downloadHandler.text;
                Info Info =
                    JsonConvert
                        .DeserializeObject<Info>(request.downloadHandler.text);
                Details Details = Info.details;
                Data Data = Details.data;
                Session Session = Data.session;

                if (Info.success == true)
                {
                    errorMessages.text = "test4";
                    errorMessages.text = "Bienvenue, " + Data.name;
                    User_Info user_data = idTable.GetComponent<User_Info>();
                    user_data.name = Data.name;
                    user_data.last_name = Data.last_name;
                    user_data.token = Session.token;
                    DontDestroyOnLoad (idTable);
                    SceneManager.LoadScene("Jeu");
                }
            }

            Debug.Log(request.downloadHandler.text);
        }
        Debug.Log (data);
    }
}
