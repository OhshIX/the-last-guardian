using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Configuración de Índices de Prefab (NetworkManager)")]
    [Tooltip("El ID del Player en la lista del NetworkManager (suele ser 0)")]
    public uint indicePlayer = 0;
    
    [Tooltip("El ID del Villano en la lista del NetworkManager (suele ser 1)")]
    public uint indiceVillano = 1;

    // 1. MODO UN JUGADOR (Clásico)
    public void CargarEscena(string nombreEscena)
    {
        if (ControladorPersistente.Instance != null) 
            ControladorPersistente.Instance.esPartidaMultijugador = false;

        SceneManager.LoadScene(nombreEscena);
    }

    // 2. CREAR PARTIDA COMO HOST (Player 1)
    public void CrearPartidaMultiplayer(string nombreEscena)
    {
        if (ControladorPersistente.Instance != null) 
            ControladorPersistente.Instance.esPartidaMultijugador = true;

        // Usamos la sintaxis limpia que tu Unity sí acepta sin errores
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[(int)indicePlayer].Prefab;

        NetworkManager.Singleton.StartHost();
        
        // El Host cambia la escena en red para todos
        NetworkManager.Singleton.SceneManager.LoadScene(nombreEscena, LoadSceneMode.Single);
    }

    // 3. UNIRSE A LA PARTIDA COMO CLIENTE (Villano)
    public void UnirsePartidaMultiplayer()
    {
        if (ControladorPersistente.Instance != null) 
            ControladorPersistente.Instance.esPartidaMultijugador = true;

        // Usamos la sintaxis limpia que tu Unity sí acepta sin errores
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[(int)indiceVillano].Prefab;
        
        NetworkManager.Singleton.StartClient();
    }

    // 4. SALIR DEL JUEGO
    public void SalirJuego()
    {
        Application.Quit();
    }
}
