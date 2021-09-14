using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMelee : EntidadeBase
{
    [HideInInspector] public float proximoAtaque = 0;
    [Header("Configuração de ataque")]
    public float taxaAtaque = 1;    
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
        if (transform.position.x >= pointB.x && transform.position.x >= pointA.x)
        {
            rb2d.velocity = Vector2.left * velocidade;
            Flip(pointB);
        }
        
        if (transform.position.x <= pointA.x && transform.position.x <= pointB.x)
        {
            rb2d.velocity = Vector2.right * velocidade;
            Flip(pointA);
        }
    }



    public override void Atacar()
    {
        if (Target != null) return;

        float distanceAttack = Vector2.Distance(Target.position, transform.position);

       

        if(distanceAttack >= 1.2f)
        {
            Vector3 direction = new Vector3((Target.position.x - transform.position.x), 0f, 0f);
            rb2d.velocity = direction * velocidade;
            Flip(Target.position);
            Debug.Log("não atacando");

        }           
        else if(distanceAttack <= 1.2f)
        {
            rb2d.velocity = Vector2.zero;
            Flip(Target.position);

            if (Time.time > proximoAtaque)
            {
                proximoAtaque = Time.time + taxaAtaque;
                spriteAnimacao.SetBool("podeAtacar", true);
                Debug.Log("atacando");
                CreateCollision();
            }else
                spriteAnimacao.SetBool("podeAtacar", false);
        }

    }

    public void CreateCollision()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(posicaoArma.position, tamanhoAtaque, hitMask);

        //Passando por cada inimigo e aplicando dano e aplicando força
        foreach (Collider2D objeto in hitEnemies)
        {
            if (!objeto.GetComponent<Personagem>().invulneravel)
            {
                objeto.GetComponent<Personagem>().DarDano(dano);
                objeto.GetComponent<Rigidbody2D>().AddForce(new Vector2(objeto.GetComponent<Personagem>().direcaoOlhar * -1, 0) * 3f, ForceMode2D.Impulse);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (posicaoArma == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoArma.position, tamanhoAtaque);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radiusView);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointA, 0.6f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointB, 0.6f);
    }
    public virtual void VerifyDistance()
    {
        float distance = Vector3.Distance(Target.position, transform.position);
        if (distance <= radiusView)
            enemyState = EnemyState.Attacking;
        else if (distance >= radiusView)
            enemyState = EnemyState.Patrolling;
    }
}
