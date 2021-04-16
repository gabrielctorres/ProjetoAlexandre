using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public Transform posicaoSaida;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            collision.transform.position = posicaoSaida.position;
        }
    }
}
