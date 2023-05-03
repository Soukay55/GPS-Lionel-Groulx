using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementCameraCarte : MonoBehaviour
{
    private Vector3 toucheDepart;
    public float zoomOutMin = 3;
    public float zoomOutMax = 8;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            toucheDepart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            Touch toucheZero = Input.GetTouch(0);
            Touch toucheUn = Input.GetTouch(1);

            Vector2 toucheAvantZero = toucheZero.position - toucheZero.deltaPosition;
            Vector2 toucheAvantUn = toucheUn.position - toucheUn.deltaPosition;

            float magnitudePrecedente = (toucheAvantZero - toucheAvantUn).magnitude;
            float magnitude = (toucheZero.position - toucheUn.position).magnitude;

            float differenceMagnitude = magnitude - magnitudePrecedente;
            
            Zoom(differenceMagnitude*0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = toucheDepart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            
        }

    }

    public void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
    
    

