using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alexandre : Personagem
{
    float attackRate = 1;
    float attackRateSword = 5;
    float nextAttack = 0;

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
        if (Input.GetButtonDown("Fire1") && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            spriteAnimation.SetBool("AtacouNormal", true);
        }
            
       else
            spriteAnimation.SetBool("AtacouNormal", false);
    }

    public override void SegundoAtaque()
    {
        if (Input.GetButtonDown("Fire2") && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRateSword;
            spriteAnimation.SetBool("AtacouEspada", true);
        }

        else
            spriteAnimation.SetBool("AtacouEspada", false);
    }
}
