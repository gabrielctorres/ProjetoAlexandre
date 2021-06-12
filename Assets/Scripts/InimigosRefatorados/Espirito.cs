using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espirito : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            collision.GetComponent<Personagem>().DarDano(1f);
            Destroy(this.gameObject);
        }
    }
}
