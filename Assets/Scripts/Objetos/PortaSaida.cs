using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaSaida : MonoBehaviour
{
    bool podeEntrar = false;
    bool canCollision = false;

    public GameObject guarda;


    public GameObject botaoInteracao;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(guarda == null)
        {
            canCollision = true;
        }

        if (Input.GetButtonDown("Interaction") && podeEntrar)
        {
            this.GetComponent<ChangeScene>().Change();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>()!= null && canCollision)
        {
            podeEntrar = true;
            botaoInteracao.SetActive(true);
        }
    }
}
