using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InimigoComum : MonoBehaviour
{
    private float velocidadeDoInimigo = 3;

    private Transform posicaoDoJogador;

    private float distancia;
    // Start is called before the first frame update
    public virtual void Start()
    {
        posicaoDoJogador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        seguirJogador();
    }

    void seguirJogador()
    {
        if (posicaoDoJogador.gameObject != null)
        {
            distancia = Vector2.Distance(this.gameObject.transform.position, posicaoDoJogador.position);

            if (distancia >= 2)
            {
                velocidadeDoInimigo = 3;
                this.transform.position = Vector2
                .MoveTowards(this.gameObject.transform.position, new Vector2(posicaoDoJogador.transform.position.x, posicaoDoJogador.transform.position.y), velocidadeDoInimigo * Time.deltaTime);
            }
            else if(distancia <= 2)
            {
                velocidadeDoInimigo = 0;
            }            
        }
    }
}
