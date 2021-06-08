using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Alexandre : Personagem
{
    public GameObject tutorialAtaque,tutorialAtaque2;  

    bool podePegar;
    Animator mesaAnimator;
    float timerSkillOne = 0f;
    float timerSkillOneMax = 1.4f;

    public  BossFase1 bossController;

    float timerSkillTwo = 0;
    float timerSkillTwoMax = 10;
    bool habilidadeAdagaAtiva = true;
    bool habilidadeEspadaAtiva = true;

    float timerDesh = 0f;
    bool habilidadeDesh = true;

    public Image timerImageAdaga;
    public Image timerImageEspada;

    [Header("Configuração da adaga")]
    public Transform pointAdaga;
    public float tamanhoAdaga = 0.5f;
    public float danoAdaga;


    [Header("Configuração da espada")]
    public Transform pointEspada;
    public float tamanhoEspada = 0.5f;
    public float danoEspada;


    public LayerMask hitMask;
    
    public override void Start()
    {
        base.Start();
    }   


    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Dash();
    }

    public void Update()
    {
        if(estaNoChao)
             Flip();


        if (!semArma)
        {
            Ataque();
            SegundoAtaque();
            if(uiHabilidades !=null)uiHabilidades.SetActive(true);
        }
        VerificarMorte();
        if(vidaImagem !=null) vidaImagem.fillAmount = vida / vidaMax;
        spriteAnimation.SetBool("SemArma", semArma);

        if (Input.GetKeyDown(KeyCode.Z) && podePegar)
        {
            PegaArma();
        }
        if(textReliquias != null)
            textReliquias.text = "Reliquias Coletadas: " + numReliquias;       
    }

    #region Ataques
    public override void Ataque()
    {        
        if (!habilidadeAdagaAtiva)
        {
            if (timerSkillOne <= 1.4f)
            {
                timerSkillOne += Time.deltaTime;               
            }    
            else
            {
                habilidadeAdagaAtiva = true;
                timerSkillOne = 0;
            }
                    
        }
        
        if(timerImageAdaga != null ) timerImageAdaga.fillAmount = timerSkillOne / timerSkillOneMax;

        if (Input.GetButtonDown("PrimeiroAtaque") && habilidadeAdagaAtiva )
        {
            //Guardando cada inimigo dependendo da layer que a adaga colidiu
            Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(pointAdaga.position,tamanhoAdaga,hitMask);
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(pointAdaga.position, tamanhoAdaga, LayerMask.GetMask("ObjetosNormais"));
            //Passando por casa inimigo e aplicando dano e aplicando força
           foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Usando a adaga você acertou: " + enemy.name);
                enemy.GetComponent<InimigoComum>().TomarDano(danoAdaga);
                enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(enemy.GetComponent<InimigoComum>().direcaoOlhar * -1, 0) * 1.9f, ForceMode2D.Impulse);
            }

            foreach (Collider2D objeto in hitObjects)
            {
                objeto.GetComponent<ObjetosQuebraveis>().spriteAnimation.SetTrigger("quebrou");
                objeto.GetComponent<ObjetosQuebraveis>().Destroi();
            }

            spriteAnimation.SetBool("AtacouNormal", true);
            habilidadeAdagaAtiva = false;
            StartCoroutine(nameof(BloqueandoRotacao));
        }            
       else
            spriteAnimation.SetBool("AtacouNormal", false);
    }

    public override void SegundoAtaque()
    {
        if (!habilidadeEspadaAtiva)
        {
            if (timerSkillTwo <= 10)
            {
                timerSkillTwo += Time.deltaTime;
            }
            else
            {
                habilidadeEspadaAtiva = true;
                timerSkillTwo = 0;
            }

        }

        if(timerImageAdaga != null ) timerImageEspada.fillAmount = timerSkillTwo / timerSkillTwoMax;
        if (Input.GetButtonDown("SegundoAtaque") && habilidadeEspadaAtiva)
        {         

            //Guardando cada inimigo dependendo da layer que a adaga colidiu
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(pointEspada.position, tamanhoEspada, hitMask);
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(pointAdaga.position, tamanhoAdaga, LayerMask.GetMask("ObjetosPesados"));
            //Passando por casa inimigo e aplicando dano e aplicando força
            foreach (Collider2D enemys in hitEnemies)
            {
                Debug.Log("Usando a espada você acertou: " + enemys.name);
                enemys.GetComponent<InimigoComum>().TomarDano(danoEspada);
                enemys.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                enemys.GetComponent<Rigidbody2D>().AddForce(new Vector2(enemys.GetComponent<InimigoComum>().direcaoOlhar * -1, 0) * 2.6f, ForceMode2D.Impulse);
            }

            foreach (Collider2D objeto in hitObjects)
            {
                objeto.GetComponent<ObjetosQuebraveis>().spriteAnimation.SetTrigger("quebrou");
                objeto.GetComponent<ObjetosQuebraveis>().Destroi();
            }

            spriteAnimation.SetBool("AtacouEspada", true);
            habilidadeEspadaAtiva = false;
            StartCoroutine(nameof(BloqueandoMovimentacao));
        }
        else
            spriteAnimation.SetBool("AtacouEspada", false);
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (pointAdaga == null || pointEspada == null)
            return;

        Gizmos.DrawWireSphere(pointAdaga.position, tamanhoAdaga);
        Gizmos.DrawWireSphere(pointEspada.position, tamanhoEspada);
    }

    public void PegaArma()
    {
        semArma = false;
        if(mesaAnimator !=null)
            mesaAnimator.SetBool("PegarArma", true);
        tutorialAtaque.SetActive(true);
        tutorialAtaque2.SetActive(true);
    }


    public void Dash()
    {
        if (!habilidadeDesh)
        {
            if (timerDesh <= 0.3f)
            {
                timerDesh += Time.deltaTime;
            }
            else
            {
                habilidadeAdagaAtiva = true;
                timerDesh = 0;
            }
                
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && habilidadeDesh)
        {
            
            spriteAnimation.SetBool("Dash", true);            
            rb2d.AddForce((Vector2.right * direcaoOlhar).normalized * (velocidade * 10f), ForceMode2D.Impulse);
        }
        else
        {
            spriteAnimation.SetBool("Dash", false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "BonecoTeste")
        {
            DarDano(0.5f);
            Debug.Log("a");
        }

        if (collision.collider.CompareTag("Mesa") && semArma)
        {
            podePegar = true;
            mesaAnimator = collision.collider.GetComponent<Animator>();
        }

        if (collision.collider.CompareTag("Escorpiao"))
        {
            if (!invulneravel)
            {
                this.DarDano(collision.collider.GetComponent<Escorpiao>().dano);                
            }                        
        }

        if(collision.collider.name == "Barco" && bossController.hpBoss <=0)
        {
            menuDead.SetActive(true);
            menuDead.GetComponentInChildren<TextMeshProUGUI>().text = "Obrigado por testar nosso jogo";
            Time.timeScale = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Reliquias>() != null)
        {
            numReliquias++;
            Destroy(collision.gameObject);
        }
    }       
    IEnumerator BloqueandoRotacao()
    {
        atacandoAdaga = true;
        yield return new WaitForSeconds(1f);
        atacandoAdaga = false;
    }
    
    IEnumerator BloqueandoMovimentacao()
    {
        podeAndar = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        podeAndar = true;
    }
}