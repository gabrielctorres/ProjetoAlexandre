using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaFogo : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 jogadorPosition;

    public AudioSource starSound;
    void Start()
    {
        starSound.Play();
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Chao") || collision.CompareTag("Parede"))
        {
            this.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
        }

        if(collision.GetComponent<Personagem>() != null)
        {
            collision.GetComponent<Personagem>().DarDano(1f);
            this.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
        }
    }
}
