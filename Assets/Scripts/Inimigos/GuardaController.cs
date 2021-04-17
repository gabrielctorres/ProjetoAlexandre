using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardaController : MonoBehaviour
{
    private Transform posicaoDoJogador;
    public GameObject guarda;

    // Start is called before the first frame update
    void Start()
    {
        posicaoDoJogador = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    // Update is called once per frame
    void Update()
    {
        if (posicaoDoJogador.position.x >= 79f)
        {
            guarda.SetActive(true);
        }
    }
}
