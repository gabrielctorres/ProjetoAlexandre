using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosQuebraveis : MonoBehaviour
{
    public Animator spriteAnimation;   

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.GetComponentInParent<Animator>() !=null)
            spriteAnimation = this.gameObject.GetComponentInParent<Animator>();
    }

    //spriteAnimation.SetBool("Pulando", false);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.CompareTag("Tabua"))
        {
            if (collision.gameObject.name == "Personagem")
            {
                spriteAnimation.SetTrigger("quebrou");                
                StartCoroutine(DelayedBrokenObject(1.8f));
            }
        }
    }
    public void Destroi()
    {
        StartCoroutine(DelayedBrokenObject(1));
    }

    IEnumerator DelayedBrokenObject(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (this.CompareTag("Tabua"))
            this.gameObject.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
        else
        {
           if(this.gameObject.GetComponentInParent<LootScript>() != null) this.gameObject.GetComponentInParent<LootScript>().calculandoLoot();
        }
        Destroy(transform.parent.gameObject);
    }
}
