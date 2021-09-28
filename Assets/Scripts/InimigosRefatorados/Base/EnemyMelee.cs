using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMelee : EntidadeBase
{
    [HideInInspector] public float proximoAtaque = 0;
    [Header("Configuração de ataque")]
    public float taxaAtaque = 1;
    public float distanciaProAtaque = 1.2f;
    public Transform posicaoArma;
    public float tamanhoAtaque = 0.5f;
    public float dano;
    public LayerMask hitMask;

    public virtual void Start()
    {
        enemyState = EnemyState.Patrolling;
        rb2d = GetComponent<Rigidbody2D>();
        spriteAnimacao = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        VerifyState();
        VerifyDistance();
        VerificarMorte();
    }

    public override void VerifyState()
    {
        spriteAnimacao.SetFloat("Horizontal", Mathf.Abs(transform.position.x));
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                Andar();
                break;
            case EnemyState.Attacking:
                Atacar();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Ponto A : Esquerda | Ponto B : Direita
    /// </summary>
    public override void Andar()
    {
        spriteAnimacao.SetBool("podeAtacar", false);

        Vector2 direction = Vector2.zero;
        float distance = Vector2.Distance(transform.position, pointB);
        if (distance <= 1.2f)
        {
            transform.localScale = new Vector3(-1, 1, 0);
            direction = Vector2.left;
            rb2d.velocity = direction * velocidade;
        }
        else if (distance >= 9.3f)
        {
            transform.localScale = new Vector3(1, 1, 0);
            direction = Vector2.right;
            rb2d.velocity = direction * velocidade;
        }


        if (rb2d.velocity == Vector2.zero)
        {
            float randomDirection = Random.Range(1, 2);
            if (randomDirection == 1)
                rb2d.velocity = Vector2.left * velocidade;
            else
                rb2d.velocity = Vector2.right * velocidade;
        }
    }


    public override void Atacar()
    {
       if(Target != null)
        {
            float distanceAttack = Vector2.Distance(Target.position, transform.position);

            Flip(Target.position);

            if (distanceAttack >= distanciaProAtaque)
            {
                Vector3 direction = new Vector3((Target.position.x - transform.position.x), rb2d.velocity.y, 0f);
                rb2d.velocity = direction * velocidade;
                spriteAnimacao.SetBool("podeAtacar", false);
                Debug.Log("não atacando");
            }
            else if (distanceAttack <= distanciaProAtaque)
            {            
               

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(posicaoArma.position, tamanhoAtaque, hitMask);
                //Passando por cada inimigo e aplicando dano e aplicando força
                foreach (Collider2D objeto in hitEnemies)
                {
                    if (!objeto.GetComponent<Personagem>().invulneravel && Time.time > proximoAtaque)
                    {
                        proximoAtaque = Time.time + taxaAtaque;
                        spriteAnimacao.SetBool("podeAtacar", true);
                        objeto.GetComponent<Personagem>().DarDano(dano);
                        objeto.GetComponent<Rigidbody2D>().AddForce(new Vector2(objeto.GetComponent<Personagem>().direcaoOlhar * -1, 0) * 3f, ForceMode2D.Impulse);
                    }                  
                }

            }
        }

    }


    private void OnDrawGizmos()
    {
        if (posicaoArma == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoArma.position, tamanhoAtaque);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointA, 0.6f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointB, 0.6f);
    }
    public virtual void VerifyDistance()
    {
        float distance = Vector3.Distance(Target.position, transform.position);
        if (distance <= transform.position.x)
            enemyState = EnemyState.Attacking;
        else if (distance >= transform.position.x)
            enemyState = EnemyState.Patrolling;
    }
}
