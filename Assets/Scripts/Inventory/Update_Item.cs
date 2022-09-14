using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Update_Item : MonoBehaviour
{
    [SerializeField]
    private GameObject Inventory;

    [SerializeField]
    private InputField inputQuantity;

    [SerializeField]
    public string id;

    void Start()
    {
        Button btnmore =
            this.transform.Find("ButtonMore").GetComponent<Button>();
        Button btnless =
            this.transform.Find("ButtonLess").GetComponent<Button>();
        btnless.onClick.AddListener (DeduceItem);
        btnmore.onClick.AddListener (AddItem);
    }

    void DeduceItem()
    {
        int quantity =
            int
                .Parse(this
                    .transform
                    .Find("InputField")
                    .Find("Placeholder")
                    .GetComponent<Text>()
                    .text);
        if (quantity > 1)
        {
            Inventory.GetComponent<Inventory>().AddInventory(id, -1);
        }
        else if (quantity <= 1)
        {
            Inventory.GetComponent<Inventory>().RemoveInventory(id);
        }
    }

    void AddItem()
    {
        int quantity =
            int
                .Parse(this
                    .transform
                    .Find("InputField")
                    .Find("Placeholder")
                    .GetComponent<Text>()
                    .text);
        Inventory.GetComponent<Inventory>().AddInventory(id, 1);
    }
}
