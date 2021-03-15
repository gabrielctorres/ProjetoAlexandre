using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Personagem : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Transform posicaoPe;

    private float horizontal;
    private float direcaoOlhar = 1f;
    public float vida;
    public float velocidade;
    public float forcaPulo;
    public float velocidadeParedeDeslize;
    public float dano;

    public bool olhandoDireita = true;
    private bool estaNoChao;
    private bool tocandoNaParede;
    private bool deslizandoParede;
    private bool canMove = true;
    public virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        posicaoPe = GetComponentInChildren<Transform>();       
    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {
        DetectandoColisão();
        if (canMove)
        {
            Andar();
        }
         
        Pular();        
    }

    public void Andar()
    {
        horizontal = Input.GetAxis("Horizontal");
        Vector2 dir = new Vector2(horizontal, 0);
        rb2d.velocity = new Vector2(dir.x * velocidade, rb2d.velocity.y);
        Debug.Log(deslizandoParede);

        if (deslizandoParede)
        {
            if (rb2d.velocity.y < 0)
            {
                Debug.Log("aaaaaaaaaaaaaaaaaaaaaa");
                rb2d.velocity = new Vector2(rb2d.velocity.x, -velocidadeParedeDeslize);
            }
        }
      


        if ((horizontal < 0 && olhandoDireita) || (horizontal > 0 && !olhandoDireita))
        {
            Flip();
        }

    }
    
    public void Pular()
    {
        
        
        if (Input.GetButton("Jump") && estaNoChao && !deslizandoParede)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(rb2d.velocity.x,forcaPulo));
        }else if (Input.GetButton("Jump") && !estaNoChao && deslizandoParede)
        {            
            Vector2 direcaoPulo = new Vector2(horizontal, 1.8f);
            Vector2 forca = new Vector2(4f * direcaoPulo.x * -direcaoOlhar, 4f * direcaoPulo.y);
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(forca, ForceMode2D.Impulse);

            StartCoroutine(nameof(PararDeMover));
        }
    }

   
    IEnumerator PararDeMover()
    {
        canMove = false;

        transform.localScale = transform.localScale.x == 1 ? new Vector2(-1f, 1) : Vector2.one;

        yield return new WaitForSeconds(0.5f);

        transform.localScale = Vector2.one;

        canMove = true;

        
    }


    public void Flip()
    {
        direcaoOlhar *= -1;
        olhandoDireita = !olhandoDireita;
        transform.Rotate(0, 180f, 0f);
    }

    public void DetectandoColisão()
    {
        
        estaNoChao = Physics2D.OverlapCircle(posicaoPe.position, 0.5f, LayerMask.GetMask("Chao"));

        tocandoNaParede = Physics2D.Raycast(transform.position, new Vector2(direcaoOlhar,0), 0.4f, LayerMask.GetMask("Parede"));
        Debug.DrawRay(transform.position, new Vector2(direcaoOlhar, 0) * 0.4f, Color.green);

        if (tocandoNaParede && !estaNoChao && rb2d.velocity.y < 0 && horizontal != 0)
            deslizandoParede = true;
        else
            deslizandoParede = false;

    }

    void OnGUI()
    {
        GUI.contentColor = Color.green;
        GUI.Label(new Rect(25, 25, 650, 30), "Pode Andar: " + canMove);
        GUI.Label(new Rect(25, 40, 650, 30), "Ta no chao: " + estaNoChao);
        GUI.Label(new Rect(25, 65, 650, 30), "Tocando a Parede: " + tocandoNaParede);
        GUI.Label(new Rect(25, 80, 650, 30), "Deslizando Parede: " + deslizandoParede);
        GUI.Label(new Rect(25, 95, 650, 30), "Velocidade: " + rb2d.velocity);
    }

    public abstract void Ataque();

}
