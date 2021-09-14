using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marinheiro : EnemyMelee
{
    public BossFase1 bossFase1;
    public bool podeSeguir;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        VerifyDistance();

        if (podeSeguir)
            VerifyState();
    }

    public override void VerifyState()
    {
        
        switch (enemyState)
        {
            case EnemyState.Attacking:
                Atacar();
                break;                
            default:
                break;
        }
    }

    public override void Atacar()
    {
        

        float distanceAttack = Vector2.Distance(Target.position, transform.position);

        Debug.Log(distanceAttack);


        if (distanceAttack >= 1.8f)
        {
            Vector3 direction = new Vector3((Target.position.x - transform.position.x), 0f, 0f);
            rb2d.velocity = direction * velocidade;
            Flip(Target.position);
            Debug.Log("não atacando");
        }
        else if (distanceAttack <= 1.8f)
        {
            rb2d.velocity = Vector2.zero;
            Flip(Target.position);

            if (Time.time > proximoAtaque)
            {
                proximoAtaque = Time.time + taxaAtaque;
                spriteAnimacao.SetBool("podeAtacar", true);
                Debug.Log("atacando");
                CreateCollision();
            }
            else
                spriteAnimacao.SetBool("podeAtacar", false);
        }

    }



    public override void TomarDano(float danoJogador)
    {
        vida -= danoJogador;
        StartCoroutine(EfeitoDano());
        if (vida < 1)
        {
            if (bossFase1 != null)
            {
                bossFase1.hpBoss -= 1;
                bossFase1.inimigos.RemoveAt(bossFase1.inimigos.IndexOf(this.gameObject));
            }
            Destroy(this.gameObject);
        }
    }

    public override void VerifyDistance(){}


    private void OnDestroy()
    {
        if(gameObject.GetComponent<LootScript>() != null) gameObject.GetComponent<LootScript>().calculandoLoot();
    }

}
