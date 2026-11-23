using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonCompra : MonoBehaviour
{
    [Header("Identificador único (ej. item_1, item_2, item_3)")]
    public string idCompra = "item_1";

    [Header("Imagen al comprar")]
    public Sprite spriteComprado;

    // Guarda las compras SOLO mientras el juego esté abierto (se reinicia al cerrar o quitar Play)
    private static HashSet<string> comprasEnEstaPartida = new HashSet<string>();

    private Button boton;
    private Image imagen;
    private bool yaComprado = false;

    void Awake()
    {
        boton = GetComponent<Button>();
        imagen = GetComponent<Image>();

        if (comprasEnEstaPartida.Contains(idCompra))
        {
            AplicarEstadoComprado();
        }
    }

    void Start()
    {
        if (boton != null && !yaComprado)
        {
            boton.onClick.AddListener(MarcarComoComprado);
        }
    }

    public void MarcarComoComprado()
    {
        if (yaComprado) return;

        comprasEnEstaPartida.Add(idCompra);
        AplicarEstadoComprado();
    }

    private void AplicarEstadoComprado()
    {
        yaComprado = true;

        if (imagen != null && spriteComprado != null)
        {
            imagen.sprite = spriteComprado;
        }

        if (boton != null)
        {
            boton.interactable = false;
        }
    }
}