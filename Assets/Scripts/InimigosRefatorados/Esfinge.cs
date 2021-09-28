using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Esfinge : EntidadeBase
{    
    [Header("Controle dos ataques", order = 1)]
    public Queue<string> ataques = new Queue<string>();
    public int ataqueMax = 3;

    private bool vendoPlayer;  

    private float atktempoderecarga = 0;
    public float tempoMaximoDescansado = 15f;
    private float tempoDescansado;
    private int maxPlataform = 2;


    public GameObject prefabPlataformaVertical;
    public GameObject prefabPlataforma;
    public GameObject prefabTornado;

    public List<GameObject> plataformas = new List<GameObject>();

    private Animator spriteAnimator;

    public CinemachineVirtualCamera cam;

    [Header("Interface")]
    public GameObject bossLife;
    public Image bossImage;
    public TextMeshProUGUI nameText;
    public GameObject menuDead;

    private void Start()
    {
        tempoDescansado = tempoMaximoDescansado;
        spriteAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        VerifyState();
        ControlandoVida();
    }

    public void ControlandoVida()
    {
        bossImage.fillAmount = vida / vidaMax;

        if (vida <= 0)
        {
            menuDead.SetActive(true);
            menuDead.GetComponentInChildren<TextMeshProUGUI>().text = "Obrigado por testar nosso jogo, não esqueça de responder o  formulario";
            Time.timeScale = 0;
        }   
    }


    public override void VerifyState()
    {
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                EsperandoAtaque();
                break;
            case EnemyState.Attacking:
                Atacar();
                break;
            case EnemyState.Resting:
                Descansando();
                break;
            default:
                break;
        }
    }

    public override void Atacar()
    {
        if (atktempoderecarga <= 0)
        {
            plataformas.Clear();
            if (ataques.Count > 0)
            {
                
                StartCoroutine(ataques.Dequeue());
                atktempoderecarga = 5;
            }
            else
                enemyState = EnemyState.Resting;
        }
        else
        {
            atktempoderecarga -= Time.deltaTime;
        }
    }


    public IEnumerator AttackLaser()
    {        
        if (plataformas.Count < maxPlataform)
        {
            GameObject plataformaLaser = Instantiate(prefabPlataforma, new Vector2(pointA.x,101f), Quaternion.identity);
            GameObject plataformaSegura = Instantiate(prefabPlataforma, new Vector2(pointB.x, pointA.y - 3f), Quaternion.identity);
            plataformaLaser.GetComponent<PlataformaEsfinge>().PosValue = pointA.y;
            plataformaSegura.GetComponent<PlataformaEsfinge>().PosValue = pointB.x;
            plataformas.Add(plataformaLaser);
            plataformas.Add(plataformaSegura);
            SetandoPlataforma(0, 0.6f, new Vector2(pointA.x - 6f, 101f), new Vector2(pointB.x + 5f, 101f), MovimentType.MoveHorizontal, AttackType.Laser);
            SetandoPlataforma(1, 0.6f, new Vector2(pointA.x, pointA.y + 2f), new Vector2(pointB.x + 5f, pointB.y), MovimentType.Stop, AttackType.Espinho);

            Destroy(plataformaLaser, 5f);
            Destroy(plataformaSegura, 5f);


            yield return new WaitForSeconds(0.9f);
        }          

        spriteAnimator.SetBool("laser", true);
        yield return new WaitForSeconds(6f);
        spriteAnimator.SetBool("laser", false);        
    }

    public IEnumerator AttackTornado()
    {
        if (plataformas.Count < maxPlataform)
        {
            GameObject tornado = Instantiate(prefabTornado, new Vector2(transform.position.x, 98.46f), Quaternion.identity);
            tornado.GetComponent<TornadoEsfinge>().RbPersonagem = Target.GetComponent<Rigidbody2D>();
            plataformas.Add(tornado);
            Destroy(tornado, 5f);
            yield return new WaitForSeconds(0.9f);
        }             
        spriteAnimator.SetBool("tornado", true);
        yield return new WaitForSeconds(6f);
        spriteAnimator.SetBool("tornado", false);
    }

    public IEnumerator AttackWall()
    {
        if (plataformas.Count < maxPlataform)
        {
            GameObject plataformaEspinho = Instantiate(prefabPlataformaVertical, new Vector2(pointA.x, 98.22f), Quaternion.identity);
            plataformaEspinho.GetComponent<PlataformaEsfinge>().PosValue = pointA.y;
            plataformaEspinho.GetComponent<PlataformaEsfinge>().target = Vector3.right;

            GameObject plataformaEspinho2 = Instantiate(prefabPlataformaVertical, new Vector2(pointB.x, 98.22f), Quaternion.identity);
            plataformaEspinho2.GetComponent<PlataformaEsfinge>().PosValue = pointA.y;
            plataformaEspinho2.GetComponent<PlataformaEsfinge>().target = Vector3.left;

            Animator[] animators = plataformaEspinho.GetComponentsInChildren<Animator>();

            plataformas.Add(plataformaEspinho);
            plataformas.Add(plataformaEspinho2);

            Destroy(plataformaEspinho, 5f);
            Destroy(plataformaEspinho2, 5f);

            SetandoPlataforma(0, 2f, Vector2.zero, Vector2.zero, MovimentType.Target, AttackType.Espinho);
            SetandoPlataforma(1, 2f, Vector2.zero, Vector2.zero, MovimentType.Target, AttackType.Espinho);
            SetandoVerticalSkill(0, plataformaEspinho.GetComponentsInChildren<Animator>());
            SetandoVerticalSkill(1, plataformaEspinho2.GetComponentsInChildren<Animator>());

            yield return new WaitForSeconds(0.9f);
        }
        spriteAnimator.SetBool("parede", true);
        yield return new WaitForSeconds(6f);
        spriteAnimator.SetBool("parede", false);     
    }


    public void RandomizarAtaque()
    {
        if (ataques.Count >= ataqueMax)
            return;
        for (int i = 0; i < ataqueMax; i++)
        {
            int random = Random.Range(1, 4);

            if (random == 1)
                ataques.Enqueue("AttackLaser");
            else if (random == 2)
                ataques.Enqueue("AttackTornado");
            else if(random == 3)
                ataques.Enqueue("AttackWall");            
        }

    }
    public void EsperandoAtaque()
    {
        if (vendoPlayer)
        {
            StartCoroutine(Esperar());
        }
        else
            return;
    }
    
    public void Descansando()
    {
        if (tempoDescansado <= 0)
        {            
            RandomizarAtaque();
            enemyState = EnemyState.Attacking;
            tempoDescansado = tempoMaximoDescansado;
        }
        else
        {
            tempoDescansado -= Time.deltaTime;            
            if (plataformas.Count < maxPlataform)
            {
                GameObject plataformaEspinho = Instantiate(prefabPlataforma, pointA, Quaternion.identity);
                GameObject plataformaEspinho2 = Instantiate(prefabPlataforma, pointB, Quaternion.identity);
                plataformaEspinho.GetComponent<PlataformaEsfinge>().PosValue = pointA.x;
                plataformaEspinho2.GetComponent<PlataformaEsfinge>().PosValue = pointB.x;
                plataformas.Add(plataformaEspinho);
                plataformas.Add(plataformaEspinho2);

                Destroy(plataformaEspinho, tempoMaximoDescansado);
                Destroy(plataformaEspinho2, tempoMaximoDescansado);

                SetandoPlataforma(0, 0.6f, pointA, pointB, MovimentType.MoveVertical, AttackType.Espinho);
                SetandoPlataforma(1, 0.6f, pointA, pointB, MovimentType.MoveVertical, AttackType.Espinho);
            }               
        }
    }

    IEnumerator Esperar()
    {
        yield return new WaitForSeconds(5f);
        RandomizarAtaque();
        enemyState = EnemyState.Attacking;
        StopCoroutine(Esperar());
    }

    #region ControlePlataformas
    public void RemoveToList()
    {

        foreach (GameObject plataforma in plataformas)
        {
            Destroy(plataforma);
        }
        plataformas.Clear();

    }

    public void SetandoVerticalSkill(int index, Animator[] animators)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool("isNormal", true);
        }
    }

    public void SetandoPlataforma(int index, float velocidade, Vector2 aPoint, Vector2 bPoint, MovimentType moveType, AttackType attackType)
    {
        if(index < plataformas.Count && plataformas[index] != null)
        {
            plataformas[index].GetComponent<PlataformaEsfinge>().MovimentType = moveType;
            plataformas[index].GetComponent<PlataformaEsfinge>().AttackType = attackType;
            plataformas[index].GetComponent<PlataformaEsfinge>().Velocidade = velocidade;
            plataformas[index].GetComponent<PlataformaEsfinge>().PointA = aPoint;
            plataformas[index].GetComponent<PlataformaEsfinge>().PointB = bPoint;
        }
    } 
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() != null)
        {
            cam.m_Lens.OrthographicSize = 9;
            bossLife.SetActive(true);
            nameText.text = this.gameObject.name;
            vendoPlayer = true;
            Target = collision.transform;
            this.GetComponent<CircleCollider2D>().radius = 3.5f;
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointA, 0.6f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointB, 0.6f);
    }



    public override void Andar()
    {
        throw new System.NotImplementedException();
    }

}
