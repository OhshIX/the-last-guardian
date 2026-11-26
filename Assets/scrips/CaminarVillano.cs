using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // IMPORTANTE: Agregamos la librería de Netcode para que funcione IsOwner

// Cambiamos MonoBehaviour por NetworkBehaviour para habilitar las funciones de red
public class CaminarVillano : NetworkBehaviour 
{
    [Header("Movimiento Villano")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;

    [Header("Salto Villano")]
    public float fuerzaSalto = 10f;
    public AudioClip Salto;

    [Header("Detector de suelo")]
    public Transform puntoSuelo;
    public float distanciaSuelo = 0.2f;
    public LayerMask capaSuelo;

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
        // 1. FILTRO DE RED: Si este clon no me pertenece, no ejecuto nada de lo que está abajo
        if (!IsOwner) return; 

        if (estaMuerto) return;

        // ACCIÓN DE ATAQUE EXCLUSIVA PARA EL VILLANO
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

        movimiento = 0f;

        // CONTROLES EXCLUSIVOS DEL VILLANO: Flechas laterales y Shift Derecho
        if (Input.GetKey(KeyCode.LeftArrow)) movimiento = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) movimiento = 1f;

        corriendo = Input.GetKey(KeyCode.RightShift);

        // DETECTOR DE SUELO
        if (puntoSuelo != null)
        {
            enSuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        }

        // ACCIÓN DE SALTO: Flecha Arriba
        if (Input.GetKeyDown(KeyCode.UpArrow) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

            if (AudioManager.Instance != null && Salto != null)
            {
                AudioManager.Instance.ReproducirSonido(Salto);
            }

            if (animator != null)
            {
                if (TieneParametro("ENSUELO", animator)) animator.SetTrigger("ENSUELO");
            }
        }

        // ENVIAR PARÁMETROS AL ANIMATOR
        if (animator != null)
        {
            float velocidadForzada = Mathf.Abs(movimiento);

            if (TieneParametro("VELOCIDAD", animator)) animator.SetFloat("VELOCIDAD", velocidadForzada);
            if (TieneParametro("ENSUELO", animator)) animator.SetBool("ENSUELO", enSuelo);
            if (TieneParametro("CORRER", animator)) animator.SetBool("CORRER", corriendo);
        }

        // VOLTEAR PERSONAJE POR ESCALA
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
        // 2. FILTRO DE RED PARA FISICAS: Si no soy el dueño, no altero sus físicas locales
        if (!IsOwner || estaMuerto) return;

        float velocidadActual = corriendo ? velocidadCorrer : velocidad;
        rb.velocity = new Vector2(movimiento * velocidadActual, rb.velocity.y);
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
}

