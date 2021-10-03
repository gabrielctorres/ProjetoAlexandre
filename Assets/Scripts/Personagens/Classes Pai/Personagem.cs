using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public abstract class Personagem : MonoBehaviour
{
    public int cena;


    protected Rigidbody2D rb2d;
    private Transform posicaoPe;
    protected Animator spriteAnimation;
    private SpriteRenderer sprite;

    public Image vidaImagem;
    public GameObject uiHabilidades;
    public GameObject uiLife;
    public GameObject menuDead;
    public TextMeshProUGUI textReliquias;
    [HideInInspector] public PersonagemSom sons;
    public GameObject menuPause;

    public int numReliquias;
    protected float horizontal;
    private float vertical;
    public float direcaoOlhar = 1f;
    public float vida;
    public float vidaMax;
    public float velocidade;
    public float forcaPulo;
    public float velocidadeParedeDeslize;
    public float dano;

    public bool semArma;
    protected bool atacandoAdaga;
    protected bool olhandoDireita = true;
    protected bool estaNoChao;
    protected bool tocandoNaParede;
    protected bool tocandoNaCorda;
    protected bool segurandoCorda;
    protected bool deslizandoParede;
    protected bool podeAndar = true;
    public bool invulneravel = false;
    protected bool canStun = false;



    private float timeStun = 3f;

    public float TimeStun { get => timeStun; set => timeStun = value; }

    public virtual void Start()
    {
        
        rb2d = GetComponent<Rigidbody2D>();
        posicaoPe = transform.GetChild(0).GetComponent<Transform>();
        spriteAnimation = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        Time.timeScale = 1;
    }
    public virtual void FixedUpdate()
    {
        cena = SceneManager.GetActiveScene().buildIndex;
        SegurarCorda();

        Andar();

        Pular();
     

        MovimentacaoCorda();  
        DetectandoColisão();
    }

    public void Andar()
    {
        if (!podeAndar)
            return;

        if (!segurandoCorda)
        {
            rb2d.gravityScale = 3f;
            horizontal = Input.GetAxis("Horizontal");
            Vector2 dir = new Vector2(horizontal, 0);
            rb2d.velocity = new Vector2(dir.x * velocidade, rb2d.velocity.y);

            if(horizontal != 0 && estaNoChao)
            {
                sons.PlayAndarSom();
            }


        }
        
        
        /*else if (!segurandoCorda || !tocandoNaParede || !deslizandoParede || segurandoParede)
        {
            horizontal = Input.GetAxis("Horizontal");
            Vector2 dir = new Vector2(horizontal, 0);
            rb2d.velocity = Vector2.Lerp(rb2d.velocity, (new Vector2(dir.x * velocidade, rb2d.velocity.y)), 0.5f * Time.deltaTime);
            spriteAnimation.SetBool("Pulando", true);
            spriteAnimation.SetBool("Deslizando", false);
        }*/
    }

    public void OpenPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuPause.activeInHierarchy)
            {
                menuPause.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                menuPause.SetActive(false);
            }
                
        }
    }
    public void Pular()
    {

        if (Input.GetButton("Jump") && estaNoChao && !deslizandoParede && !segurandoCorda && !canStun)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.velocity += Vector2.up.normalized * forcaPulo;
            spriteAnimation.SetBool("Pulando", true);
            sons.PlayPuloSom();
        }
        else if (Input.GetButton("Jump") && !estaNoChao && deslizandoParede && !segurandoCorda)
        {
            Vector2 direcaoPulo = new Vector2(horizontal, 2f);
            Vector2 forca = new Vector2(4.5f * direcaoPulo.x * -direcaoOlhar, 4.5f * direcaoPulo.y);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.velocity += Vector2.up.normalized * forcaPulo;
            sons.PlayPuloSom();
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

        if (Input.GetButton("Interaction") && tocandoNaCorda)
            segurandoCorda = true;
        else if (Input.GetButton("Jump") && estaNoChao ||  !tocandoNaCorda)
            segurandoCorda = false;        
    }

    IEnumerator PararDeMover()
    {
        podeAndar = false;       

        yield return new WaitForSeconds(0.3f);

        transform.localScale = Vector2.one;

        podeAndar = true;


    }

    public void MovimentacaoCorda()
    {
        if (segurandoCorda)
        {
            rb2d.velocity = Vector2.zero;
            vertical = Input.GetAxis("Vertical");
            Vector2 dir = new Vector2(0, vertical);
            rb2d.velocity = new Vector2(rb2d.velocity.x, dir.y * velocidade);
            rb2d.gravityScale = 0;
            spriteAnimation.SetFloat("Vertical", Mathf.Abs(vertical));
        }

    }
    public void DarDano( float  damage)
    {     
        if (vida > 0 && !invulneravel)
        {
            vida -= damage;
            sons.PlayDanoSom();
            StartCoroutine(FicarInvulneravel());
            invulneravel = true;
        }        
    }

    IEnumerator FicarInvulneravel()
    {
        for(float i = 0f; i<1f; i+= 0.1f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        invulneravel = false;
        StopCoroutine(FicarInvulneravel());
    }

    public void VerificarMorte()
    {
        if(vida <= 0)
        {
            this.gameObject.SetActive(false);
            uiHabilidades.SetActive(false);
            uiLife.SetActive(false);
            menuDead.SetActive(true);
            menuDead.GetComponentInChildren<TextMeshProUGUI>().text = "Você esta morto";
            Time.timeScale = 0;
        }
    }
    public void Flip()
    {
        if ((horizontal < 0 && olhandoDireita) && !atacandoAdaga && !tocandoNaParede|| (horizontal > 0 && !olhandoDireita) && !atacandoAdaga && !tocandoNaParede)
        {
            direcaoOlhar *= -1;
            olhandoDireita = !olhandoDireita;
            transform.Rotate(0, 180f, 0f);
        }
    }

    public void ParedeDeslize()
    {
        if (deslizandoParede)
        {
            if (rb2d.velocity.y < -velocidadeParedeDeslize)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -velocidadeParedeDeslize);
            }
        }
    }

    public void DetectandoColisão()
    {
       

        estaNoChao = Physics2D.OverlapCircle(posicaoPe.position, 0.4f, LayerMask.GetMask("Chao"));
        tocandoNaCorda = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.3f), 0.25f, LayerMask.GetMask("Corda"));
        tocandoNaParede = Physics2D.OverlapCircle(new Vector2(transform.position.x + 0.3f, transform.position.y - 0.8f), 0.25f, LayerMask.GetMask("Parede")) ||
                          Physics2D.OverlapCircle(new Vector2(transform.position.x - 0.3f, transform.position.y - 0.8f), 0.25f, LayerMask.GetMask("Parede"));


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
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + 0.3f, transform.position.y - 0.8f), 0.25f);       
    }

   /* void OnGUI()
     {
         GUI.contentColor = Color.green;
         GUI.Label(new Rect(25, 25, 650, 30), "Pode Andar: " + podeAndar);
         GUI.Label(new Rect(25, 65, 650, 30), "Tocando a Parede: " + tocandoNaParede);
         GUI.Label(new Rect(25, 80, 650, 30), "Deslizando Parede: " + deslizandoParede);
         GUI.Label(new Rect(25, 95, 650, 30), "Velocidade: " + rb2d.velocity);
     }*/
    
    public void Stun()
    {
        if (TimeStun >= 1 && canStun)
        {
            podeAndar = false;
            rb2d.velocity = Vector2.zero;
            TimeStun -= Time.deltaTime;
        }
        else
        {
            timeStun = 3f;
            podeAndar = true;
            canStun = false;
        }
    }

    public void TomarStun()
    {
        canStun = true;
    }

    public abstract void Ataque();

    public abstract void SegundoAtaque();    
}
