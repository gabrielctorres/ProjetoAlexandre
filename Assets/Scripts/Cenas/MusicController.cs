using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public GameObject musicaFase;
    public GameObject musicaBoss;

    bool canPlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlay)
        {
            StartCoroutine(diminuirVolume(musicaFase.GetComponent<AudioSource>(), 0.01f));           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Personagem>() != null)
        {
            canPlay = true;
            musicaBoss.GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator diminuirVolume(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        musicaFase.SetActive(false);
        audioSource.volume = startVolume;
    }
}
