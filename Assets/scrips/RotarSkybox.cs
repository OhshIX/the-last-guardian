using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarSkybox : MonoBehaviour
{
    public Transform camara;
    public float velocidadRotacion = 2f;

    void Update()
    {
        if (camara != null)
        {
            // Rota el Skybox según la posición X de la cámara
            float rotacion = camara.position.x * velocidadRotacion;
            RenderSettings.skybox.SetFloat("_Rotation", rotacion);
            }    
    }
}
