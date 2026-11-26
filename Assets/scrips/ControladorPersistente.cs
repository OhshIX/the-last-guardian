using UnityEngine;

public class ControladorPersistente : MonoBehaviour
{
    public static ControladorPersistente Instance;
    public bool esPartidaMultijugador = false;

    void Awake()
    {
        // Patron Singleton para que solo exista uno en todo el juego
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de nivel
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

