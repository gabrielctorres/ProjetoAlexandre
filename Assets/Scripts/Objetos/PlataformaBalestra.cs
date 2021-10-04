using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaBalestra : MonoBehaviour
{
    float posX;    
    float posY;
    float oldY;
    [Header("Definições da balestra correspondente")]
    public GameObject projetil;
    public Transform balestra;
    public Transform spawnProjetil;
    GameObject projetilInstanciado = null;

    bool canSpawn;

    public int offSetEffector;
    // Start is called before the first frame update
    void Start()
    {
        posX = GetComponent<Transform>().transform.position.x;        
        posY = GetComponent<Transform>().transform.position.y;        
    }

    // Update is called once per frame
    void Update()
    {
        if (projetilInstanciado == null)
            canSpawn = true;
        else
            canSpawn = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            oldY = transform.position.y;
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(posX, posY-0.1f));

            if (canSpawn)
            {              
                if (projetilInstanciado == null)
                    projetilInstanciado = Instantiate(projetil, spawnProjetil.position, balestra.rotation);

                projetilInstanciado.GetComponent<Rigidbody2D>().velocity = (spawnProjetil.position - balestra.transform.position) * 50f;
                projetilInstanciado.GetComponent<PlatformEffector2D>().rotationalOffset = offSetEffector;
            }
            Destroy(projetilInstanciado, 5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(posX, oldY));
        }            
    }
}
