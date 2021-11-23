using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aranha : EnemyRanged
{
    
    public override void Start()
    {
        base.Start();
    }

    
    public override void Update()
    {
        base.Update();
    }


    public override void VerifyState()
    {

        switch (enemyState)
        {
            case EnemyState.Patrolling:
                this.GetComponent<AudioSource>().mute = false;
                Andar();
                break;
            case EnemyState.Attacking:
                Atacar();
                this.GetComponent<AudioSource>().mute = true;
                break;
            default:
                break;
        }
    }
}
