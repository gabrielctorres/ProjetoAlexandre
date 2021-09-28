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
        enemyState = EnemyState.Attacking;
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
        if (Target != null)
        {
            float distanceAttack = Vector2.Distance(Target.position, transform.position);         
            

            if (distanceAttack >= distanciaProAtaque)
            {
                Vector3 direction = new Vector3((Target.position.x - transform.position.x), 0f, 0f);
                spriteAnimacao.SetFloat("Horizontal", Mathf.Abs(transform.position.x));
                spriteAnimacao.SetBool("podeAtacar", false);
                rb2d.velocity = direction.normalized * velocidade;
                Flip(Target.position);
                Debug.Log("não atacando");
            }
            else if (distanceAttack <= distanciaProAtaque)
            {
                if (Time.time > proximoAtaque)
                {
                    proximoAtaque = Time.time + taxaAtaque;
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(posicaoArma.position, tamanhoAtaque, hitMask);
                    spriteAnimacao.SetBool("podeAtacar", true);
                    foreach (Collider2D objeto in hitEnemies)
                    {
                        if (!objeto.GetComponent<Personagem>().invulneravel)
                        {
                            objeto.GetComponent<Personagem>().DarDano(dano);
                            objeto.GetComponent<Rigidbody2D>().AddForce(new Vector2(objeto.GetComponent<Personagem>().direcaoOlhar * -1, 0) * 3f, ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }

    }


    public override void TomarDano(float danoJogador)
    {
        vida -= danoJogador;
        StartCoroutine(EfeitoDano());
        if (vida < 1)
        {
            if (gameObject.GetComponent<LootScript>() != null)
                gameObject.GetComponent<LootScript>().calculandoLoot();
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
        
    }

}
