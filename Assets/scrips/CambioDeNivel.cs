using UnityEngine;
using UnityEngine.SceneManagement; // <-- SE AGREGÓ ESTA LÍNEA

public class CambioDeNivel : MonoBehaviour
{
    [Header("Configuración del Destino")]
    [SerializeField] private bool esElUltimoNivel = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡Cofre tocado físicamente! Cargando Escena Ganaste.");
            
            // LA PARTE QUE CAMBIÓ: Ahora carga la escena directamente sin usar el GameManager
            SceneManager.LoadScene("Ganaste"); 
        }
    }
}
