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

    [Header("Ataque Terremoto")]
    public int quantidadeDeAtaque;
    public List<GameObject> spawnPointsTerremoto = new List<GameObject>();

    [Header("Ataque Investida")]
    float taxaAtaqueInvestida = 4f;
    public GameObject portal;
    public GameObject prefabEspirito;

    [Header("Ataque Bola de Fogo")]
    public Transform spawnPosition;
    public GameObject prefabFogo;
    float proximoAtaque;
    float taxaAtaque = 1f;

    [Header("Movimentação Circular")]
    public float frequencia = 16f;
    public float magnitude = 0.5f;



    private Transform jogadorPosicao;
   

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteAnimacao = GetComponent<Animator>();
        modoFauno = FaunoEstado.Flutuando;
    }
    private void Update()
    {
        VerificarEstados();
        ControlandoVida();
    }

    public void VerificarEstados()
    {
        switch (modoFauno)
        {
            case FaunoEstado.Flutuando:
                StartCoroutine(EsperandoAtaque());
                break;
            case FaunoEstado.Descansando:
                StartCoroutine(Descansar());
                Debug.Log("Descansar");
                break;
            case FaunoEstado.Atacando:
                Atacar();              
                if (ataques.Count <= 0)
                    modoFauno = FaunoEstado.Descansando;                    
                break;
        }
    }

    public void ControlandoVida()
    {
        
        if (vida < 0)
            Destroy(this.gameObject);           

    }

    #region Açoes
    public override void Andar()
    {
       transform.position = new Vector2(Mathf.Cos(Time.time * frequencia) * magnitude, (Mathf.Sin(Time.time * frequencia) * magnitude) + 3f);        
    }

    

    public override void Atacar()
    {
        if (jogadorPosicao == null)
           modoFauno = FaunoEstado.Flutuando;

        StartCoroutine(SelecionandoAtaque());
    }

    public override void ProcurandoJogador()
    {
        
    }
    #endregion

    #region Ataques

    public void AtaqueFogo(string nomeAtaque)
    {
        Debug.Log(nomeAtaque);        
        if (Time.time > proximoAtaque)
        {
            proximoAtaque = Time.time + taxaAtaque;
            GameObject bolaFogo = Instantiate(prefabFogo, spawnPosition.position, Quaternion.identity);
            bolaFogo.GetComponent<Rigidbody2D>().velocity = (jogadorPosicao.position - bolaFogo.transform.position).normalized * 6f;            
        }
        spriteAnimacao.SetBool(nomeAtaque, true);
        spriteAnimacao.SetBool("AtaqueTerremoto", false);
        spriteAnimacao.SetBool("AtaqueInvestida", false);
        modoFauno = FaunoEstado.Flutuando;
    }

    public void AtaqueTerremoto(string nomeAtaque)
    {
        Debug.Log(nomeAtaque);
        if(Time.time > proximoAtaque)
        {
            proximoAtaque = Time.time + taxaAtaque;
            for (int i = 0; i < quantidadeDeAtaque; i++)
            {
                int random = Random.Range(0, spawnPointsTerremoto.Count);
                GameObject estalaquitite = Instantiate(spawnPointsTerremoto[random], spawnPointsTerremoto[random].transform.position, spawnPointsTerremoto[random].transform.rotation);
                estalaquitite.GetComponent<Rigidbody2D>().gravityScale = 1;
                estalaquitite.GetComponent<Estalaquitite>().StartDestroy();
            }
            spriteAnimacao.SetBool(nomeAtaque, true);
            spriteAnimacao.SetBool("AtaqueFogo", false);
            spriteAnimacao.SetBool("AtaqueInvestida", false);
        }
    }

    public IEnumerator AtaqueInvestida(string nomeAtaque)
    {
        Debug.Log(nomeAtaque);
        portal.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (Time.time > proximoAtaque)
        {
            proximoAtaque = Time.time + taxaAtaqueInvestida;
            GameObject espirito = Instantiate(prefabEspirito, new Vector3(portal.transform.position.x, -3.22f,0f), transform.rotation);
            espirito.GetComponent<Rigidbody2D>().velocity = Vector2.left * 9f;
            spriteAnimacao.SetBool(nomeAtaque, true);
            spriteAnimacao.SetBool("AtaqueFogo", false);
            spriteAnimacao.SetBool("AtaqueTerremoto", false);
           
        }
        yield return new WaitForSeconds(2f);
        portal.GetComponent<Animator>().SetBool("Fechou", true);
        yield return new WaitForSeconds(0.2f);
        portal.GetComponent<Animator>().SetBool("Fechou", false);
        portal.SetActive(false);
        StopCoroutine(AtaqueInvestida(" "));
    }
    #endregion


    public void RandomizarAtaque()
    {
        if (ataques.Count >= ataqueMax)
            return;
        for (int i = 0; i < ataqueMax; i++)
        {
            int random = Random.Range(1, 6);

            if (random == 1)
                ataques.Enqueue("AtaqueFogo");
            else if (random == 2)
                ataques.Enqueue("AtaqueInvestida");
            else if (random == 3)
                ataques.Enqueue("AtaqueTerremoto");
        }

    }
    public IEnumerator SelecionandoAtaque()
    {
        string ataque = " ";

        if (ataques.Count > 0) ataque = ataques.Dequeue();
        yield return new WaitForSeconds(3f);
        if (ataque == "AtaqueFogo")
        {
            AtaqueFogo(ataque);
        }else if (ataque == "AtaqueInvestida")
        {
            StartCoroutine(AtaqueInvestida(ataque));

        }else if (ataque == "AtaqueTerremoto")
        {
            AtaqueTerremoto(ataque);
        }
        StopCoroutine(SelecionandoAtaque());
    }
    IEnumerator EsperandoAtaque()
    {
        Andar();
        if (ataques.Count <= 0)
            RandomizarAtaque();
        yield return new WaitForSeconds(6f);       
        modoFauno = FaunoEstado.Atacando;
        StopCoroutine(EsperandoAtaque());
    }

    IEnumerator Descansar()
    {
        spriteAnimacao.SetBool("AtaqueTerremoto", false);
        spriteAnimacao.SetBool("AtaqueFogo", false);
        spriteAnimacao.SetBool("AtaqueInvestida", false);
        spriteAnimacao.SetBool("Cansado", true);
        yield return new WaitForSeconds(3);
        if(transform.position.y > -2.8f)
            transform.Translate((new Vector3(0, -2.8f, 0f) * 7f * Time.deltaTime));
        yield return new WaitForSeconds(6.5f);       
        modoFauno = FaunoEstado.Flutuando;
        spriteAnimacao.SetBool("Cansado", false);
        StopCoroutine(Descansar());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() != null)
            jogadorPosicao = collision.transform;
    }
}