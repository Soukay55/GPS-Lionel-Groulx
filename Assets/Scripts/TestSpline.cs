using System.Collections.Generic;
using UnityEngine;

public class TestSpline : MonoBehaviour
{
    [SerializeField] private Material _material;
    
    void Start()
    {
        var spline = new GameObject("Spline");
        List<Vector3> Points = new List<Vector3>();
        // Points.Add(new Vector3(1257.34f, 0.00f, 653.16f));
        // Points.Add(new Vector3(-12,4,14));
        // Points.Add(new Vector3(-13,5,12));
        Points.Add(new Vector3(1257.341f,0f,653.1613f));
        Points.Add(new Vector3(1252.11f, 0.00f, 494.28f));
        Points.Add(new Vector3(1252.246f,0f,393.4313f));
        Points.Add(new Vector3(1257.355f,249f,315.4543f));
        Points.Add(new Vector3(1805.081f,249f,336.9844f));
        Points.Add(new Vector3(2015.905f,249f,406.5918f));
        // Points.Add(new Vector3(-7,1,4));
        // Points.Add(new Vector3(-6,-4,6));
        // Points.Add(new Vector3(-6,-4,7));
        // Points.Add(new Vector3(-7,-4,8));
        // Points.Add(new Vector3(-9,-4,8));
        // Points.Add(new Vector3(-8,10,6));
        // Points.Add(new Vector3(-5,10,7));
        // Points.Add(new Vector3(-4,10,9));
        // Points.Add(new Vector3(-4,0,11));
        // Points.Add(new Vector3(-4,0,13));
        // Points.Add(new Vector3(-3,0,15));
        // Points.Add(new Vector3(1,0,15));
        // Points.Add(new Vector3(3,0,13));
        // Points.Add(new Vector3(2,0,11));
        var xs = new float[Points.Count];
        var ys = new float[Points.Count];
        var zs = new float[Points.Count];
        
        for (int i = 0; i < Points.Count; i++)
        {
            xs[i] = Points[i].x;
            ys[i] = Points[i].z;
            zs[i] = Points[i].y;
        }

      var pts=  SplineCubique.InterpolerPts(Points, 200);

      for (int i = 0; i < pts.Length; i++)
      {
          var r = GameObject.CreatePrimitive(PrimitiveType.Sphere);
          r.transform.position = pts[i];
      }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
