using UnityEngine;
using Unity.Netcode;
using Cinemachine; // Cargamos el sistema de cámaras de Unity

public class SeguimientoCamaraNet : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // Solo si este clon es mi propio personaje (el jugador local)
        if (IsOwner)
        {
            // Buscamos la cámara virtual de Cinemachine en la escena
            CinemachineVirtualCamera camaraVirtual = FindObjectOfType<CinemachineVirtualCamera>();

            if (camaraVirtual != null)
            {
                // Le decimos a la cámara que nos siga y nos mire a nosotros
                camaraVirtual.Follow = transform;
                camaraVirtual.LookAt = transform;
            }
        }
    }
}