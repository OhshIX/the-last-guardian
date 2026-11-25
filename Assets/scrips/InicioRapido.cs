using Unity.Netcode;
using UnityEngine;

public class InicioRapido : MonoBehaviour
{
    void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            Debug.LogError("Falta poner el objeto NetworkManager en esta escena.");
        }
    }
}
