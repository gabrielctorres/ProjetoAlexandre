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

    [Header("Configuração de ataque")]
    public Transform posicaoArma;
    public float tamanhoAtaque = 0.5f;
    public float dano;
    public LayerMask hitMask;


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
        posicaoDoJogador = GameObject.Find("Personagem").GetComponent<Transform>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteAnimation = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();        

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
        if(this.gameObject.tag == "Guarda" || this.gameObject.tag == "Escorpiao" || this.gameObject.tag == "Marinheiro" && podeSeguir)
        {
            SeguirJogador();
        }        
    }

    public void SeguirJogador()
    {
        if(gameObject.tag != "Escorpiao")   spriteAnimation.SetFloat("Horizontal", Mathf.Abs(transform.position.x));
        if (posicaoDoJogador.gameObject != null && (posicaoDoJogador.transform.position.y-transform.position.y)<3)
        {
            distancia = Vector2.Distance(this.gameObject.transform.position, posicaoDoJogador.position);            
            
            if (distancia >= 1.7f && distancia<8f)
            {
                velocidadeDoInimigo = 2.5f;
                this.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, new Vector2(posicaoDoJogador.transform.position.x, this.transform.position.y), velocidadeDoInimigo * Time.deltaTime);
                if (gameObject.tag != "Escorpiao") spriteAnimation.SetBool("podeAtacar", false);                
            }
            else if(distancia <= 1.7f)
            {           
                velocidadeDoInimigo = 0;                
                AtacarJogador();
            }
            else if (distancia > 8f)
            {
                if (gameObject.tag != "Escorpiao") spriteAnimation.SetBool("podeAndar", false);
                velocidadeDoInimigo = 0;
            }
        }
        else if(posicaoDoJogador.gameObject != null && posicaoDoJogador.position.y > 0f)
        {
            if (this.gameObject.transform.position.y > 0f)
            {
                distancia = Vector2.Distance(this.gameObject.transform.position, posicaoDoJogador.position);

                if (distancia >= 1.7f && distancia < 8f)
                {
                    velocidadeDoInimigo = 2.5f;
                    this.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, new Vector2(posicaoDoJogador.transform.position.x, this.transform.position.y), velocidadeDoInimigo * Time.deltaTime);
                    spriteAnimation.SetBool("podeAndar", true);
                    spriteAnimation.SetBool("podeAtacar", false);
                }
                else if (distancia <= 1.7f)
                {
                    spriteAnimation.SetBool("podeAndar", false);
                    velocidadeDoInimigo = 0;
                    AtacarJogador();
                }
                else if (distancia > 8f)
                {
                    spriteAnimation.SetBool("podeAndar", false);
                    velocidadeDoInimigo = 0;
                }
            }
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
            //Guardando cada inimigo dependendo da layer que a adaga colidiu
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(posicaoArma.position, tamanhoAtaque, hitMask);
            
            //Passando por casa inimigo e aplicando dano e aplicando força
            foreach (Collider2D objeto in hitEnemies)
            {
                if (!objeto.GetComponent<Personagem>().invulneravel)
                {
                    objeto.GetComponent<Personagem>().DarDano(dano);
                    objeto.GetComponent<Rigidbody2D>().AddForce(new Vector2(objeto.GetComponent<Personagem>().direcaoOlhar * -1, 0) * 3f, ForceMode2D.Impulse);
                }
            }           
        }

        else
        {
            spriteAnimation.SetBool("podeAtacar", false);            
        }              
    }

    private void OnDrawGizmos()
    {
        if (posicaoArma == null)
            return;

        Gizmos.DrawWireSphere(posicaoArma.position, tamanhoAtaque);
    }

    public void TomarDano (float danoJogador)
    {
        hp -= danoJogador;
        StartCoroutine(EfeitoDano());
        if (hp < 1)
        {
            if(bossFase1 != null)
            {
                bossFase1.hpBoss -= 1;
                bossFase1.inimigos.RemoveAt(bossFase1.inimigos.IndexOf(this.gameObject));
            }
                Destroy(this.gameObject);
        }
    }
    
    IEnumerator EfeitoDano()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sprite.color = Color.white;        
    }       
}
