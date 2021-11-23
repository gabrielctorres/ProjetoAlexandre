using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StartData : ScriptableObject
{
    public int atualCena;
    public int quantidadeEstrelas;
    public Vector3 position;
    public bool reliquiaAtivada = true;
}
