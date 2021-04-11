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
            }
        }
        if (this.CompareTag("Tabua"))
        {
            if (collision.gameObject.name == "Personagem")
            {
                spriteAnimation.SetTrigger("quebrou");
                StartCoroutine(DelayedBrokenObject(1.5f));
            }
        }
    }

    IEnumerator DelayedBrokenObject(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        Destroy(transform.parent.gameObject);
    }
}
