using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SavePlayer(Personagem personagem)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string dataPath = Application.streamingAssetsPath + "/Saves";
        string path = dataPath + "/player.smite";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(personagem);        
        formatter.Serialize(stream, data);
        stream.Close();

    }
    public static PlayerData LoadPlayer()
    {
        string dataPath = Application.streamingAssetsPath + "/Saves";
        string path = dataPath + "/player.smite";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
             
            PlayerData data = formatter.Deserialize(stream) as PlayerData;            
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Arquivo não encontrato no " + path);
            return null;
        }

    }
    
}
