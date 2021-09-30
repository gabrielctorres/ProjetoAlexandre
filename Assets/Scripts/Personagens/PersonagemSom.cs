using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonagemSom : MonoBehaviour
{
    public List<AudioSource> listaSom = new List<AudioSource>();    
    public void PlayAdagaSom()
    {
        listaSom[0].Play();
        Debug.Log("aaa");
    }

    public void PlayEspadaSom()
    {
        listaSom[1].Play();
    }

    public void PlayDanoSom()
    {
        listaSom[2].Play();
    }

    public void PlayPuloSom()
    {
        listaSom[3].Play();
    }

    public void PlayAndarSom()
    {
        listaSom[4].Play();
    }
    
    public void PlayDashSom()
    {
        listaSom[5].Play();
    }
}
