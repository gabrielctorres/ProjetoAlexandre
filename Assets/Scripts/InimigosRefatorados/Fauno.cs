using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FaunoEstado
{
    Flutuando,
    Descansando,
    Atacando,
}
public class Fauno : EntidadeBase
{
    public FaunoEstado modoFauno;

    [Header("Controle dos ataques", order = 1)]
    public Queue<string> ataques = new Queue<string>();
    public int ataqueMax;


    [Header("Ataque Bola de Fogo")]
    public Transform spawnPosition;
    public GameObject prefabFogo;

    [Header("Movimentação Circular")]
    public float frequencia = 16f;
    public float magnitude = 0.5f;



    private Transform jogadorPosicao;

    private bool podeAtacar = false;

   

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteAnimacao = GetComponent<Animator>();
        modoFauno = FaunoEstado.Flutuando;
    }
    private void Update()
    {
        VerificarEstados();       
        Debug.Log(podeAtacar);
    }

    public void VerificarEstados()
    {
        switch (modoFauno)
        {
            case FaunoEstado.Flutuando:
                StartCoroutine(EsperandoAtaque());
                break;
            case FaunoEstado.Descansando:
                Debug.Log("Descansar");
                spriteAnimacao.SetBool("AtaqueFogo", false);
                break;
            case FaunoEstado.Atacando:
                Atacar();
                break;
        }
    }


    public override void Andar()
    {
        transform.position = new Vector2(Mathf.Cos(Time.time * frequencia) * magnitude, (Mathf.Sin(Time.time * frequencia) * magnitude) + 3f);
    }

    public override void Atacar()
    {
        if (jogadorPosicao == null)
           modoFauno = FaunoEstado.Flutuando;

        SelecionandoAtaque();

        if (ataques.Count <= 0)
            modoFauno = FaunoEstado.Descansando;
    }

    public override void ProcurandoJogador()
    {
        
    }


    public void RandomizarAtaque()
    {
        if (ataques.Count >= ataqueMax)
            return;
        for (int i = 0; i < ataqueMax; i++)
        {
            int random = Random.Range(1,4);     

            if (random == 1)
                ataques.Enqueue("AtaqueFogo");
            else if (random == 2)
                ataques.Enqueue("AtaqueFogo");
            else if (random == 3)
                ataques.Enqueue("AtaqueFogo");
        }

    }

    public void SelecionandoAtaque()
    {
        string ataque = " ";
        if (ataques.Count >0)  ataque = ataques.Dequeue();

        if (ataque == "AtaqueFogo")
        {
            AtaqueFogo(ataque);
        }
        else if (ataque == "AtaqueInvestida")
        {            
            Debug.Log(ataque);
            modoFauno = FaunoEstado.Flutuando;
        }
        else
        {            
            Debug.Log(ataque);
            modoFauno = FaunoEstado.Flutuando;            
        }
    }


    public void AtaqueFogo(string nomeAtaque)
    {
        Debug.Log(nomeAtaque);
        GameObject bolaFogo = Instantiate(prefabFogo, spawnPosition.position, Quaternion.identity);
        bolaFogo.GetComponent<BolaFogo>().jogadorPosition = jogadorPosicao.position;
        spriteAnimacao.SetBool(nomeAtaque, true);
      
        
        modoFauno = FaunoEstado.Flutuando;
    }


    IEnumerator EsperandoAtaque()
    {
        Andar();
        if(ataques.Count <= 0)
            RandomizarAtaque();
        yield return new WaitForSeconds(6f);       
        modoFauno = FaunoEstado.Atacando;
        StopCoroutine(EsperandoAtaque());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() != null)
            jogadorPosicao = collision.transform;
    }
}