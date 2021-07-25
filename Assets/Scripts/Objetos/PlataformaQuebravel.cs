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

    IEnumerator Quebrar()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
