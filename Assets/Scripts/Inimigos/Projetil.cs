using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    Transform posicaoColisor;
    public float raioColisor;      

    // Start is called before the first frame update
    void Start()
    {
        posicaoColisor = transform.GetChild(0).transform;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void colidirComJogador()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(posicaoColisor.position, raioColisor, LayerMask.GetMask("Jogador"));

        //Passando por cada inimigo e aplicando dano e aplicando força
        foreach (Collider2D objeto in hitEnemies)
        {
            if (!objeto.GetComponent<Personagem>().invulneravel)
            {
                            
            }
        }
    }    
}
