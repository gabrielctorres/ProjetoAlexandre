using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estalaquitite : MonoBehaviour
{


    public void StartDestroy()
    {
        Destroy(gameObject, 15f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            collision.GetComponent<Personagem>().DarDano(0.5f);
            Destroy(this.gameObject);
        }
           
    }

}
