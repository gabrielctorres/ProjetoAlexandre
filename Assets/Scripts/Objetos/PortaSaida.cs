using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaSaida : MonoBehaviour
{
    bool podeEntrar = false;

    public GameObject botaoInteracao;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("F") && podeEntrar)
        {
            this.GetComponent<ChangeScene>().Change();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>()!= null)
        {
            podeEntrar = true;
            botaoInteracao.SetActive(true);
        }
    }
}
