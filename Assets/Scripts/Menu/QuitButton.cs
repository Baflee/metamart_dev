using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    public void Inscription()
    {
        Application.OpenURL("http://metamart.eri.gg/auth/signup");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

