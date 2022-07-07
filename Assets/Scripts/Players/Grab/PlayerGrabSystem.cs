using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGrabSystem : MonoBehaviour
{
    float Yrot;

    RaycastHit hitPickup;

    [SerializeField]
    private GameObject CameraUIGrab;

    GameObject GetMouseHoverObject(float grabeRange)
    {
        //Check for Collider with Raycast
        Vector3 position = gameObject.transform.position;
        Vector3 target = position + Camera.main.transform.forward * grabeRange;
        if (Physics.Linecast(position, target, out hitPickup))
            return hitPickup.collider.gameObject;
        return null;
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (GetMouseHoverObject(4F))
        {
            if(hitPickup.transform.gameObject.tag == "Product") {
                var ProductInfo = hitPickup.transform.gameObject.GetComponent<Product_Info>();
                if (
                    Input.GetButtonDown("Use")
                )
                {
                    pauseMenuUI.GetComponent<InventoryMenu>().AddInventory(ProductInfo.id, 1);
                }
                float price = ProductInfo.price;
                CameraUIGrab.GetComponent<Text>().text = "[E] ACHETER " + ProductInfo.name + " " + price.ToString() + " Є";
                CameraUIGrab.SetActive(true);
            }
        } else {
            CameraUIGrab.SetActive(false);
        }
    }
}
