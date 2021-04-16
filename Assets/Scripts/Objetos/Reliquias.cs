using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reliquias : MonoBehaviour
{
    public static int numReliquias;
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
        if (collision.gameObject.tag == "Player")
        {
            numReliquias++;
            Debug.Log("Relíquias coletadas: " + numReliquias);
            Destroy(this.gameObject);
        }        
    }
}
