using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoComumVoador : MonoBehaviour
{
    public float hp;
    public float dano;
    public bool podeSeguir;
    public bool estaSeMovendo;
    public float velocidadeDoInimigo = 3;
    public Transform posicaoDoJogador;
    private float distancia;
    public float distanciaParaAtacar = 6.5f;
    private static Rigidbody2D rb;
    protected Animator spriteAnimation;
    protected SpriteRenderer sprite;
    public int direcaoOlhar = -1;

    public Personagem personagem;

    // Start is called before the first frame update
    public virtual void Start()
    {
        posicaoDoJogador = GameObject.Find("Personagem").GetComponent<Transform>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteAnimation = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        personagem = GameObject.Find("Personagem").GetComponent<Personagem>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        distancia = DistanciaJogador();
        estaSeMovendo = (distancia <= distanciaParaAtacar);

        if (estaSeMovendo)
        {
            if(posicaoDoJogador.position.x>transform.position.x && sprite.flipX || (posicaoDoJogador.position.x < transform.position.x && !sprite.flipX)){
                Flip();
            }
        }
    }

    public virtual void FixedUpdate()
    {

    }

    protected float DistanciaJogador()
    {
        return Vector2.Distance(posicaoDoJogador.position, transform.position);
    }

    protected void Flip()
    {
        sprite.flipX = !sprite.flipX;
        direcaoOlhar *= -1;
        velocidadeDoInimigo *= -1;
    }    
}
