using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColision : MonoBehaviour
{
    public bool estaNoChao = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chao"))
            estaNoChao = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Chao"))
            estaNoChao = false;
    }
}
