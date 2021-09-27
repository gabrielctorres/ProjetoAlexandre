using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    Rigidbody2D rb2d;
    BoxCollider2D box2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        box2d.isTrigger = true;
        box2d.usedByEffector = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ParedeFlecha"))
        {
            box2d.isTrigger = false;
            box2d.usedByEffector = true;
            gameObject.layer = LayerMask.NameToLayer("Chao");
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Static;
        }

        if(collision.GetComponent<Personagem>() != null)
        {
            collision.GetComponent<Personagem>().DarDano(1f);
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(collision.GetComponent<Personagem>().direcaoOlhar * -1, 0) * 1.2f, ForceMode2D.Impulse);
        }

    }
}
