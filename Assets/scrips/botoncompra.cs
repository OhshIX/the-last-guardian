using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonCompra : MonoBehaviour
{
    [Header("Identificador único (ej. item_1, item_2, item_3)")]
    public string idCompra = "item_1";

    [Header("Imagen al comprar")]
    public Sprite spriteComprado;

    private static HashSet<string> comprasEnEstaPartida = new HashSet<string>();
    private static bool sesionInicializada = false;

    private Button boton;
    private Image imagen;
    private bool yaComprado = false;

   [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void PrepararSesionLimpia()
    {
        comprasEnEstaPartida.Clear();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        sesionInicializada = true;
    }

    void Awake()
    {
        if (!sesionInicializada)
        {
            comprasEnEstaPartida.Clear();
            sesionInicializada = true;
        }

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

    public static bool EstaComprado(string id)
    {
        return comprasEnEstaPartida.Contains(id);
    }

    void OnApplicationQuit()
    {
        comprasEnEstaPartida.Clear();
    }
}