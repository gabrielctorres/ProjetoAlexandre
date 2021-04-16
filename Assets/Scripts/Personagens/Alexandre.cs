using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alexandre : Personagem
{
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

    public override void Update()
    {
        base.Update();
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
        if (Input.GetButtonDown("Fire1") && habilidadeAdagaAtiva )
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
        if (Input.GetButtonDown("Fire2") && habilidadeEspadaAtiva)
        {          
            spriteAnimation.SetBool("AtacouEspada", true);
            habilidadeEspadaAtiva = false;
            
        }
        else
            spriteAnimation.SetBool("AtacouEspada", false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "BonecoTeste")
        {
            DarDano(0.5f);
            Debug.Log("a");
        }
    }
}