using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EntidadeBase
{
    [HideInInspector] public float proximoAtaque = 0;
    [Header("Configuração de ataque")]
    public float taxaAtaque = 1;
    public float dano;

    public float distanceMax = 7f;

    public GameObject prefabProjetil;


    [HideInInspector] public Vector2 oldVelocity;
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


    /// <summary>
    /// Ponto A : Esquerda | Ponto B : Direita
    /// </summary>
    public override void Andar()
    {
        spriteAnimacao.SetBool("podeAtacar", false);

        Vector2 direction = oldVelocity;
        Debug.Log(oldVelocity);
        float distance = Vector2.Distance(transform.position, pointB);
        if (distance <= 1.2f)
        {
            transform.localScale = new Vector3(-1, 1, 0);
            direction = Vector2.left;
            rb2d.velocity = direction * velocidade;
        }            
        else if(distance >= 9.3f)
        {
            transform.localScale = new Vector3(1, 1, 0);
            direction = Vector2.right;
            rb2d.velocity = direction * velocidade;
        }


        if(rb2d.velocity == Vector2.zero)
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
        oldVelocity = rb2d.velocity;
        rb2d.velocity = Vector2.zero;
        Flip(Target.position);
        if (Time.time > proximoAtaque)
        {
            proximoAtaque = Time.time + taxaAtaque;
            GameObject projetilInstanciado = Instantiate(prefabProjetil, new Vector2(transform.position.x, (transform.position.y - 0.964f)), Quaternion.identity);
            if (transform.localScale.x == -1)
                projetilInstanciado.GetComponent<Rigidbody2D>().velocity = Vector2.left * 6f;
            else if(transform.localScale.x == 1)
                projetilInstanciado.GetComponent<Rigidbody2D>().velocity = Vector2.right * 6f;


            Destroy(projetilInstanciado, 3f);
            spriteAnimacao.SetBool("podeAtacar", true);
        }       
    }

    public override void VerifyState()
    {
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointA, 0.6f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointB, 0.6f);
    }


    public virtual void VerifyDistance()
    {
        float distance = Vector3.Distance(Target.position, transform.position);
        if (distance <= distanceMax)
            enemyState = EnemyState.Attacking;
        else if (distance >= distanceMax)
            enemyState = EnemyState.Patrolling;
    }
}
