using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reliquias : MonoBehaviour
{
    public StartData data;
    
    void Start()
    {
        gameObject.SetActive(data.reliquiaAtivada);
    }

    private void Update()
    {
        
    }

}
