using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caminar : MonoBehaviour
{
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
        animator = GetComponent<Animator>();
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        // Si el jugador ya murió, bloqueamos por completo el control
        if (estaMuerto) return;

        movimiento = Input.GetAxisRaw("Horizontal");
        corriendo = Input.GetKey(KeyCode.LeftShift);

        // DETECTOR DE SUELO
        if (puntoSuelo != null)
        {
            enSuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        }

        // ACCIÓN DE SALTO
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

            if (AudioManager.Instance != null && Salto != null)
            {
                AudioManager.Instance.ReproducirSonido(Salto);
            }

            if (animator != null)
            {
                if (TieneParametro("SALTAR", animator)) animator.SetTrigger("SALTAR");
                else if (TieneParametro("SALTAR", animator)) animator.SetTrigger("SALTAR");
            }
        }

        // ENVIAR PARÁMETROS AL ANIMATOR DE FORMA SEGURA
        if (animator != null)
         
        {
            // Tomamos el valor directo de la tecla presionada (movimiento es 1 o -1)
            // Usamos Mathf.Abs para que siempre sea positivo (así da 1 al caminar)
            float velocidadForzada = Mathf.Abs(movimiento);

            // Intentamos enviar tanto en mayúsculas como en minúsculas por si acaso
            if (TieneParametro("VELOCIDAD", animator)) animator.SetFloat("VELOCIDAD", velocidadForzada);
            else if (TieneParametro("VELOCIDAD", animator)) animator.SetFloat("VELOCIDAD", velocidadForzada);

            if (TieneParametro("EN_SUELO", animator)) animator.SetBool("EN_SUELO", enSuelo);
            else if (TieneParametro("EN_SUELO", animator)) animator.SetBool("EN_SUELO", enSuelo);

            if (TieneParametro("CORRER", animator)) animator.SetBool("CORRER", corriendo);
            else if (TieneParametro("CORRER", animator)) animator.SetBool("CORRER", corriendo);
        }

        // Voltear personaje
        if (movimiento > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
        else if (movimiento < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
    }

    void FixedUpdate()
    {
        if (estaMuerto) return;

        float velocidadActual = corriendo ? velocidadCorrer : velocidad;
        rb.velocity = new Vector2(movimiento * velocidadActual, rb.velocity.y);
    }

    // ==========================================
    // MÉTODO DE MUERTE (Llamar desde script de daño)
    // ==========================================
    public void Morir()
    {
        if (estaMuerto) return; // Evita que se ejecute más de una vez

        estaMuerto = true;

        if (animator != null)
        {
            if (TieneParametro("MORIR", animator)) animator.SetTrigger("MORIR");
            else if (TieneParametro("MORIR", animator)) animator.SetTrigger("MORIR");
        }

        // Detener físicamente al personaje y desactivar fuerzas gravitacionales complejas
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    // Validador inteligente de parámetros
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

    // ==========================================
    // POWER-UPS / POTENCIADORES
    // ==========================================
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
}
