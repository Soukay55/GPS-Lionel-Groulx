using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOverlap : MonoBehaviour
{
    public GameObject overlappingObject;
    public float scaleReduction= 0.1f;

    private bool overlaps = false;
    void OnCollisionEnter(Collision collision)
    {
        overlappingObject = collision.gameObject;
        DeleteOverlap();
    }

    void DeleteOverlap()
    {
        GameObject overlappingObj;
        overlaps = Physics.CheckSphere(overlappingObject.transform.position, gameObject.transform.localScale.z / 2);
          while (overlaps)
          {
              
              overlappingObject.transform.localScale= new Vector3(overlappingObject.transform.localScale.x,
                  overlappingObject.transform.localScale.y,overlappingObject.transform.localScale.z-scaleReduction);
            overlappingObject.transform.Translate(Vector3.forward*scaleReduction/2);
            
          
          }
    }

   

}
