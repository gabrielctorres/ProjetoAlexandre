using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{

    public Camera mainCamera;

    float shakeForca;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }


    public void MexendoCamera(float amt,float length)
    {
        
        shakeForca = amt;
        InvokeRepeating("MexerCamera", 0, 0.01f);
        Invoke("Parar", length);
    }

    void MexerCamera()
    {
        if(shakeForca > 0)
        {
            Vector3 camPos = mainCamera.transform.position;
            float shakeAmtX = Random.value * shakeForca * 2 - shakeForca;
            float shakeAmtY = Random.value * shakeForca * 2 - shakeForca;
            camPos.x += shakeAmtX;
            camPos.y += shakeAmtY;

            mainCamera.transform.position = camPos;
        }
        
    }

    void Parar()
    {
        CancelInvoke("MexerCamera");
        mainCamera.transform.localPosition = new Vector3(0,0,-2);
    }
}
