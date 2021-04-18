using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScript : MonoBehaviour
{
    [System.Serializable]
   public class DropItens
    {
        public string nome;
        public GameObject item;
        public int Raridade;
    }

    public List<DropItens> TabelaLoot = new List<DropItens>();
    public int dropChance;

    private void Start()
    {
        calculandoLoot();
    }


    void calculandoLoot()
    {
        int calc_dropChance = Random.Range(0, 101); //  aqui q a gente seta os 100%


        if(calc_dropChance > dropChance)
        {           
            return;
        }


        if(calc_dropChance <= dropChance)
        {
            int itemWeight = 0;

            for (int i = 0; i < TabelaLoot.Count; i++)
            {
                itemWeight += TabelaLoot[i].Raridade;
            }

            int valorRandomico = Random.Range(0, itemWeight);

            for (int j = 0; j < TabelaLoot.Count; j++)
            {
                if (valorRandomico <= TabelaLoot[j].Raridade)
                {
                    Instantiate(TabelaLoot[j].item, transform.position, Quaternion.identity);
                    return;
                }
                valorRandomico -= TabelaLoot[j].Raridade;
            }
        }
    }
}
