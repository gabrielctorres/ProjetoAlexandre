using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReativadorPlataforma : MonoBehaviour
{
    public GameObject plataforma;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!plataforma.activeSelf)
        {
            StartCoroutine(SpawnarPlataforma());
        }
    }

    IEnumerator SpawnarPlataforma()
    {
        yield return new WaitForSeconds(3f);
        plataforma.SetActive(true);
    }
}
