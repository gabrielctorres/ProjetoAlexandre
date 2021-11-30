using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laranja : MonoBehaviour
{    
    private  Personagem personagemScript;

    public GameObject effectRegen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() !=null )
        {
            personagemScript = collision.GetComponent<Personagem>();
            GameObject effectInstnaciado =  Instantiate(effectRegen, new Vector2(collision.transform.position.x,(collision.transform.position.y -1.19f)), Quaternion.identity, collision.transform);
            effectInstnaciado.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DelayedBrokenObject(0.2f));           
        }
    }

    IEnumerator DelayedBrokenObject(float _delay)
    {
        if (personagemScript.vida < 5)
        {
            float diferenca = 5f - personagemScript.vida;

            if (diferenca >= 2f)
            {
                personagemScript.vida += 2f;
            }
            else
            {
                personagemScript.vida += diferenca;
            }
        }        
        Debug.Log("Vida: " + personagemScript.vida);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(_delay);        
        Destroy(this.gameObject);
    }
}
