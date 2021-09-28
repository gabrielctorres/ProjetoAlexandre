using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class LightController : MonoBehaviour
{

    public Light2D luz;

    bool canChange = false;

    private void Update()
    {
        if (canChange)
        {
            if (luz.intensity >= 0.3)
                luz.intensity -= 0.01f;
        }
        else
        {
           if (luz.intensity <= 1)
               luz.intensity += 0.01f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Personagem>() != null)
        {
            if (canChange == false)
                canChange = true;
            else
                canChange = false;
        }

    }
}
