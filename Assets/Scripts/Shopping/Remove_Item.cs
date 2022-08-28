using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remove_Item : MonoBehaviour
{
    [SerializeField]
    private GameObject Inventory;

    [SerializeField]
    public string id;

    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener (RemoveItemOnClick);
    }

    void RemoveItemOnClick()
    {
        Inventory.GetComponent<Inventory>().RemoveInventory(id);
        Inventory.GetComponent<Inventory>().ShowInventory();
    }
}
