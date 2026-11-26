using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode; // Agregamos la librería de Netcode para que reconozca el multijugador

public class CambioDeNivel : MonoBehaviour
{
    [Header("Configuración del Destino")]
    [SerializeField] private bool esElUltimoNivel = false;
    [SerializeField] private string nombreSiguienteNivel = "NIVEL2";
    [SerializeField] private string escenaGanaste = "Ganaste";

    private bool yaCambio = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (yaCambio) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // OBTENEMOS EL COMPONENTE DE RED DEL JUGADOR QUE CHOCÓ
            NetworkObject netObject = collision.gameObject.GetComponent<NetworkObject>();

            if (netObject != null)
            {
                yaCambio = true;
                string destino = esElUltimoNivel ? escenaGanaste : nombreSiguienteNivel;
                Debug.Log("¡Jugador chocó con el cofre en Red! Cargando: " + destino);

                // --- AQUÍ SE AGREGÓ LA LÓGICA MULTIJUGADOR ---
                // Validamos si la red está activa y si somos el Servidor/Host
                if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
                {
                    // Netcode cambia la escena para TODOS los jugadores al mismo tiempo de forma segura
                    NetworkManager.Singleton.SceneManager.LoadScene(destino, LoadSceneMode.Single);
                }
                else if (NetworkManager.Singleton == null)
                {
                    // Si por alguna razón estás probando el juego sin red (modo un solo jugador), usa el método clásico
                    SceneManager.LoadScene(destino);
                }
            }
        }
    }
}