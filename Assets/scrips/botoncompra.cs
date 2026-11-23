using UnityEngine;
using UnityEngine.UI;

public class BotonCompra : MonoBehaviour
{
    [Header("Identificador único (ej. item_1, item_2, item_3)")]
    public string idCompra = "item_1";

    [Header("Imagen al comprar")]
    public Sprite spriteComprado;

    private Button boton;
    private Image imagen;
    private bool yaComprado = false;

    void Awake()
    {
        boton = GetComponent<Button>();
        imagen = GetComponent<Image>();

        // Al cargar la escena TIENDA, revisa si ya estaba comprado
        if (PlayerPrefs.GetInt("Comprado_" + idCompra, 0) == 1)
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

        PlayerPrefs.SetInt("Comprado_" + idCompra, 1);
        PlayerPrefs.Save();

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