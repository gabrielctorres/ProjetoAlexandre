using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InimigoComum : MonoBehaviour
{
    float taxaAtaque = 1;
    float proximoAtaque = 0;
    private int hp;

    private float velocidadeDoInimigo = 3;

    public Transform posicaoDoJogador;

    private float distancia;
    
    private static Rigidbody2D rb;
    private static bool unidadePodeAtacar;

    protected Animator spriteAnimation;

    protected SpriteRenderer sprite;

    BossFase1 bossFase1;

    // Start is called before the first frame update
    public virtual void Start()
    {
        posicaoDoJogador = GameObject.FindGameObjectWithTag("Player").transform;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteAnimation = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        AtribuirHp();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (posicaoDoJogador.transform.position.x > this.gameObject.transform.position.x && sprite.flipX || posicaoDoJogador.transform.position.x < this.gameObject.transform.position.x && !sprite.flipX)
        {
            Flip();
        }
    }

    public virtual void FixedUpdate()
    {
        if(this.gameObject.tag == "Guarda" || this.gameObject.tag == "Escorpiao")
        {
            SeguirJogador();
        }
    }

    public void SeguirJogador()
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

    public void Flip()
    {
        sprite.flipX = !sprite.flipX;
        velocidadeDoInimigo *= -1;
    }

    public void AtacarJogador()
    {
        if (Time.time > proximoAtaque)
        {
            proximoAtaque = Time.time + taxaAtaque;
            spriteAnimation.SetBool("podeAtacar", true);
        }
        else
            spriteAnimation.SetBool("podeAtacar", false);
    }    

    public void TomarDano (int danoJogador)
    {

        hp -= danoJogador;
        StartCoroutine(EfeitoDano());
        if (hp < 1)
        {
            Destroy(this.gameObject);
            bossFase1.hpBoss -= 1;
        }
    }
    
    IEnumerator EfeitoDano()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public void AtribuirHp()
    {
        if(this.gameObject.tag == "Marinheiro")
        {
            hp = 8;
        }
    }
}
