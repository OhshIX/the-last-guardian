using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class caminar : MonoBehaviour
{
    [Header("Movimiento Caballero")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;

    [Header("Salto Caballero")]
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
        if (estaMuerto) return;
        movimiento = 0f;

        // CONTROLES EXCLUSIVOS DEL CABALLERO: Letras A y D
        if (Input.GetKey(KeyCode.A)) movimiento = -1f;
        if (Input.GetKey(KeyCode.D)) movimiento = 1f;

        corriendo = Input.GetKey(KeyCode.LeftShift);

        // DETECTOR DE SUELO
        if (puntoSuelo != null)
        {
            enSuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        }

        // ACCIÓN DE SALTO: Letra W
        if (Input.GetKeyDown(KeyCode.W) && enSuelo)
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

        // ENVIAR PARÁMETROS AL ANIMATOR
        if (animator != null)
        {
            float velocidadForzada = Mathf.Abs(movimiento);

            if (TieneParametro("VELOCIDAD", animator)) animator.SetFloat("VELOCIDAD", velocidadForzada);
            if (TieneParametro("EN_SUELO", animator)) animator.SetBool("EN_SUELO", enSuelo);
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
        if (estaMuerto) return;

        float velocidadActual = corriendo ? velocidadCorrer : velocidad;
        rb.velocity = new Vector2(movimiento * velocidadActual, rb.velocity.y);
    }

    public void Morir()
    {
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
        // Lógica intacta para dañar al villano mediante un pisotón desde arriba
        if (collision.gameObject.name == "villano" || collision.gameObject.CompareTag("Villano"))
        {
            foreach (ContactPoint2D punto in collision.contacts)
            {
                if (punto.normal.y > 0.5f)
                {
                    VillanoAtaqueFuego ataque = collision.gameObject.GetComponent<VillanoAtaqueFuego>();
                    if (ataque != null)
                    {
                        ataque.RecibirPisoton();
                    }
                }
            }
        }
    }
}
