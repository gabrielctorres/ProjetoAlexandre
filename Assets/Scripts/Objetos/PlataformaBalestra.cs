using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaBalestra : MonoBehaviour
{
    float posX;    
    float posY;   

    [Header("Definições da balestra correspondente")]
    public GameObject projetil;
    public Transform spawnProjetil;
    public int direcaoOlhar;

    // Start is called before the first frame update
    void Start()
    {
        posX = GetComponent<Transform>().transform.position.x;        
        posY = GetComponent<Transform>().transform.position.y;
        direcaoOlhar = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(posX, posY-0.1f));
            GameObject projetilInstanciado = Instantiate(projetil, spawnProjetil.position, transform.rotation);
            projetilInstanciado.GetComponent<Rigidbody2D>().velocity = transform.right * -10f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Rigidbody2D>().MovePosition(new Vector2(posX, posY + 0.1f));        
    }
}
