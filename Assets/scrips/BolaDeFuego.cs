using UnityEngine;

public class BolaDeFuego : MonoBehaviour
{
    [Header("Físicas del Proyectil")]
    public float velocidad = 15f;
    public float tiempoDeVida = 4f;
    
    // Al declarar la variable aquí arriba de forma global, se mantiene guardada para el FixedUpdate
    private Vector2 _direccion; 
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // Se destruye automáticamente tras pasar el tiempo de vida asignado
        Destroy(gameObject, tiempoDeVida); 
    }

    // El villano llama a esta función inmediatamente después de crear la bola de fuego
    public void ConfigurarDireccion(Vector2 nuevaDireccion)
    {
        _direccion = nuevaDireccion;
        
        // Rota la punta de la bola de fuego hacia donde va viajando en el plano 2D
        float angulo = Mathf.Atan2(nuevaDireccion.y, nuevaDireccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }

    void FixedUpdate()
    {
        // Aplica el empuje físico constante cuadro por cuadro utilizando la dirección recibida
        if (_rb != null)
        {
            _rb.velocity = _direccion * velocidad;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detecta el impacto con el Player
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PerderVida();
            Destroy(gameObject); 
        }
        // Detecta el impacto contra las paredes o el suelo
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}
