using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal: MonoBehaviour
{
    public void CargarEscena(string NIVEL1)
    {
        SceneManager.LoadScene(NIVEL1);
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}