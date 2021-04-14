using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float tamanho, primeiraPos;

    public GameObject cam;

    public float parallaxEffect;
   

    // Start is called before the first frame update
    void Start()
    {
        primeiraPos = transform.position.x;
        tamanho = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(primeiraPos + dist, transform.position.y, transform.position.z);

        if (temp > primeiraPos + tamanho) primeiraPos += tamanho;
        else if (temp < primeiraPos - tamanho) primeiraPos -= tamanho;

    }
}
