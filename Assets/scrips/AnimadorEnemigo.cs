using UnityEngine;
using System.Collections;
public class AnimadorEnemigoViejo : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector3 posicionPrevia;
    private bool estaMuerto = false;
    private bool esInvulnerable = false;

    [Header("Configuración de Capas de Suelo")]
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float radioDeteccionSuelo = 0.2f;

    [Header("Combate y Ataque")]
    [SerializeField] private GameObject prefabBolaFuego; // Arrastra aquí el Prefab de la bola de fuego
    [SerializeField] private Transform puntoDisparo;     // Objeto vacío en la mano/boca del villano
    [SerializeField] private float velocidadBolaFuego = 5f;

    [Header("Salud del Villano (Jefe)")]
    [SerializeField] private int vidas = 3; // El jugador debe pisarlo 3 veces
    [SerializeField] private float fuerzaReboteJugador = 12f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        posicionPrevia = transform.position;
    }

    void Update()
    {
        if (estaMuerto || animator == null) return;

        // 1. CONTROLAR EL PARÁMETRO 'VELOCIDAD'
        float movimientoX = Mathf.Abs(transform.position.x - posicionPrevia.x) / Time.deltaTime;
        if (TieneParametro("VELOCIDAD", animator)) animator.SetFloat("VELOCIDAD", movimientoX);
        else if (TieneParametro("VELOCIDAD", animator)) animator.SetFloat("VELOCIDAD", movimientoX);

        // 2. CONTROLAR EL PARÁMETRO 'EN_SUELO'
        if (rb != null)
        {
            Vector2 posicionPies = new Vector2(transform.position.x, transform.position.y - 0.5f);
            bool tocandoSuelo = Physics2D.OverlapCircle(posicionPies, radioDeteccionSuelo, capaSuelo);

            if (TieneParametro("EN_SUELO", animator)) animator.SetBool("EN_SUELO", tocandoSuelo);
            else if (TieneParametro("EN_SUELO", animator)) animator.SetBool("EN_SUELO", tocandoSuelo);
        }

        posicionPrevia = transform.position;
    }

    // 3. DETECTAR SI EL PLAYER BRINCA SOBRE ÉL O SI ÉL LO ATACA FÍSICAMENTE
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (estaMuerto) return;

        if (other.gameObject.CompareTag("Player"))
        {
            // Conseguimos el punto de contacto para saber si nos pisó desde arriba
            foreach (ContactPoint2D contacto in other.contacts)
            {

                if (contacto.normal.y < -0.8f) 
                    {
                        RecibirDanio(other.gameObject);
                        return;
                    }

                
            }
        }

    }


    // =========================================================================
    // MECÁNICA: RECIBIR DAÑO AL SER PISADO 3 VECES
    // =========================================================================
       private void RecibirDanio(GameObject jugador)
    {
        // CANDADO: Si ya recibió daño en este instante, ignora el pisotón repetido
        if (esInvulnerable || estaMuerto) return;

        vidas--;
        StartCoroutine(TiempoInmunidadJefe()); // Activa el candado de tiempo

        // Hace que el jugador rebote hacia arriba de forma fluida al pisarlo
        Rigidbody2D rbJugador = jugador.GetComponent<Rigidbody2D>();
        if (rbJugador != null)
        {
            rbJugador.velocity = new Vector2(rbJugador.velocity.x, fuerzaReboteJugador);
        }

        // Si se queda sin vidas, muere por completo
        if (vidas <= 0)
        {
            MORIR(); // Recuerda verificar si tu método se llama MORIR o Morir
        }
        else
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null) StartCoroutine(EfectoGolpe(sprite));
        }
    }

    // Co-rutina que mantiene el candado activo durante medio segundo
    IEnumerator TiempoInmunidadJefe()
    {
        esInvulnerable = true;
        yield return new WaitForSeconds(0.5f); // Medio segundo de protección para el villano
        esInvulnerable = false;
    }

    IEnumerator EfectoGolpe(SpriteRenderer sprite)
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sprite.color = Color.white;
    }

    public void MORIR()
    {
        if (estaMuerto) return;
        estaMuerto = true;

        if (animator != null)
        {
            if (TieneParametro("MORIR", animator)) animator.SetTrigger("MORIR");
            else if (TieneParametro("MORIR", animator)) animator.SetTrigger("MORIR");
            else if (TieneParametro("DEAD", animator)) animator.SetTrigger("DEAD");
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        this.enabled = false;
    }

    private bool TieneParametro(string nombreParametro, Animator anim)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == nombreParametro) return true;
        }
        return false;
    }

        private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 posicionPies = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Gizmos.DrawWireSphere(posicionPies, radioDeteccionSuelo);
    }
    
}
