using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papagaio : InimigoComumVoador
{
    Vector3 posicaoDeslocadaDireita;
    Vector3 posicaoDeslocadaEsquerda;
    bool irPraEsquerda;
    bool irPraDireita;    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        posicaoDeslocadaDireita = transform.position;
        posicaoDeslocadaDireita.x += 10;
        posicaoDeslocadaEsquerda = transform.position;
        posicaoDeslocadaEsquerda.x -= 10;
        irPraDireita = true;
        irPraEsquerda = false;        
        dano = 2f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (estaSeMovendo)
        {
            velocidadeDoInimigo = 6.0f;
            transform.position = Vector3.MoveTowards(transform.position, posicaoDoJogador.position, Mathf.Abs(velocidadeDoInimigo) * Time.deltaTime);
            spriteAnimation.SetTrigger("podeAtacar");
            StartCoroutine(Kamikaze());
        }
        else
        {
            StartCoroutine(Patrulha());            
        }
    }  
    
    IEnumerator Patrulha()
    {
        if (irPraDireita)
        {
            sprite.flipX = false;            
            transform.position = Vector3.MoveTowards(transform.position, posicaoDeslocadaDireita, Mathf.Abs(velocidadeDoInimigo) * Time.deltaTime);
            yield return new WaitForSeconds(5.0f);
            irPraDireita = false;
            irPraEsquerda = true;            
        }

        if (irPraEsquerda)
        {
            sprite.flipX = true;            
            transform.position = Vector3.MoveTowards(transform.position, posicaoDeslocadaEsquerda, Mathf.Abs(velocidadeDoInimigo) * Time.deltaTime);
            yield return new WaitForSeconds(5.0f);
            irPraEsquerda = false;
            irPraDireita = true;              
        }        
    }    
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Personagem")
        {
            personagem.DarDano(dano);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Kamikaze()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
}
