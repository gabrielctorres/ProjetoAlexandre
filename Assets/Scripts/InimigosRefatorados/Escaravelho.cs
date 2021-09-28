using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escaravelho : EntidadeBase
{

    private bool canAttack = false;
    private SpriteRenderer sprite;

    public bool CanAttack { get => canAttack; set => canAttack = value; }

    public void Start()
    {
        if (CanAttack)
            enemyState = EnemyState.Attacking;
        else
            enemyState = EnemyState.Patrolling;

        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        VerifyState();
    }

    public override void VerifyState()
    {
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                Patrulha();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            default:
                break;
        }
    }

    public void Attack()
    {
        rb2d.velocity = (Target.position - transform.position) * velocidade;
        Flip(Target.position);
    }
    void Patrulha()
    {
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


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointA, 0.6f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointB, 0.6f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canAttack && collision.GetComponent<Personagem>() != null)
        {
            collision.GetComponent<Personagem>().DarDano(1f);
        }
    }

    public override void Atacar()
    {
        throw new System.NotImplementedException();
    }

    public override void Andar()
    {
        throw new System.NotImplementedException();
    }
}
