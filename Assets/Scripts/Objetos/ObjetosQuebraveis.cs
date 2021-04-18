using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosQuebraveis : MonoBehaviour
{
    protected Animator spriteAnimation;   

    // Start is called before the first frame update
    void Start()
    {
        spriteAnimation = this.gameObject.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //spriteAnimation.SetBool("Pulando", false);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.CompareTag("ObjetoQuebravelSimples"))
        {
            if (collision.gameObject.name == "Adaga")
            {
                spriteAnimation.SetTrigger("quebrou");
                StartCoroutine(DelayedBrokenObject(1));
                this.gameObject.GetComponentInParent<LootScript>().calculandoLoot();
            }
        }

        if (this.CompareTag("Tabua"))
        {
            if (collision.gameObject.name == "Personagem")
            {
                spriteAnimation.SetTrigger("quebrou");                
                StartCoroutine(DelayedBrokenObject(1.8f));
            }
        }
        if (this.CompareTag("CaixaForte"))
        {
            if (collision.gameObject.name == "Espada")
            {
                spriteAnimation.SetTrigger("quebrou");
                StartCoroutine(DelayedBrokenObject(1));
                this.gameObject.GetComponentInParent<LootScript>().calculandoLoot();
            }
        }
    }

    IEnumerator DelayedBrokenObject(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if(this.CompareTag("Tabua"))
            this.gameObject.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
        Destroy(transform.parent.gameObject);
    }
}
