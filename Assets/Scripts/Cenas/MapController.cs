using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject cameraPoiter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseController();
        LimitandoTela();
    }

    void MouseController()
    {
        Vector3 mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        cameraPoiter.transform.position = Vector2.MoveTowards(cameraPoiter.transform.position, mousePosition, 6 * Time.deltaTime);
    }


    void LimitandoTela()
    {
        if (cameraPoiter.transform.position.x <= 2.2f || cameraPoiter.transform.position.x >= -0.9f)
        {            
            float xPos = Mathf.Clamp(cameraPoiter.transform.position.x, -0.9f, 2.2f);            
            cameraPoiter.transform.position = new Vector2(xPos, transform.position.y);
        }
        if (cameraPoiter.transform.position.y <= 7.6f || cameraPoiter.transform.position.y >= -7.9f)
        {                      
            float YPos = Mathf.Clamp(cameraPoiter.transform.position.y,-7.9f, 7.6f);
            cameraPoiter.transform.position = new Vector2(cameraPoiter.transform.position.x, YPos);

        }
    }
}
