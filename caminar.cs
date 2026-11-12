using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caminar : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 10f;

    [Header("Detector de suelo")]
    public public Transform puntoSuelo;
    public float distanciaSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private float movimiento;
    private bool enSuelo;

    // Escala original
    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Guardar escala inicial
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        // Movimiento horizontal
        movimiento = Input.GetAxisRaw("Horizontal");
    
        // Detectar suelo
        enSuelo = Physics2D.Raycast(
            puntoSuelo.position,
            Vector2.down,
            distanciaSuelo,
            capaSuelo
        );

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                fuerzaSalto
            );
        }

        // Voltear personaje sin deformar escala
        if (movimiento > 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(escalaOriginal.x),
                escalaOriginal.y,
                escalaOriginal.z
            );
        }
        else if (movimiento < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(escalaOriginal.x),
                escalaOriginal.y,
                escalaOriginal.z
            );
        }
    }  
     
    void FixedUpdate()
    {
        rb.velocity = new Vector2(
            movimiento * velocidad,
            rb.velocity.y
        );
    }

    // Dibujar Raycast
    void OnDrawGizmos()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(
                puntoSuelo.position,
                puntoSuelo.position + Vector3.down * distanciaSuelo
                );
        }   
    }
}
