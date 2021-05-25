using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espinho : MonoBehaviour
{
    Personagem personagem;
    // Start is called before the first frame update
    void Start()
    {
        personagem = GameObject.Find("Personagem").GetComponent<Personagem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Personagem")
        {
            personagem.vida = 0;
        }
    }
}
