using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private Animator bandeiraAnimator;

    private void Start()
    {
        bandeiraAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            SaveSystem.SavePlayer(collision.GetComponent<Personagem>());
            bandeiraAnimator.SetBool("subirBandeira",true);
        }
    }

}
