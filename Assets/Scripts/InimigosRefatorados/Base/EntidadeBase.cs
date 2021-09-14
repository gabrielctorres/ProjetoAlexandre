using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class EntidadeBase : MonoBehaviour
{
    public EnemyState enemyState;
    private Transform target;
    public float radiusView;
    public float vida,vidaMax;    
    public float velocidade;
    [HideInInspector] public Animator spriteAnimacao;
    [HideInInspector] public Rigidbody2D rb2d;

    
    [Header("Config Points")]
    public Vector2 pointA;
    public Vector2 pointB;

    public Transform Target { get => target; set => target = value; }

    public abstract void VerifyState();
    public abstract void Atacar();
    public virtual void ProcurandoJogador() { }
    public abstract void Andar();

   
    public virtual void TomarDano(float dano)
    {
        vida -= dano;
        StartCoroutine(EfeitoDano());
    }

    public virtual void Flip(Vector2 target )
    {
        if (transform.position.x > target.x)
            transform.localScale = new Vector3(-1, 1f, 0f);
        else if (transform.position.x < target.x)
            transform.localScale = new Vector3(1, 1f, 0f);
    }

   public  IEnumerator EfeitoDano()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        StopCoroutine(EfeitoDano());
    }


}
