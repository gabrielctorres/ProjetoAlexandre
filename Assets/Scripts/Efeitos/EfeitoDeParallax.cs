using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoDeParallax : MonoBehaviour
{
    public GameObject cameraPlayer;
    private float length, startPos;
    public float speedParallax;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cameraPlayer.transform.position.x * (1 - speedParallax));
        float dist = (cameraPlayer.transform.position.x * speedParallax);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if(temp > startPos + length / 2)
        {
            startPos += length;
        }
        else if(temp < startPos - length / 2)
        {
            startPos -= length;
        }
    }
}
