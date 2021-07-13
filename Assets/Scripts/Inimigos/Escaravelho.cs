using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escaravelho : InimigoComumVoador
{
    public GameObject pontoDireitoPatrulha;
    public GameObject pontoEsquerdoPatrulha;
    bool irPraEsquerda;
    bool irPraDireita;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();        
        irPraDireita = true;
        irPraEsquerda = false;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        Patrulha();
    }   

    void Patrulha()
    {
        if (irPraDireita)
        {            
            sprite.flipX = false;
            transform.position = Vector3.MoveTowards(transform.position, pontoDireitoPatrulha.transform.position, Mathf.Abs(velocidadeDoInimigo) * Time.deltaTime);
            if (transform.position.x >= pontoDireitoPatrulha.transform.position.x)
            {
                irPraDireita = false;
                irPraEsquerda = true;
            }
        }

        if (irPraEsquerda)
        {           
            sprite.flipX = true;
            transform.position = Vector3.MoveTowards(transform.position, pontoEsquerdoPatrulha.transform.position, Mathf.Abs(velocidadeDoInimigo) * Time.deltaTime);
            if (transform.position.x <= pontoEsquerdoPatrulha.transform.position.x)
            {
                irPraDireita = true;
                irPraEsquerda = false;
            }
        }
    }
}
