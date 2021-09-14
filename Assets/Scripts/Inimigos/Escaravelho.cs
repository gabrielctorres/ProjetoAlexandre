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
        float time = Mathf.PingPong(Time.time * velocidade, 1f);
        Vector2 direction = Vector2.Lerp(pointA, pointB, time);
        transform.position = direction;
        Debug.Log(direction);

        if (direction.x >= pointA.x)
        {
            Flip(pointA);
            Debug.Log("a");
        }
        else if (direction.x <= pointB.x)
        {
            Flip(pointB);
            sprite.flipX = true;
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
