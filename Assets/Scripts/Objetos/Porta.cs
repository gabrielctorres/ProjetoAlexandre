using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    bool podeEntrar;
    public Transform playerTransform;
    public Transform posicaoSaida;

    public GameObject botaoInteracao;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && podeEntrar)
        {
            playerTransform.position = posicaoSaida.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            podeEntrar = true;
            botaoInteracao.SetActive(true);
            //positionAux = collision.transform;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() != null)
        {
            podeEntrar = false;
            botaoInteracao.SetActive(false);           
        }
    }
}
