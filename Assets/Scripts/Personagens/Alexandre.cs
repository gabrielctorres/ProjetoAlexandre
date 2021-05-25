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
    float timerSkillOne = 0;
    float timerSkillOneMax = 1;

    public  BossFase1 bossController;

    float timerSkillTwo = 0;
    float timerSkillTwoMax = 10;
    bool habilidadeAdagaAtiva = true;
    bool habilidadeEspadaAtiva = true;

    public Image timerImageAdaga;
    public Image timerImageEspada;    
    
    public override void Start()
    {
        base.Start();
    }   


    public override void FixedUpdate()
    {
        base.FixedUpdate();
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

    public override void Ataque()
    {        
        if (!habilidadeAdagaAtiva)
        {
            if (timerSkillOne <= 1)
            {
                timerSkillOne += Time.fixedDeltaTime;               
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
                timerSkillTwo += Time.fixedDeltaTime;
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
            spriteAnimation.SetBool("AtacouEspada", true);
            habilidadeEspadaAtiva = false;
            StartCoroutine(nameof(BloqueandoMovimentacao));
        }
        else
            spriteAnimation.SetBool("AtacouEspada", false);
    }


    public void PegaArma()
    {
        semArma = false;
        if(mesaAnimator !=null)
            mesaAnimator.SetBool("PegarArma", true);
        tutorialAtaque.SetActive(true);
        tutorialAtaque2.SetActive(true);
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Escorpiao"))
        {
            if (!invulneravel)
            {
                this.DarDano(collision.collider.GetComponent<Escorpiao>().dano);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "EspadaInimigo")
        {
            this.DarDano(collision.GetComponent<Weapon>().dano);
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(direcaoOlhar * -1, 0) * 60, ForceMode2D.Impulse);
        }

        if(collision.name == "Alabarda")
        {
            this.DarDano(collision.GetComponent<Weapon>().dano);
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(direcaoOlhar * -1, 0) * 60, ForceMode2D.Impulse);
        }

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