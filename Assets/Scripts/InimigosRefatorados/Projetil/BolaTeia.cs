using UnityEngine;

public class BolaTeia : MonoBehaviour
{
    public GameObject effectStun;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Personagem>() != null)
        {
            GameObject effectInstanciado = null;

            if(effectInstanciado == null)
                effectInstanciado = Instantiate(effectStun, collision.transform.position, effectStun.transform.rotation);

            collision.GetComponent<Personagem>().TomarStun();

            Destroy(effectInstanciado, collision.GetComponent<Personagem>().TimeStun);
            Destroy(this.gameObject);
        }
    }
}
