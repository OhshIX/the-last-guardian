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

        // REGRESAMOS A TU LÓGICA: Solo el objeto con Tag "Player" puede activar la meta
        if (collision.gameObject.CompareTag("Player"))
        {
            yaCambio = true;
            string destino = esElUltimoNivel ? escenaGanaste : nombreSiguienteNivel;
            Debug.Log("¡El Héroe chocó con el cofre! Cargando: " + destino);

            // --- LÓGICA MULTIJUGADOR OPTIMIZADA ---
            
            // Si el Controlador Persistente dice que es multijugador, viajamos por red
            if (ControladorPersistente.Instance != null && ControladorPersistente.Instance.esPartidaMultijugador)
            {
                // Netcode exige que SOLO el Servidor/Host ordene el cambio de escena
                if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
                {
                    // Netcode cambia la escena para TODOS los jugadores al mismo tiempo de forma segura
                    NetworkManager.Singleton.SceneManager.LoadScene(destino, LoadSceneMode.Single);
                }
            }
            else
            {
                // Si el jugador está jugando en modo Solitario, viaja de la forma clásica de Unity
                SceneManager.LoadScene(destino);
            }
        }
    }
}
