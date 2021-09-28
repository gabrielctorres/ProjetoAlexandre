using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ShakeCamera : MonoBehaviour
{

    public CinemachineVirtualCamera mainCamera;

    

    float shakeForca;

    private void Awake()
    {

    }

    private void Start()
    {
        
    }
    public void MexendoCamera(float amt,float length)
    {
        
        shakeForca = amt;
        InvokeRepeating("MexerCamera", 0, 0.01f);
        Invoke("Parar", length);
    }

    void MexerCamera()
    {
        
        if (shakeForca > 0)
        {
           /* Vector3 camPos = mainCamera.transform.position;
            float shakeAmtX = Random.value * shakeForca * 2 - shakeForca;
            float shakeAmtY = Random.value * shakeForca * 2 - shakeForca;
        
            camPos.x += shakeAmtX;
            camPos.y += shakeAmtY;
            mainCamera.transform.SetPositionAndRotation(new Vector3(shakeAmtX, shakeAmtY), mainCamera.transform.rotation);*/


           
        }
        
    }

    void Parar()
    {
        CancelInvoke("MexerCamera");       
    }
}
