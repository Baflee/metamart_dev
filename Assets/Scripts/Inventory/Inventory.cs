using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Text;

public class Inventory : MonoBehaviour
{
	[SerializeField] string Information;
	
	[SerializeField] string url;

    [SerializeField]
	public class product
	{
	    public string product_id;
	    public int quantity;
	}

	public void ShowInventory() => StartCoroutine(ShowInventory_Coroutine());

    public void AddInventory(string product_id, int quantity) => StartCoroutine(ShowInventory_Coroutine(string product_id, int quantity));

    IEnumerator ShowInventory_Coroutine()
    {

		User_Info User_Info = GameObject.Find("ID").GetComponent<User_Info>();

		if(!User_Info) {
			Debug.Log("dead user");
		}

        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        request.SetRequestHeader("Content-Type", "application/json");
		request.SetRequestHeader("Authorization", "Bearer " + User_Info.token);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.downloadHandler.text);    
        }
        else
        {
            Debug.Log("Form upload complete!");

            if(request.downloadHandler.text != null) {
				Debug.Log(request.downloadHandler.text);
            }
        }
    }

    IEnumerator AddInventory_Coroutine(string product_id, int quantity)
    {

	Product product = new Product();
	    product.product_id = product_id;
	    product.quantity = quantity;

	string data = JsonUtility.ToJson(product);

		User_Info User_Info = GameObject.Find("ID").GetComponent<User_Info>();

		if(!User_Info) {
			Debug.Log("dead user");
		}

        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + User_Info.token);
        var jsonBytes = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Info Info = JsonConvert.DeserializeObject<Info>(request.downloadHandler.text);
			if(Info.success == false) {
				errorMessages.text = Info.message;
			}			            
        }
        else
        {
            Debug.Log("Form upload complete!");

            if(request.downloadHandler.text != null) {
				Info Info = JsonConvert.DeserializeObject<Info>(request.downloadHandler.text);
				Details Details = Info.details;
				Data Data = Details.data;
				Session Session = Data.session;

				if(Info.success == true) {
					errorMessages.text = "Bienvenue, " + Data.name;
					User_Info user_data = idTable.GetComponent<User_Info>();
					user_data.name = Data.name;
					user_data.last_name = Data.last_name;
					user_data.token = Session.token;
					DontDestroyOnLoad(idTable);
					SceneManager.LoadScene("Jeu");
				}						
            }

            Debug.Log(request.downloadHandler.text);
        }
        Debug.Log(data);
    }
}   
}
