using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FaunoEstado
{
    Flutuando,
    Descansando,
    AtaqueFogo,
    AtaqueInvestida,
    AtaqueTerremoto
}
public class Fauno : EntidadeBase
{
    public FaunoEstado modoFauno;

    [Header("Controle dos ataques",order = 1)]
    private int ataqueCurret;
    public int ataqueMax;


    [Header("Movimentação Circular")]
    public float frequencia = 16f;
    public float magnitude = 0.5f;

    

    private Transform jogadorPosicao;

    private bool podeAtacar = false;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        modoFauno = FaunoEstado.Flutuando;
    }
    private void Update()
    {
        VerificarEstados();
        ProcurandoJogador();

        Atacar();


        Debug.Log(ataqueCurret);
    }

    public void VerificarEstados()
    {
        switch (modoFauno)
        {
            case FaunoEstado.Flutuando:
                Andar();
                break;
            case FaunoEstado.Descansando:
                break;
            case FaunoEstado.AtaqueFogo:
                AtaqueFogo();
                break;
            case FaunoEstado.AtaqueInvestida:
                AtaqueInvestida();
                break;
            case FaunoEstado.AtaqueTerremoto:
                AtaqueTerremoto();
                break;
        }
    }

    public override void Andar()
    {
        transform.position = new Vector2(Mathf.Cos(Time.time * frequencia) * magnitude, (Mathf.Sin(Time.time * frequencia) * magnitude) + 3f);
    }

    public override void Atacar()
    {        
        if (!podeAtacar)
            return;     

        
        if(ataqueCurret < ataqueMax)
        {
            //Fazer esperar um tempo para randomizar 
           int randomAttack = Random.Range(0, 3);
            VerificarAtaque(randomAttack);
        }
        else if(ataqueCurret >= ataqueMax)
        {
            StartCoroutine(Descansar());
        }
    }

    private void AtaqueFogo()
    {
        ataqueCurret++;     
        Debug.LogWarning("Atacando com a Bola de fogo");        
    }

    private void AtaqueInvestida()
    {
        ataqueCurret++;
        Debug.LogWarning("Atacando com a Investida");        
    }

    private void AtaqueTerremoto()
    {
        ataqueCurret++;
        Debug.LogWarning("Atacando com o Terremoto");        
    }



    public void VerificarAtaque(int value)
    {
        if (value == 0)
            modoFauno = FaunoEstado.AtaqueFogo;
        else if (value == 1)
            modoFauno = FaunoEstado.AtaqueInvestida;
        else
            modoFauno = FaunoEstado.AtaqueTerremoto;
    }

    public override void ProcurandoJogador()
    {
        if (jogadorPosicao != null)
            StartCoroutine(EsperandoAtaque());
    }


    IEnumerator Descansar()
    {
        modoFauno = FaunoEstado.Descansando;
        Debug.Log("Descansando");
        podeAtacar = false;
        yield return new WaitForSeconds(3f);
        ataqueCurret = 0;
        modoFauno = FaunoEstado.Flutuando;
        Debug.Log("Descansado");
        StopCoroutine(Descansar());
    }   
    IEnumerator EsperandoAtaque()
    {
        yield return new WaitForSeconds(3f);
        podeAtacar = true;
        StopCoroutine(EsperandoAtaque());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() != null)
            jogadorPosicao = collision.transform;
    }
}
