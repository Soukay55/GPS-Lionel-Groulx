using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementCouloir : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var objetIntersect = collision.gameObject;
        if (objetIntersect.name.Contains("BlocCouloir"))
        {
            var index = objetIntersect.transform.GetSiblingIndex();
            var nbInitialBlocs = objetIntersect.transform.parent.childCount;
            
            if(nbInitialBlocs-1-index>4||nbInitialBlocs-1-index>nbInitialBlocs-1-4)
            Destroy(objetIntersect);
            // if (Physics.ComputePenetration(collision.collider, transform.position, transform.rotation, 
            //         collision.collider, collision.transform.position, collision.transform.rotation, 
            //         out Vector3 direction, out float distance))
            // {
            //     gameObject.transform.position -= direction * distance/2;
            //     var r = transform.localScale.z;
            //     r -= distance / 2;
            // }
        }
    }
}
