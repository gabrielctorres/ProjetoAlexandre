using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escorpiao : EnemyMelee
{    
    // Start is called before the first frame update
    public override void Start()
    {       
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = EnemyState.Attacking;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
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
        if (distance <= transform.position.x)
            enemyState = EnemyState.Attacking;
        else if (distance >= transform.position.x)
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
