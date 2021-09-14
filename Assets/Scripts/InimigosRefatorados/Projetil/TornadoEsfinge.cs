using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoEsfinge : MonoBehaviour
{
    public GameObject prefabEnemy;
    public List<Vector2> spawnPosition = new List<Vector2>();

    private Rigidbody2D rbPersonagem;

    public float force;

    public Rigidbody2D RbPersonagem { get => rbPersonagem; set => rbPersonagem = value; }

    void Start()
    {           
        InvokeRepeating(nameof(SpawnEscaravelhos), 1, 1f);
    }

    // Update is called once per frame
    void Update()
    {
    
        if (rbPersonagem != null)
        {
            Gravidade();
        }
    }

    private void FixedUpdate()
    {

    }

    public void Gravidade()
    {
        Vector2 tornadoPos = new Vector2(transform.position.x, 0f);
        Vector2 personagemPos = new Vector2(rbPersonagem.transform.position.x, 0f);
        rbPersonagem.AddRelativeForce(new Vector2(tornadoPos.x - personagemPos.x, 0f) * force);
    }


    public void SpawnEscaravelhos()
    {
        for (int i = 0; i < spawnPosition.Count; i++)
        {
            GameObject enemyInstanciado = Instantiate(prefabEnemy, spawnPosition[i], Quaternion.identity);            
            enemyInstanciado.GetComponent<Escaravelho>().Target = this.transform;
            enemyInstanciado.GetComponent<Escaravelho>().CanAttack = true;
            enemyInstanciado.transform.parent = this.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Escaravelho>() != null)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPosition[0], 0.6f);
        Gizmos.DrawWireSphere(spawnPosition[1], 0.6f);
        Gizmos.DrawWireSphere(spawnPosition[2], 0.6f);
        Gizmos.DrawWireSphere(spawnPosition[3], 0.6f);
        Gizmos.DrawWireSphere(spawnPosition[4], 0.6f);
        Gizmos.DrawWireSphere(spawnPosition[5], 0.6f);
    }

}
