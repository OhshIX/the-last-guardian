using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeNivel : MonoBehaviour
{
    [Header("Configuración del Destino")]
    [SerializeField] private bool esElUltimoNivel = false;
    [SerializeField] private string nombreSiguienteNivel = "NIVEL2";
    [SerializeField] private string escenaGanaste = "Ganaste";

    private bool yaCambio = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (yaCambio) return;

        if (other.CompareTag("Player"))
        {
            yaCambio = true;
            string destino = esElUltimoNivel ? escenaGanaste : nombreSiguienteNivel;
            Debug.Log("¡Jugador entró en CambioNivel! Cargando: " + destino);
            SceneManager.LoadScene(destino);
        }
    }
}