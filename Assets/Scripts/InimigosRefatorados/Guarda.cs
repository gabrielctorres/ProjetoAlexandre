using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guarda : EnemyMelee
{        
    
    public override void Start()
    {
        base.Start();
        enemyState = EnemyState.Attacking;
    }

    
    public override void Update()
    {
       base.Update();        
    }

}
