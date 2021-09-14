using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escorpiao : EnemyMelee
{    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Atacar()
    {
        if (Target != null) return;

        float distanceAttack = Vector2.Distance(Target.position, transform.position);



        if (distanceAttack >= 1.2f)
        {
            Vector3 direction = new Vector3((Target.position.x - transform.position.x), 0f, 0f);
            rb2d.velocity = direction * velocidade;
            Flip(Target.position);
            Debug.Log("não atacando");

        }
        else if (distanceAttack <= 1.2f)
        {
            rb2d.velocity = Vector2.zero;
            Flip(Target.position);

            if (Time.time > proximoAtaque)
            {
                proximoAtaque = Time.time + taxaAtaque;
                
                Debug.Log("atacando");
                CreateCollision();
            }
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

    public override void VerifyDistance()
    {
        if (Target != null) return;
        float distance = Vector3.Distance(Target.position, transform.position);
        if (distance <= radiusView)
            enemyState = EnemyState.Attacking;
        else if (distance >= radiusView)
            enemyState = EnemyState.Patrolling;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            Target = collision.transform;
        }
    }

}
