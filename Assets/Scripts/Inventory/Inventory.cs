using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public GameObject[] inventoryItems;

    [SerializeField]
    string Information;

    [SerializeField]
    GameObject Next;

    [SerializeField]
    GameObject Before;

    [SerializeField]
    GameObject First;

    [SerializeField]
    int page = 0;

    [SerializeField]
    GameObject UI_ITEM;

    [SerializeField]
    GameObject UI_INTERFACE;

    [SerializeField]
    string url;

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
        public ProductInventory[] raw_data;

        public Product[] data;
    }

    [SerializeField]
    public class ProductInventory
    {
        public string product_id;

        public int quantity;
    }

    [SerializeField]
    public class Product
    {
        public string name;

        public int price;
    }

    void Start()
    {
        Next.GetComponent<Button>().onClick.AddListener(NextPage);
        Before.GetComponent<Button>().onClick.AddListener(PreviousPage);
        First.GetComponent<Button>().onClick.AddListener(FirstPage);
    }

    void NextPage()
    {
        page = page + 1;
        ShowInventory();
    }

    void PreviousPage()
    {
        page = page - 1;
        ShowInventory();
    }

    void FirstPage()
    {
        page = 0;
        ShowInventory();
    }

    void Update()
    {
        if (page == 0)
        {
            Before.SetActive(false);
            First.SetActive(false);
        }
        else if (page > 0)
        {
            Before.SetActive(true);
            First.SetActive(true);
        }
    }

    public void ShowInventory() => StartCoroutine(ShowInventory_Coroutine());

    public void AddInventory(string product_id, int quantity) =>
        StartCoroutine(AddInventory_Coroutine(product_id, quantity));

    public void RemoveInventory(string product_id) =>
        StartCoroutine(RemoveInventory_Coroutine(product_id));

    IEnumerator ShowInventory_Coroutine()
    {
        User_Info User_Info = GameObject.Find("ID").GetComponent<User_Info>();

        if (!User_Info)
        {
            Debug.Log("Token User Expired");
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
            Debug.Log("Inventory Update Show !");
            Debug.Log(request.downloadHandler.text);
            if (request.downloadHandler.text != null)
            {
                Info Info =
                    JsonConvert
                        .DeserializeObject<Info>(request.downloadHandler.text);
                Details Details = Info.details;
                Product[] Datas = Details.data;

                ProductInventory[] Raw_Datas = Details.raw_data;
                int lengthpage = 20 + (page * 20);
                int indexpage = 0 + (page * 20);
                for (int i = indexpage; i < lengthpage; i++)
                {
                    Debug.Log("debutpage : " + i);
                    Debug.Log("indexactuelvalue : " + indexpage);
                    Debug.Log("finpage : " + lengthpage);
                    if (i < Datas.Length)
                    {
                        inventoryItems[i - (page * 20)].SetActive(true);
                        inventoryItems[i - (page * 20)]
                            .transform
                            .Find("InventoryName")
                            .GetComponent<Text>()
                            .text = Datas[i].name;
                        inventoryItems[i - (page * 20)]
                            .transform
                            .Find("InventoryPrice")
                            .GetComponent<Text>()
                            .text = Datas[i].price + " €";
                        inventoryItems[i - (page * 20)]
                            .transform
                            .Find("InputField")
                            .Find("Placeholder")
                            .GetComponent<Text>()
                            .text = Raw_Datas[i].quantity.ToString();
                        inventoryItems[i - (page * 20)]
                            .transform
                            .Find("Button")
                            .GetComponent<Remove_Item>()
                            .id = Raw_Datas[i].product_id;
                    }
                    else
                    {
                        inventoryItems[i - (page * 20)].SetActive(false);
                    }
                }
            }
        }
    }

    IEnumerator AddInventory_Coroutine(string product_id, int quantity)
    {
        ProductInventory product = new ProductInventory();
        product.product_id = product_id;
        product.quantity = quantity;

        string data = JsonUtility.ToJson(product);

        User_Info User_Info = GameObject.Find("ID").GetComponent<User_Info>();

        if (!User_Info)
        {
            Debug.Log("Token User Expired");
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
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Item Remove completed!");

            if (request.downloadHandler.text != null)
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    IEnumerator UpdateInventory_Coroutine(string product_id, int quantity)
    {
        ProductInventory product = new ProductInventory();
        product.product_id = product_id;
        product.quantity = quantity;

        string data = JsonUtility.ToJson(product);

        User_Info User_Info = GameObject.Find("ID").GetComponent<User_Info>();

        if (!User_Info)
        {
            Debug.Log("Token User Expired");
        }

        var request = new UnityWebRequest(url, "patch");
        request.method = "patch";
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + User_Info.token);
        var jsonBytes = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Item Remove completed!");

            if (request.downloadHandler.text != null)
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    IEnumerator RemoveInventory_Coroutine(string product_id)
    {
        ProductInventory product = new ProductInventory();
        product.product_id = product_id;

        string data = JsonUtility.ToJson(product);

        User_Info User_Info = GameObject.Find("ID").GetComponent<User_Info>();

        if (!User_Info)
        {
            Debug.Log("Token User Expired");
        }

        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbDELETE);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + User_Info.token);
        var jsonBytes = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Erreur Remove : " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Item Remove completed!");

            if (request.downloadHandler.text != null)
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
