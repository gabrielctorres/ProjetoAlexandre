using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InimigoComum : MonoBehaviour
{
    float taxaAtaque = 1;
    float proximoAtaque = 0;

    private float velocidadeDoInimigo = 3;

    private Transform posicaoDoJogador;

    private float distancia;
    
    private static Rigidbody2D rb;

    protected Animator spriteAnimation;

    // Start is called before the first frame update
    public virtual void Start()
    {
        posicaoDoJogador = GameObject.FindGameObjectWithTag("Player").transform;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        SeguirJogador();        
    }

    void SeguirJogador()
    {
        if (posicaoDoJogador.gameObject != null)
        {
            distancia = Vector2.Distance(this.gameObject.transform.position, posicaoDoJogador.position);

            if (distancia >= 1.7f)
            {
                velocidadeDoInimigo = 3;
                this.transform.position = Vector2
                .MoveTowards(this.gameObject.transform.position, new Vector2(posicaoDoJogador.transform.position.x, posicaoDoJogador.transform.position.y), velocidadeDoInimigo * Time.deltaTime);
                spriteAnimation.SetBool("podeAndar", true);
                spriteAnimation.SetBool("podeAtacar", false);
            }
            else if(distancia <= 1.7f)
            {
                velocidadeDoInimigo = 0;
                AtacarJogador();                
            }
        }
    }

    void AtacarJogador()
    {
        if (Time.time > proximoAtaque)
        {
            proximoAtaque = Time.time + taxaAtaque;
            spriteAnimation.SetBool("podeAtacar", true);
        }
        else
            spriteAnimation.SetBool("podeAtacar", false);
    }    
}
