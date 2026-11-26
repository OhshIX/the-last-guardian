using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void CargarEscena(string NIVEL1)
    {
        SceneManager.LoadScene(NIVEL1);
    }

    public void CrearPartidaMultiplayer(string NIVEL1)
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(NIVEL1, LoadSceneMode.Single);
    }

    public void UnirsePartidaMultiplayer()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}
