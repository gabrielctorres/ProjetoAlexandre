using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class FaunoVG : EntidadeBase
{
    public FaunoEstado modoFauno;

    [Header("Interface")]
    public GameObject bossLife;
    public Image bossImage;
    public TextMeshProUGUI nameText;
    public GameObject menuDead;

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

    [Header("Movimentação Circular")]
    public float frequencia;
    public float magnitude;
    private Transform jogadorPosicao;

    public CinemachineVirtualCamera cam;
    
    private bool vendoPlayer;
    private float atktempoderecarga = 0;
    public float tempomaximopradescanada = 5;
    private float tempodescanado;

    private void Start()
    {
        
        spriteAnimacao = GetComponent<Animator>();
        modoFauno = FaunoEstado.Flutuando;    

        tempodescanado = tempomaximopradescanada;
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
                EsperandoAtaque();
                break;
            case FaunoEstado.Descansando:
                Descansando();
                break;
            case FaunoEstado.Atacando:
                Atacar();
                break;
        }
    }


    #region Açoes
    public override void Andar()
    {
        transform.position = new Vector2((Mathf.Cos(Time.time * frequencia) * magnitude) + 271, (Mathf.Sin(Time.time * frequencia) * magnitude) + 3f);
    }

    public override void Atacar()
    {
        if (atktempoderecarga <= 0)
        {
            if (ataques.Count > 0)
            {
                StartCoroutine(ataques.Dequeue());
                atktempoderecarga = 5;
            }
            else
                modoFauno = FaunoEstado.Descansando;
        }
        else
        {           
            Andar();
            atktempoderecarga -= Time.deltaTime;
        }
    }

    public void EsperandoAtaque()
    {
        Andar();
        if (vendoPlayer)
        {
            StartCoroutine(esperar());
        }
        else
        {
            
            return;
        }
    }

    IEnumerator esperar()
    {
        yield return new WaitForSeconds(5f);
        RandomizarAtaque();
        modoFauno = FaunoEstado.Atacando;
        StopCoroutine(esperar());
    }
    public void ControlandoVida()
    {
        bossImage.fillAmount = vida / vidaMax;

        frequencia = (vida / vidaMax) * 3;

        if(vida <=0)
            Destroy(this.gameObject);

    }


    public void Descansando()
    {

        if (tempodescanado <= 0)
        {
            spriteAnimacao.SetBool("Cansado", false);            
            tempodescanado = tempomaximopradescanada;
            RandomizarAtaque();
            modoFauno = FaunoEstado.Flutuando;
        }
        else
        {            
            tempodescanado -= Time.deltaTime;
            if (transform.position.y > -2.8f)
                transform.Translate((new Vector3(0, -2.8f, 0f) * 5f * Time.deltaTime));
            spriteAnimacao.SetBool("Cansado", true);
        }
    }

    #endregion


    #region Ataques

    public IEnumerator AtaqueFogo()
    {
        
        GameObject bolaFogo = Instantiate(prefabFogo, spawnPosition.position, Quaternion.identity);
        bolaFogo.GetComponent<Rigidbody2D>().velocity = (jogadorPosicao.position - bolaFogo.transform.position).normalized * 10f;
        spriteAnimacao.SetBool("AtaqueFogo",true);
        yield return new WaitForSeconds(2f);
        spriteAnimacao.SetBool("AtaqueFogo", false);        
    }
    public IEnumerator AtaqueTerremoto()
    {        
        for (int i = 0; i < quantidadeDeAtaque; i++)
        {
            int random = Random.Range(0, spawnPointsTerremoto.Count);
            GameObject estalaquitite = Instantiate(spawnPointsTerremoto[random], spawnPointsTerremoto[random].transform.position, spawnPointsTerremoto[random].transform.rotation);
            estalaquitite.GetComponent<Rigidbody2D>().gravityScale = 1;
            estalaquitite.GetComponent<Estalaquitite>().StartDestroy();
        }
        spriteAnimacao.SetBool("AtaqueTerremoto", true);
        yield return new WaitForSeconds(4f);
        spriteAnimacao.SetBool("AtaqueTerremoto", false);        
    }
    public IEnumerator AtaqueInvestida()
    {        
        if (jogadorPosicao.position.x > 0 && !portal.activeInHierarchy)
            portal.transform.position = new Vector3((jogadorPosicao.position.x + 3f), portal.transform.position.y, 0f);
        else
            portal.transform.position = new Vector3((jogadorPosicao.position.x - 3f), portal.transform.position.y, 0f);
        portal.SetActive(true);
        yield return new WaitForSeconds(1f);

        GameObject espirito = Instantiate(prefabEspirito, new Vector3(portal.transform.position.x, -3.22f, 0f), Quaternion.identity);
        if (jogadorPosicao.position.x > 0)
        {
            espirito.GetComponent<SpriteRenderer>().flipX = true;
            espirito.GetComponent<Rigidbody2D>().velocity = (jogadorPosicao.position - espirito.transform.position).normalized * 16f;
        }
        else
        {
            espirito.GetComponent<SpriteRenderer>().flipX = false;
            espirito.GetComponent<Rigidbody2D>().velocity = (jogadorPosicao.position - espirito.transform.position).normalized * 16f;
        }
        spriteAnimacao.SetBool("AtaqueInvestida", true);
        yield return new WaitForSeconds(4f);
        portal.GetComponent<Animator>().SetBool("Fechou", true);
        yield return new WaitForSeconds(3f);
        portal.GetComponent<Animator>().SetBool("Fechou", false);        
        spriteAnimacao.SetBool("AtaqueInvestida", false);
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
                ataques.Enqueue("AtaqueInvestida");
            else if (random == 3)
                ataques.Enqueue("AtaqueTerremoto");
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() !=null)
        {
            cam.m_Lens.OrthographicSize = 7;
            vendoPlayer = true;
            jogadorPosicao = collision.transform;
            bossLife.SetActive(true);
            nameText.text = this.gameObject.name;
            this.GetComponent<CircleCollider2D>().radius = 1;
        }
           
    }   


    public override void ProcurandoJogador()
    {
       
    }

    private void OnDestroy()
    {
        menuDead.SetActive(true);
        menuDead.GetComponentInChildren<TextMeshProUGUI>().text = "Obrigado por testar nosso jogo, não esqueça de responder o  formulario";
        Time.timeScale = 0;
    }
}
