using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Papagaio: EnemyRanged
{   

    private AIPath path;
    public GameObject effectDead;
    public override void Start()
    {
        distanceMax = 3f;
        enemyState = EnemyState.Patrolling;
        rb2d = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
        spriteAnimacao = GetComponent<Animator>();
    }


    public override void Update()
    {
        VerifyState();
        VerifyDistance();
        VerificarMorte();
    }


    public override void VerifyState()
    {
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                path.enabled = false;
                Andar();
                break;
            case EnemyState.Attacking:
                path.enabled = true;
                Atacar();
                break;
            default:
                break;
        }
    }

    public override void Andar()
    {
        Vector2 direction = oldVelocity;
        Debug.Log(oldVelocity);
        float distance = Vector2.Distance(transform.position, pointB);
        Debug.Log(distance);
        if (distance <= 1.2f)
        {
            transform.localScale = new Vector3(-0.8f, 0.8f, 0);
            direction = Vector2.left;
            rb2d.velocity = direction * velocidade;
        }
        else if (distance >= 9.3f)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 0);
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

    public override void Flip(Vector2 target)
    {
        if (transform.position.x > target.x)
            transform.localScale = new Vector3(-0.8f, 0.8f, 0f);
        else if (transform.position.x < target.x)
            transform.localScale = new Vector3(0.8f, 0.8f, 0f);
    }

    public override void Atacar()
    {
        float distanceToAttack = Vector3.Distance(Target.position, transform.position);
        if (distanceToAttack <= 7.6f)
        {
            spriteAnimacao.SetBool("podeAtacar", true);
            this.GetComponent<GhostEffect>().makeGhost = true;
        }
        else
        {
            spriteAnimacao.SetBool("podeAtacar", false);
            this.GetComponent<GhostEffect>().makeGhost = false;
        }
        Flip(target.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            GameObject explosion = null;
            if(explosion == null)
                Instantiate(effectDead, transform.position, Quaternion.identity);
            collision.GetComponent<Personagem>().DarDano(dano);

            Destroy(this.gameObject);
        }
    }

}
