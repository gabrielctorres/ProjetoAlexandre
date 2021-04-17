using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Personagem : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Transform posicaoPe;
    protected Animator spriteAnimation;

    public Image vidaImagem;
    public GameObject uiHabilidades;
    private float horizontal;
    private float direcaoOlhar = 1f;
    public float vida;
    public float vidaMax;
    public float velocidade;
    public float forcaPulo;
    public float velocidadeParedeDeslize;
    public float dano;

    public bool semArma;
    protected bool olhandoDireita = true;
    protected bool estaNoChao;
    protected bool tocandoNaParede;
    protected bool tocandoNaCorda;
    protected bool segurandoCorda;
    protected bool deslizandoParede;
    protected bool podeAndar = true;

    public virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        posicaoPe = transform.GetChild(0).GetComponent<Transform>();
        spriteAnimation = GetComponent<Animator>();
    }




    public virtual void FixedUpdate()
    {
        SegurarCorda();
        if (podeAndar)
            Andar();

        Pular();


        MovimentacaoCorda();  
        DetectandoColisão();

        if (!deslizandoParede || !tocandoNaParede || estaNoChao)
            spriteAnimation.SetFloat("Horizontal", Mathf.Abs(horizontal));

    }

    public void Andar()
    {


        if (!segurandoCorda)
        {
            rb2d.gravityScale = 3f;
            horizontal = Input.GetAxis("Horizontal");
            Vector2 dir = new Vector2(horizontal, 0);
            rb2d.velocity = new Vector2(dir.x * velocidade, rb2d.velocity.y);
        }     

        if (deslizandoParede)
        {
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -velocidadeParedeDeslize);
            }
        }       

    }
    
    public void Pular()
    {
       

        if (Input.GetButton("Jump") && estaNoChao && !deslizandoParede && !segurandoCorda)
        {           
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.velocity += Vector2.up.normalized * forcaPulo;
            spriteAnimation.SetBool("Pulando", true);

        }
        else if (Input.GetButton("Jump") && !estaNoChao && deslizandoParede && !segurandoCorda)
        {
            Vector2 direcaoPulo = new Vector2(horizontal, 2f);
            Vector2 forca = new Vector2(4.5f * direcaoPulo.x * -direcaoOlhar, 4.5f * direcaoPulo.y);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.velocity += Vector2.up.normalized * forca;
            spriteAnimation.SetBool("Pulando", true);
            StartCoroutine(nameof(PararDeMover));
        }
        else
        {
            spriteAnimation.SetBool("Pulando", false);
        }
       
    }

    public void SegurarCorda()
    {
        spriteAnimation.SetBool("PegouCorda", segurandoCorda);

        if (Input.GetButton("PrimeiroAtaque") && tocandoNaCorda)
            segurandoCorda = true;
        else if (Input.GetButton("Jump") && estaNoChao ||  !tocandoNaCorda)
            segurandoCorda = false;            

    }


    public void MovimentacaoCorda()
    {
        if (segurandoCorda)
        {
            rb2d.velocity = Vector2.zero;
            float vertical = Input.GetAxis("Vertical");
            Vector2 dir = new Vector2(0, vertical);
            rb2d.velocity = new Vector2(rb2d.velocity.x, dir.y * velocidade);
            rb2d.gravityScale = 0;
            spriteAnimation.SetFloat("Vertical", Mathf.Abs(vertical));
        }

    }
   
    IEnumerator PararDeMover()
    {
        podeAndar = false;

        transform.localScale = transform.localScale.x == 1 ? new Vector2(-1f, 1) : Vector2.one;

        yield return new WaitForSeconds(0.3f);

        transform.localScale = Vector2.one;

        podeAndar = true;

        
    }


    public void DarDano( float  damage)
    {
        if (vida >= 0)
            vida -= damage;
    }

    public void VerificarMorte()
    {
        if(vida <= 0)
        {
            //Abrir menu para reiniciar o jogo
        }
    }
    public void Flip()
    {
        if ((horizontal < 0 && olhandoDireita) && !tocandoNaParede || (horizontal > 0 && !olhandoDireita) && !tocandoNaParede)
        {
            direcaoOlhar *= -1;
            olhandoDireita = !olhandoDireita;
            transform.Rotate(0, 180f, 0f);
        }
    }

    public void DetectandoColisão()
    {
        

        estaNoChao = Physics2D.OverlapCircle(posicaoPe.position, 0.4f, LayerMask.GetMask("Chao"));
        tocandoNaCorda = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.3f), 0.25f, LayerMask.GetMask("Corda"));
        tocandoNaParede = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - 0.9f), transform.right, 0.65f, LayerMask.GetMask("Parede"));
       
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.9f), transform.right * 0.35f, Color.red);

        if (tocandoNaParede && !estaNoChao && rb2d.velocity.y <= 0 && horizontal != 0 && !segurandoCorda)
            deslizandoParede = true;
        else
            deslizandoParede = false;
        spriteAnimation.SetBool("Deslizando", deslizandoParede);
    }

    private void OnDrawGizmos()
    {
        Vector3 centerObject = new Vector3(transform.position.x, transform.position.y - 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerObject, 0.25f);
    }

   /* void OnGUI()
    {
        GUI.contentColor = Color.green;
        GUI.Label(new Rect(25, 25, 650, 30), "Pode Andar: " + podeAndar);
        GUI.Label(new Rect(25, 40, 650, 30), "Ta no chao: " + estaNoChao);
        GUI.Label(new Rect(25, 65, 650, 30), "Tocando a Parede: " + tocandoNaParede);
        GUI.Label(new Rect(25, 80, 650, 30), "Deslizando Parede: " + deslizandoParede);
        GUI.Label(new Rect(25, 95, 650, 30), "Velocidade: " + rb2d.velocity);
    }*/

    public abstract void Ataque();

    public abstract void SegundoAtaque();

    


    
}
