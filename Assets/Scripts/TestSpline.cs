using System.Collections.Generic;
using UnityEngine;

public class TestSpline : MonoBehaviour
{
    [SerializeField] private Material _material;
    
    void Start()
    {
        var spline = new GameObject("Spline");
        List<Vector3> Points = new List<Vector3>();
        Points.Add(new Vector3(-6,0,14));
        Points.Add(new Vector3(-9,0,15));
        Points.Add(new Vector3(-12,0,14));
        Points.Add(new Vector3(-13,0,12));
        Points.Add(new Vector3(-13,0,10));
        Points.Add(new Vector3(-13,0,8));
        Points.Add(new Vector3(-13,0,6));
        Points.Add(new Vector3(-12,0,5));
        Points.Add(new Vector3(-10,0,4));
        Points.Add(new Vector3(-7,0,4));
        Points.Add(new Vector3(-6,0,6));
        Points.Add(new Vector3(-6,0,7));
        Points.Add(new Vector3(-7,0,8));
        Points.Add(new Vector3(-9,0,8));
        Points.Add(new Vector3(-8,0,6));
        Points.Add(new Vector3(-5,0,7));
        Points.Add(new Vector3(-4,0,9));
        Points.Add(new Vector3(-4,0,11));
        Points.Add(new Vector3(-4,0,13));
        Points.Add(new Vector3(-3,0,15));
        Points.Add(new Vector3(1,0,15));
        Points.Add(new Vector3(3,0,13));
        Points.Add(new Vector3(2,0,11));
        var xs = new float[Points.Count];
        var ys = new float[Points.Count];

        for (int i = 0; i < Points.Count; i++)
        {
            xs[i] = Points[i].x;
            ys[i] = Points[i].z;
        }

      var pts=  SplineCubique.InterpolerPts(xs, ys,0, 200);

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
