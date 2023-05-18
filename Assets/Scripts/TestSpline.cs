using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestSpline : MonoBehaviour
{
    [SerializeField] private Material _material;

    void Start()
    {
        List<Vector3> Points = new List<Vector3>();

        Points.Add(new Vector3(1257f, 0f, 653f));
        Points.Add(new Vector3(1252f, 0f, 494f));
        Points.Add(new Vector3(1252f, 0f, 393f));
        Points.Add(new Vector3(1251f, 83f, 393f));
        Points.Add(new Vector3(1250f, 249f, 393f));
        Points.Add(new Vector3(1257f, 249f, 315f));
        Points.Add(new Vector3(1411f, 249f, 314f));
        Points.Add(new Vector3(1553f, 249f, 331f));
        Points.Add(new Vector3(1689f, 249f, 333f));
        Points.Add(new Vector3(1805f, 249f, 336f));
        Points.Add(new Vector3(1873f, 249f, 337f));
        Points.Add(new Vector3(1973f, 249f, 361f));

        var xs = new float[Points.Count];
        var ys = new float[Points.Count];
        var zs = new float[Points.Count];

        // for (int i = 0; i < Points.Count; i++)
        // {
        //     var p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //     p.transform.localScale = Vector3.one * 20;
        //     p.transform.position = Points[i];
        //
        //     xs[i] = Points[i].x;
        //     ys[i] = Points[i].z;
        //     zs[i] = Points[i].y;
        // }
        var splineCubique = new SplineCubique(Points, 300);
        
        splineCubique.RenderSpline(true,_material);


        // var pts = SplineCubique.InterpolerPts(Points, 200);
        //
        //
        //
        // var r = spline.AddComponent<LineRenderer>();
        // r.positionCount = pts.Length;
        // r.SetPositions(pts);
        // r.SetWidth(6, 6);


    }
    
}
