using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InimigoComum : MonoBehaviour
{
    float taxaAtaque = 1;
    float proximoAtaque = 0;
    public float hp;
   
    public bool podeSeguir;

    private float velocidadeDoInimigo = 3;

    public Transform posicaoDoJogador;

    private float distancia;
    
    private static Rigidbody2D rb;
    private static bool unidadePodeAtacar;

    protected Animator spriteAnimation;    

    protected SpriteRenderer sprite;
    public int direcaoOlhar = -1;
    public BossFase1 bossFase1;

    // Start is called before the first frame update
    public virtual void Start()
    {
        posicaoDoJogador = GameObject.Find("PersonagemOriginal").GetComponent<Transform>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteAnimation = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();        

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(spriteAnimation != null)
        {
            spriteAnimation = GetComponent<Animator>();
        }

        if (posicaoDoJogador.transform.position.x > this.gameObject.transform.position.x && sprite.flipX || posicaoDoJogador.transform.position.x < this.gameObject.transform.position.x && !sprite.flipX)
        {
            Flip();
        }

    }

    public virtual void FixedUpdate()
    {
        if(this.gameObject.tag == "Guarda" || this.gameObject.tag == "Escorpiao" || this.gameObject.tag == "Marinheiro" && podeSeguir)
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
                velocidadeDoInimigo = 2.5f;
                this.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, new Vector2(posicaoDoJogador.transform.position.x, posicaoDoJogador.transform.position.y), velocidadeDoInimigo * Time.deltaTime);
                spriteAnimation.SetBool("podeAndar", true);
                spriteAnimation.SetBool("podeAtacar", false);                
            }
            else if(distancia <= 1.7f)
            {
                spriteAnimation.SetBool("podeAndar", false);
                velocidadeDoInimigo = 0;
                this.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, new Vector2(posicaoDoJogador.transform.position.x, posicaoDoJogador.transform.position.y), velocidadeDoInimigo * Time.deltaTime);
                AtacarJogador();      }
        }
    }

    public void Flip()
    {
        sprite.flipX = !sprite.flipX;
        direcaoOlhar *= -1;
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
        {
            spriteAnimation.SetBool("podeAtacar", false);            
        }              
    }    

    public void TomarDano (float danoJogador)
    {
        hp -= danoJogador;
        StartCoroutine(EfeitoDano());
        if (hp < 1)
        {
            bossFase1.inimigos.RemoveAt(bossFase1.inimigos.IndexOf(this.gameObject));
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
}
