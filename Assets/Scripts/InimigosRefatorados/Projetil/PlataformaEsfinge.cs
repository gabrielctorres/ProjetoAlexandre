using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovimentType
{
    MoveHorizontal,
    MoveVertical,
    Stop,
    Both
}

public enum AttackType
{
    Laser,
    Espinho
}

public class PlataformaEsfinge : MonoBehaviour
{
    [SerializeField] private MovimentType movimentType;
    [SerializeField] private AttackType attackType;

    public float timer = 6f;

    bool triggerEspinho;

    private Vector2 pointA, pointB;
    private float velocidade;
    private Rigidbody2D rb2d;
    private Animator spriteAnimator;

    private float posValue;

    public Vector3 target;


    public MovimentType MovimentType { get => movimentType; set => movimentType = value; }
    public AttackType AttackType { get => attackType; set => attackType = value; }
    public float Velocidade { get => velocidade; set => velocidade = value; }
    public Vector2 PointB { get => pointB; set => pointB = value; }
    public Vector2 PointA { get => pointA; set => pointA = value; }
    public float PosValue { get => posValue; set => posValue = value; }

    void Start()
    {               
        if(movimentType != MovimentType.Both)
            spriteAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        VerifyState();
    }

    public void VerifyState()
    {
        switch (MovimentType)
        {
            case MovimentType.MoveHorizontal:
                HorizontalMove();
                break;
            case MovimentType.MoveVertical:
                VerticalMove();
                break;
            case MovimentType.Both:
                BothMove();
                break;
            case MovimentType.Stop:                
                break;
            default:
                break;
        }

        switch (attackType)
        {
            case AttackType.Laser:
                Laser();
                break;
            case AttackType.Espinho:
                Espinho();
                break;
            default:
                break;
        }
    }


    public void Laser()
    {
        spriteAnimator.SetBool("isNormal", false); 
    }

   public void Espinho()
    {
        if(spriteAnimator != null)
            spriteAnimator.SetBool("isNormal", true);

        if (triggerEspinho)
        {
            if(timer > 0)
                timer -= Time.deltaTime;
            else
            {
                spriteAnimator.SetBool("espinho", true);
                timer = 0f;
                triggerEspinho = false;
            }
              
        }       
    }

    public void HorizontalMove()
    {
        transform.position = Vector2.Lerp(new Vector2(pointA.x, posValue), new Vector2(pointB.x, posValue), Mathf.PingPong(Time.time * velocidade,1f));
    }

    public void VerticalMove()
    {
        transform.position = Vector2.Lerp(new Vector2(posValue, pointA.y), new Vector2(posValue, pointB.y), Mathf.PingPong(Time.time * velocidade, 1f));
    }

    public void BothMove()
    {
        transform.Translate(target * velocidade * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<Personagem>() != null)
        {
            collision.collider.transform.parent = this.transform;
            triggerEspinho = true;
            Debug.Log(triggerEspinho);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.GetComponent<Personagem>() != null)
        {
            if (attackType == AttackType.Espinho)
                Debug.Log("Dano");
            else
            {
                spriteAnimator.SetBool("laser", true);
                Debug.Log("Dano");
            }
        }
       
        
        if (collision.GetComponent<PlataformaEsfinge>() != null && collision.GetComponent<PlataformaEsfinge>().attackType == AttackType.Espinho)
        {        
            if(this.attackType == AttackType.Laser)
            {
                spriteAnimator.SetBool("laser", true);

                MaterialPropertyBlock block = new MaterialPropertyBlock();

                Renderer render = GetComponent<Renderer>();
                render.GetPropertyBlock(block);
                block.SetFloat("_laserValue", 0.5f);
                render.SetPropertyBlock(block);
                

                this.transform.GetChild(2).GetComponent<BoxCollider2D>().size = new Vector2(0.4f, 1f);
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlataformaEsfinge>() != null)
        {
            if (collision.GetComponent<PlataformaEsfinge>().attackType == AttackType.Espinho)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();

                Renderer render = GetComponent<Renderer>();
                render.GetPropertyBlock(block);
                block.SetFloat("_laserValue", 0f);
                render.SetPropertyBlock(block);            

                this.transform.GetChild(2).GetComponent<BoxCollider2D>().size = new Vector2(0.4f, 6f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Personagem>() != null)
        {
            collision.collider.transform.parent = null;            
        }
    }
}
