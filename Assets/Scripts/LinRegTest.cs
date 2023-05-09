using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LinRegTest : MonoBehaviour
{
    [SerializeField]
    private GameObject couloirPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> points = new List<Vector3>();
        
        points.Add(new Vector3(1805,0,336.98f)/10);
        points.Add(new Vector3(1689,0,333)/10);
        points.Add(new Vector3(1553,0,331)/10);
        points.Add(new Vector3(1411,0,314)/10);
        points.Add(new Vector3(1257,0,315)/10);
        points.Add(new Vector3(1055,0,317)/10);
        points.Add(new Vector3(960,0,321)/10);
        points.Add(new Vector3(1120,0,321)/10);

        foreach (var VARIABLE in points)
        {
          var c=  GameObject.CreatePrimitive(PrimitiveType.Sphere);
          c.transform.position = VARIABLE;
          c.transform.localScale = Vector3.one * 3;
        } 

        var pts = CreateurCouloir.RégressionLinéaire(points);
        var lr = gameObject.AddComponent<LineRenderer>();
        lr.positionCount = 2;

        var ret=GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ret.transform.localScale = Vector3.one * 3;
        ret.transform.position = pts[0];
        
        var ret2=GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ret2.transform.localScale = Vector3.one * 3;
        ret2.transform.position = pts[1];

        var l = gameObject.GetComponent<LineRenderer>();
        l.positionCount = 2;
        l.SetPositions(pts);

        //CreateurCouloir.CréerCouloir(pts[0], pts[1], couloirPrefab);

    }

}
