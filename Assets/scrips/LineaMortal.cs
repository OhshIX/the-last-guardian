using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineaMortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si cae el Player uno
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LineaMortal();
        }
        // Si cae el Villano (Player 2)
        else if (other.CompareTag("Villano") || other.gameObject.name == "Villano")
        {
            GameManager.Instance.LineaMortal();
        }
    }
}
