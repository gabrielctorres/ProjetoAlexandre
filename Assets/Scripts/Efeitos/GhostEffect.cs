using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public float delay;

    public float timeToDestroy;

    public bool makeGhost = false;

    public GameObject prefabGhost;
    private float delaySeconds;
    void Start()
    {
        delaySeconds = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {
            if (delaySeconds > 0)
                delaySeconds -= Time.deltaTime;
            else
            {
                GameObject ghostInstanciado = Instantiate(prefabGhost, transform.position, transform.rotation);
                ghostInstanciado.transform.localScale = this.GetComponent<Transform>().localScale;
                ghostInstanciado.GetComponent<SpriteRenderer>().flipX = this.GetComponent<SpriteRenderer>().flipX;
                delaySeconds = delay;
                Destroy(ghostInstanciado, timeToDestroy);

            }
        }

    }
}
