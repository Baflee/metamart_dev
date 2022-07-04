using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public GameObject[] Spawn;
    public string[] ShopName;
    public List<string> Shops = new List<string>();

    [SerializeField]
    public Dropdown m_Dropdown;
    int m_DropdownValue;
    int m_DropdownListNumber;

    public void List()
    {
        m_DropdownValue = m_Dropdown.value;
        m_DropdownListNumber = 5;

        for (int i = 0; i < m_DropdownListNumber; i++)
        {
            if (m_DropdownValue == i)
            {
                Player.transform.position = Spawn[i].transform.position;
                Player.transform.rotation = Quaternion.Euler(0, 0, 0);
                Physics.SyncTransforms();
            }
        }
        Debug.Log(Shops);
    }

    public void Start()
    {
        for (int i = 0; i < ShopName.Length; i++)
        {
            Shops.Add(ShopName[i]);
        }
        m_Dropdown.AddOptions(Shops);
    }
}