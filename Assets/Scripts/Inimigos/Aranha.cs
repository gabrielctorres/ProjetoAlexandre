using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aranha : InimigoComum
{   
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();            
        dano = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();        
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();        
    }       
}
