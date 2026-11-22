using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caminar : MonoBehaviour
{
    [Header("Configuración de Multijugador")]
    public bool esJugadorDos = false; // Falso para Player, Verdadero para Villano

    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;

    [Header("Salto")]
    public float fuerzaSalto = 10f;
    public AudioClip Salto;

    [Header("Detector de suelo")]
    public Transform puntoSuelo;
    public float distanciaSuelo = 0.2f;
    public LayerMask capaSuelo;

    [HideInInspector]
    public bool esInmune = false;

    private Rigidbody2D rb;
    private float movimiento;
    private bool enSuelo;
    private Animator animator;
    private bool corriendo;
    private Vector3 escalaOriginal;
    private bool estaMuerto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Restaurado: Busca el Animator en el propio objeto 2D como lo tenías antes
        animator = GetComponent<Animator>();
        
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        // ==========================================
// ACCIÓN DE ATAQUE PARA EL MULTIJUGADOR
// ==========================================
// El Player 1 podría atacar con el clic o la tecla 'F', el Villano (Jugador 2) atacará con la tecla 
// =======================================================
// ACCIÓN DE ATAQUE EXCLUSIVA PARA EL JUGADOR 2 (VILLANO)
// =======================================================
    if (esJugadorDos)
    {
    // El villano ataca al presionar la tecla P
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (animator != null)
            {
                if (TieneParametro("ATAQUE", animator)) 
            {
                animator.SetTrigger("ATAQUE");
            }
            }
        }
    }



        // Si el jugador ya murió, bloqueamos por completo el control
        if (estaMuerto) return;
        movimiento=0f;

        // SEPARACIÓN DE CONTROLES LIMPIA
        if (!esJugadorDos)
        {
            // JUGADOR 1 (PLAYER): Se mueve ÚNICAMENTE con letras A y D
            if (Input.GetKey(KeyCode.A)) movimiento = -1f;
            if (Input.GetKey(KeyCode.D)) movimiento = 1f;

            corriendo = Input.GetKey(KeyCode.LeftShift);
        }
        else
        {
            // JUGADOR 2 (VILLANO: Usa flechas laterales y Shift Derecho)
            movimiento = 0f;
            if (Input.GetKey(KeyCode.LeftArrow)) movimiento = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) movimiento = 1f;

            corriendo = Input.GetKey(KeyCode.RightShift);
        }

        // DETECTOR DE SUELO (Tu lógica original)
        if (puntoSuelo != null)
        {
            enSuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        }

        // ACCIÓN DE SALTO SEPARADA
        // Player 1 salta con Espacio | Villano salta con Flecha Arriba
        bool botonSalto = !esJugadorDos ? Input.GetKeyDown(KeyCode.Space) : Input.GetKeyDown(KeyCode.UpArrow);

        if (botonSalto && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

            if (AudioManager.Instance != null && Salto != null)
            {
                AudioManager.Instance.ReproducirSonido(Salto);
            }

            if (animator != null)
            {
                if (TieneParametro("SALTAR", animator)) animator.SetTrigger("SALTAR");
            }
        }

        // ENVIAR PARÁMETROS AL ANIMATOR (Tu lógica original intacta)
        if (animator != null)
        {
            float velocidadForzada = Mathf.Abs(movimiento);

            if (TieneParametro("VELOCIDAD", animator)) animator.SetFloat("VELOCIDAD", velocidadForzada);
            if (TieneParametro("EN_SUELO", animator)) animator.SetBool("EN_SUELO", enSuelo);
            if (TieneParametro("CORRER", animator)) animator.SetBool("CORRER", corriendo);
        }

        // VOLTEAR PERSONAJE (Tu volteo original por escala intacto)
        if (movimiento > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
        else if (movimiento < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
    }

    void FixedUpdate(){
    // SOLUCIÓN DEFINITIVA: Si el personaje ya está muerto o el script se deshabilitó,
    // frenamos en seco cualquier intento de fijar velocidad física en el Rigidbody.
    if (estaMuerto) return;

    float velocidadActual = corriendo ? velocidadCorrer : velocidad;
    rb.velocity = new Vector2(movimiento * velocidadActual, rb.velocity.y);
    }


    public void Morir()
{
    // CANDADO DE SEGURIDAD: Evita que el Villano muera por error
    if (esJugadorDos) return; 

    if (estaMuerto) return;
    estaMuerto = true;

    if (animator != null && TieneParametro("MORIR", animator))
    {
        animator.SetTrigger("MORIR");
    }

    if (rb != null)
    {
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}


    private bool TieneParametro(string nombreParametro, Animator anim)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == nombreParametro) return true;
        }
        return false;
    }

 


    void OnDrawGizmos()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(puntoSuelo.position, puntoSuelo.position + Vector3.down * distanciaSuelo);
        }
    }

    public void ActivarVelocidadBoost(float nuevaVelocidad, float duracion)
    {
        if (estaMuerto) return;
        StopCoroutine("PotenciarVelocidad");
        StartCoroutine(PotenciarVelocidad(nuevaVelocidad, duracion));
    }

    IEnumerator PotenciarVelocidad(float nuevaVelocidad, float duracion)
    {
        float vOriginal = velocidad;
        float vCorrerOriginal = velocidadCorrer;
        velocidad = nuevaVelocidad;
        velocidadCorrer = nuevaVelocidad * 1.5f;
        yield return new WaitForSeconds(duracion);
        velocidad = vOriginal;
        velocidadCorrer = vCorrerOriginal;
    }

    public void ActivarSaltoPotenciado(float nuevoSalto, float duracion)
    {
        if (estaMuerto) return;
        StopCoroutine("PotenciarSalto");
        StartCoroutine(PotenciarSalto(nuevoSalto, duracion));
    }

    IEnumerator PotenciarSalto(float nuevoSalto, float duracion)
    {
        float saltoOriginal = fuerzaSalto;
        fuerzaSalto = nuevoSalto;
        yield return new WaitForSeconds(duracion);
        fuerzaSalto = saltoOriginal;
    }

    public void ActivarInmunidad(float duracion)
    {
        if (estaMuerto) return;
        StopCoroutine("PotenciarInmunidad");
        StartCoroutine(PotenciarInmunidad(duracion));
    }

    IEnumerator PotenciarInmunidad(float duracion)
    {
        esInmune = true;
        int capaOriginal = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Fantasma");
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) sprite.color = new Color(0f, 0.8f, 0.8f, 0.6f);
        yield return new WaitForSeconds(duracion);
        esInmune = false;
        gameObject.layer = capaOriginal;
        if (sprite != null) sprite.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    // Si caemos desde arriba sobre el objeto llamado villano
        if (collision.gameObject.name == "villano" || collision.gameObject.CompareTag("Villano"))
        {
            foreach (ContactPoint2D punto in collision.contacts)
            {
                if (punto.normal.y > 0.5f) // Verifica que el golpe venga desde arriba
                {
                    VillanoAtaqueFuego ataque = collision.gameObject.GetComponent<VillanoAtaqueFuego>();
                if (ataque != null)
                    {
                    ataque.RecibirPisoton(); // Llama a la resta de vida
                    
                    // Hace que el Player rebote al dar el pisotón
                    if (rb != null) rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto * 0.7f);
                    break;
                    }
                }
            }
        }
    }
}
