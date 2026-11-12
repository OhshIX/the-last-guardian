using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Intro  : MonoBehaviour
{
    [System.Serializable]
    public class Diapositiva
    {
        public Sprite imagen;

        [TextArea(3, 5)]
        public string texto;
    }

    [Header("UI")]
    public Image imagenUI;
    public TextMeshProUGUI textoUI;

    [Header("Diapositivas")]
    public Diapositiva[] diapositivas;

    [Header("Configuración")]
    public float tiempoPorDiapositiva = 5f;

    [Header("Escena Final")]
    public bool cargarEscenaAlFinal = true;
    public string nombreEscena = "Menu";

    private int indiceActual = 0;

    void Start()
    {
        StartCoroutine(ReproducirIntro());
    }

    IEnumerator ReproducirIntro()
    {
        while (indiceActual < diapositivas.Length)
        {
            imagenUI.sprite = diapositivas[indiceActual].imagen;
            textoUI.text = diapositivas[indiceActual].texto;

            yield return new WaitForSeconds(tiempoPorDiapositiva);

            indiceActual++;
        }

        if (cargarEscenaAlFinal)
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}