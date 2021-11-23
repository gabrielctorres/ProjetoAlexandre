using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    
    public void Change()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex, LoadSceneMode.Single);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

}
