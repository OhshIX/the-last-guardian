using UnityEngine;
using UnityEngine.SceneManagement;

public class iralmenu : MonoBehaviour
{
    public string nombreEscenaMenu = "MENU";

    public void CargarMenu()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}