using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public StartData startData;
    public void ResetaFase()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Jogar()
    {
        SceneManager.LoadScene("CenaTutorial", LoadSceneMode.Single);
    }

    public void CarregarSave()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        SceneManager.LoadScene(data.atualCena);
        startData.position.x = data.position[0];
        startData.position.y = data.position[1];
        startData.position.z = data.position[2];
        startData.quantidadeReliquias = data.quantidadeReliquias;
    }
}
