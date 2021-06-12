using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaFogo : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 jogadorPosition;
    void Start()
    {
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, jogadorPosition, 10f * Time.deltaTime);
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
