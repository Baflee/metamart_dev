using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ShopTarget : MonoBehaviour
{
    [SerializeField] public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = respawnPoint.transform.position;
            other.transform.rotation = Quaternion.Euler(0, 0, 0);
            Physics.SyncTransforms();
        }
    }
}