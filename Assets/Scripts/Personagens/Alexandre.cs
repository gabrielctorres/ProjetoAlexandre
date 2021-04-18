using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alexandre : Personagem
{
    public GameObject tutorialAtaque,tutorialAtaque2;
    bool podePegar;
    Animator mesaAnimator;
    float timerSkillOne = 0;
    float timerSkillOneMax = 1;

    float timerSkillTwo = 0;
    float timerSkillTwoMax = 5;
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
        Flip();
        if (!semArma)
        {
            Ataque();
            SegundoAtaque();
            uiHabilidades.SetActive(true);
        }
        VerificarMorte();
        vidaImagem.fillAmount = vida / vidaMax;
        spriteAnimation.SetBool("SemArma", semArma);

        if (Input.GetKeyDown(KeyCode.Z) && podePegar)
        {
            PegaArma();
        }

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
        timerImageAdaga.fillAmount = timerSkillOne / timerSkillOneMax;
        if (Input.GetButtonDown("PrimeiroAtaque") && habilidadeAdagaAtiva )
        {           
            spriteAnimation.SetBool("AtacouNormal", true);
            habilidadeAdagaAtiva = false;            
        }            
       else
            spriteAnimation.SetBool("AtacouNormal", false);
    }
   


    public override void SegundoAtaque()
    {
        if (!habilidadeEspadaAtiva)
        {
            if (timerSkillTwo <= 5)
            {
                timerSkillTwo += Time.fixedDeltaTime;
            }
            else
            {
                habilidadeEspadaAtiva = true;
                timerSkillTwo = 0;
            }

        }

        timerImageEspada.fillAmount = timerSkillTwo / timerSkillTwoMax;
        if (Input.GetButtonDown("SegundoAtaque") && habilidadeEspadaAtiva)
        {          
            spriteAnimation.SetBool("AtacouEspada", true);
            habilidadeEspadaAtiva = false;
            
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "EspadaInimigo")
        {
            this.DarDano(collision.GetComponent<Weapon>().dano);
            Debug.Log(vida);
        }
    }
}