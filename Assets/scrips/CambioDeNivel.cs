using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeNivel : MonoBehaviour
{
    [Header("Configuración del Destino")]
    [SerializeField] private bool esElUltimoNivel = false;
    [SerializeField] private string nombreSiguienteNivel = "NIVEL2";
    [SerializeField] private string escenaGanaste = "Ganaste";

    private bool yaCambio = false;

    // SE CORRIGIÓ AQUÍ: Cambiado a OnCollisionEnter2D para que detecte el golpe físico
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (yaCambio) return;

        // Se cambió 'other' por 'collision.gameObject'
        if (collision.gameObject.CompareTag("Player"))
        {
            yaCambio = true;
            string destino = esElUltimoNivel ? escenaGanaste : nombreSiguienteNivel;
            Debug.Log("¡Jugador chocó con el cofre! Cargando: " + destino);
            SceneManager.LoadScene(destino);
        }
    }
}
