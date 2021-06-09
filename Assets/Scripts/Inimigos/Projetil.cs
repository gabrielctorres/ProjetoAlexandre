using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    Transform posicaoColisor;
    public float raioColisor;
    public LayerMask hitMask;
    // Start is called before the first frame update
    void Start()
    {
        posicaoColisor = transform.GetChild(0).transform;        
    }

    // Update is called once per frame
    void Update()
    {
        colidirComJogador();
    }

    void colidirComJogador()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(posicaoColisor.position, raioColisor, hitMask);

        //Passando por cada inimigo e aplicando dano e aplicando força
        foreach (Collider2D objeto in hitEnemies)
        {
            Debug.Log(objeto.name);
            if (!objeto.GetComponent<Personagem>().invulneravel)
            {
                StartCoroutine(objeto.GetComponent<Personagem>().Stun());                
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        posicaoColisor = transform.GetChild(0).transform;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoColisor.position, raioColisor);
    }
}
