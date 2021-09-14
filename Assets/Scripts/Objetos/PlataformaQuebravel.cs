using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaQuebravel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Quebrar());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Reativar());
        }
    }

    IEnumerator Quebrar()
    {
        yield return new WaitForSeconds(1.5f);

        if (gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (gameObject.GetComponent<BoxCollider2D>().enabled)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        } 
    }

    IEnumerator Reativar()
    {
        yield return new WaitForSeconds(3f);

        if (gameObject.GetComponent<SpriteRenderer>().enabled == false)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (gameObject.GetComponent<BoxCollider2D>().enabled == false)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
