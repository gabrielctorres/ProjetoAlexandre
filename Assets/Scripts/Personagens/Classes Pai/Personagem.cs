using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Personagem : MonoBehaviour
{
    public float vida;
    public float velocidade;
    public float forcaPulo;
    public float dano;
    private Rigidbody2D rb2d;
    private FloorColision colisaoChao;

    public virtual void Start()
    {
        colisaoChao = GetComponentInChildren<FloorColision>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public virtual void FixedUpdate()
    {
        Andar();
        Pular();
    }

    public void Andar()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 dir = new Vector2(horizontal,0);
        dir.Normalize();
        rb2d.velocity = new Vector2(dir.x * velocidade, rb2d.velocity.y);
    }

    public void Pular()
    {
        if (Input.GetButtonDown("Jump") && colisaoChao.estaNoChao)
        {
            rb2d.AddForce(new Vector2(rb2d.velocity.x, forcaPulo), ForceMode2D.Impulse);
        }
    }

    public abstract void Ataque();

}
