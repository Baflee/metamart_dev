using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Text;
public class LoadingProducts : MonoBehaviour
{
    [SerializeField]
    Database_Products Database_Products;

    [SerializeField] string store;

    [SerializeField] string url;

    [SerializeField]
	public class Store
	{
	    public string _id;
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
	    public Data[] data;
	}	

	[SerializeField]
	public class Data
	{
	    public string _id;
	    public string store_id;
	    public string name;	
	    public string description;
	    public double price;
	    public ObjectInfo game_object;    
	}

	[SerializeField]
	public class ObjectInfo 
	{
		public float[] position;
		public float[] location;
		public float[] scale;
	}	

    // Start is called before the first frame update
    void Start()
    {
    	Products();
    }

	void Products() => StartCoroutine(Products_Coroutine());

    IEnumerator Products_Coroutine()
    {

	Store storeInfo = new Store();
	    storeInfo._id = store;

	string data = JsonUtility.ToJson(storeInfo);
	Debug.Log(data);


		User_Info User_Info = GameObject.Find("ID").GetComponent<User_Info>();

		if(!User_Info) {
			Debug.Log("dead user");
		}

		Debug.Log(User_Info.token);
        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + User_Info.token);
        var jsonBytes = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);	            
        }
        else
        {

            if(request.downloadHandler.text != null) {
				Debug.Log(request.downloadHandler.text);	
				Info Info = JsonConvert.DeserializeObject<Info>(request.downloadHandler.text);
				Details Details = Info.details;
				Data[] Data = Details.data;
				GameObject[] Products = Database_Products.products;
				Vector3 ProductVecPosition;
				Vector3 ProductVecRotation;
				Vector3 ProductVecScale;
				ObjectInfo ObjectInfo;
				foreach(GameObject Product in Products) {
					Debug.Log("Datas " + Data);
					foreach(Data i in Data){
					Debug.Log("Data " + i);
						if(i.name == Product.name) {
							ObjectInfo = i.game_object;
        					Quaternion productsRotation = Quaternion.identity;
        					ProductVecRotation = new Vector3(ObjectInfo.location[0],ObjectInfo.location[1],ObjectInfo.location[2]);
        					ProductVecPosition = new Vector3(ObjectInfo.position[0],ObjectInfo.position[1],ObjectInfo.position[2]);
        					ProductVecScale = new Vector3(ObjectInfo.scale[0],ObjectInfo.scale[1],ObjectInfo.scale[2]);		
        					productsRotation.eulerAngles = ProductVecRotation;
        					GameObject AddedProduct = Instantiate(Product, ProductVecPosition, productsRotation);
							var Script = AddedProduct.AddComponent<Product_Info>();
							Script.id = i._id;
							Script.name = i.name;
							Script.price = i.price;
        					AddedProduct.transform.localScale = ProductVecScale;
						}
					}
				}
            }
        }
        Debug.Log(data);
    }

}