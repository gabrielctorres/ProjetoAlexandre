using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escorpiao : InimigoComum
{
    public  float dano;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.name == "Adaga")
        {
            this.TomarDano(collision.GetComponent<Weapon>().dano);
            Debug.Log(collision.GetComponent<Weapon>().dano);
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(direcaoOlhar * -1, 0) * 30, ForceMode2D.Impulse);
        }

        if (collision.name == "Espada")
        {
            this.TomarDano(collision.GetComponent<Weapon>().dano);
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(direcaoOlhar * -1, 0) * 30, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Adaga")
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(direcaoOlhar * -1, 0) * 35, ForceMode2D.Impulse);
        }

        if (collision.name == "Espada")
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(direcaoOlhar * -1, 0) * 35, ForceMode2D.Impulse);
        }
    }


}
