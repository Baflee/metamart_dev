using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Esc : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject Inventory;

    [SerializeField]
    private bool isEsc;

    bool UpdateInventory = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEsc = !isEsc;
        }

        if (isEsc)
        {
            ActivateMenu();

            if (UpdateInventory == true)
            {
                Inventory.GetComponent<Inventory>().ShowInventory();
                UpdateInventory = false;
            }
        }
        else
        {
            UpdateInventory = true;
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void DeactivateMenu()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void Reprendre()
    {
        isEsc = !isEsc;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Deconnection()
    {
        GameObject ID = GameObject.Find("ID");
        if (ID)
        {
            Destroy (ID);
        }
        SceneManager.LoadScene("Login");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
