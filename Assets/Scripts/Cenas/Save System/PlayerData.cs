using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int atualCena;
    public int quantidadeEstrelas;
    public float[] position;

    public PlayerData (Personagem personagem)
    {
        atualCena = personagem.cena;
        quantidadeEstrelas = personagem.numEstrelas;

        position = new float[3];
        position[0] = personagem.transform.position.x;
        position[1] = personagem.transform.position.y;
        position[2] = personagem.transform.position.z;
    }
}
