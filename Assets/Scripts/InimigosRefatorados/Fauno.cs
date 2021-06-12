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
        if (vida > 10)
            frequencia = 2;
        else if (vida <=10)
            frequencia = 0.5f;
        else
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
        if(jogadorPosicao.position.x > 0)
            portal.transform.position = new Vector3((jogadorPosicao.position.x + 3f), portal.transform.position.y, 0f);
        else
            portal.transform.position = new Vector3((jogadorPosicao.position.x - 3f), portal.transform.position.y, 0f);
        portal.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (Time.time > proximoAtaque)
        {
            proximoAtaque = Time.time + taxaAtaque;
            GameObject espirito = Instantiate(prefabEspirito, new Vector3(portal.transform.position.x, -3.22f,0f), transform.rotation);
            if (jogadorPosicao.position.x > 0)
            {
                espirito.GetComponent<SpriteRenderer>().flipX = true;
                espirito.GetComponent<Rigidbody2D>().velocity = Vector2.right * 15f;
            }                
            else
            {
                espirito.GetComponent<SpriteRenderer>().flipX = false;
                espirito.GetComponent<Rigidbody2D>().velocity = Vector2.left * 15f;               
            }
            spriteAnimacao.SetBool(nomeAtaque, true);
            spriteAnimacao.SetBool("AtaqueFogo", false);
            spriteAnimacao.SetBool("AtaqueTerremoto", false);
           
        }
        portal.GetComponent<Animator>().SetBool("Fechou", true);
        yield return new WaitForSeconds(3f);
        portal.SetActive(false);
    }
    #endregion


    public void RandomizarAtaque()
    {
        if (ataques.Count >= ataqueMax)
            return;
        for (int i = 0; i < ataqueMax; i++)
        {
            int random = Random.Range(1, 4);

            if (random == 1)
                ataques.Enqueue("AtaqueFogo");
            else if (random == 2)
                ataques.Enqueue("AtaqueTerremoto");
            else if (random == 3)
                ataques.Enqueue("AtaqueInvestida");
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
        }
        yield return new WaitForSeconds(3f);
        if (ataque == "AtaqueInvestida")
        {
            StartCoroutine(AtaqueInvestida(ataque));
        }
        yield return new WaitForSeconds(3f);
        if (ataque == "AtaqueTerremoto")
        {
            AtaqueTerremoto(ataque);
        }
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
        frequencia = 0;
        yield return new WaitForSeconds(3);
        if(transform.position.y > -2.8f)
            transform.Translate((new Vector3(0, -2.8f, 0f) * 10f * Time.deltaTime));
        yield return new WaitForSeconds(10f);       
        modoFauno = FaunoEstado.Flutuando;
        spriteAnimacao.SetBool("Cansado", false);
        frequencia = 2;
        StopCoroutine(Descansar());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() != null)
            jogadorPosicao = collision.transform;
    }
}