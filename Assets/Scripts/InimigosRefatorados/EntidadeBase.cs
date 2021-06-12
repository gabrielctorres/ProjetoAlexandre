using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EntidadeBase : MonoBehaviour
{
    public float vida;
    public float dano;
    public float velocidade;
    [HideInInspector] public Animator spriteAnimacao;
    [HideInInspector] public Rigidbody2D rb2d;


    public abstract void Atacar();
    public abstract void ProcurandoJogador();
    public abstract void Andar();

    //Flip();
    public void TomarDano(float dano)
    {
        vida -= dano;
        StartCoroutine(EfeitoDano());
    }



    IEnumerator EfeitoDano()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        StopCoroutine(EfeitoDano());
    }
}
