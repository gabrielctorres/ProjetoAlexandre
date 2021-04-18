using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GuardaController : MonoBehaviour
{
    private Transform posicaoDoJogador;
    public GameObject guarda;  


    
    // Start is called before the first frame update
    void Start()
    {
        posicaoDoJogador = GameObject.Find("Personagem").GetComponent<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (posicaoDoJogador.position.x >= 77f && !posicaoDoJogador.GetComponent<Personagem>().semArma && guarda != null)
        {
            if(!guarda.activeInHierarchy)
                guarda.SetActive(true);          
        }

        if(guarda == null)
        {
            SceneManager.LoadScene("CenaFase1", LoadSceneMode.Single);
        }


    }
}
