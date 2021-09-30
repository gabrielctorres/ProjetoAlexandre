using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
public class Buttons : MonoBehaviour
{
    public StartData startData;

    public TextMeshProUGUI textPlay;
    public void ResetaFase()
    {
        string path = Application.persistentDataPath + "/player.smite";

        if (File.Exists(path))
            CarregarSave();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if(textPlay != null)
            ChangeText();
    }

    public void ChangeText()
    {
        string path = Application.persistentDataPath + "/player.smite";

        if (File.Exists(path))
            textPlay.text = "CONTINUAR";
        else
            textPlay.text = "NOVO JOGO";            

    }

    public void Jogar()
    {
        string path = Application.persistentDataPath + "/player.smite";

        if (File.Exists(path))        
            CarregarSave();
        else        
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1), LoadSceneMode.Single);


        
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
