using Unity.Netcode;
using UnityEngine;

public class InicioRapido : MonoBehaviour
{
    // Esto creará de inmediato la casilla en el Inspector de Unity
    [Header("Referencias de Red")]
    public NetworkManager networkManager; 

    void Start()
    {
        // Lógica automática: Si arrastraste el NetworkManager, arranca el Host al dar Play
        if (networkManager != null)
        {
            networkManager.StartHost();
            Debug.Log("¡Inicio Rápido activado: Host iniciado automáticamente!");
        }
        else
        {
            // Si olvidaste arrastrarlo, el código intentará buscarlo solo para que no falle
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.StartHost();
            }
        }
    }
}
