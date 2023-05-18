using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;


public class SplineCubique
{
    public List<Vector3> Points { get; set; }
    public Vector3[] Interpolation { get; set; }
    
    public int[]Stops { get; set; }

    private int Count { get; set; }

    public SplineCubique(List<Vector3> points, int count)
    {
        Points = points;
        Count = count;
    }

    public SplineCubique(List<PathfindingNode> path, int count)
    {
        Points = new List<Vector3>();

        foreach (var node in path)
        {
            Points.Add(node.Position);
        }
        
        Stops = new int[Points.Count];

        Count = count;
    }

    public void Interpoler()
    {
        Interpolation = InterpolerPts(Points, Count);
        
        
        for (int i = 0; i < Points.Count; i++)
        {
            for (int j = 0; j < Interpolation.Length; j++)
            {
                if (SontMême(Points[i], Interpolation[j]))
                {
                    Stops[i] = j;
                    break;
                }
            }
        }
    }

    public static bool SontMême(Vector3 a, Vector3 b)
    {
        var limite = 1.3f;
        return Mathf.Abs(a.x - b.x) <limite && Mathf.Abs(a.z - b.z )< limite &&Mathf.Abs( a.y - b.y)< limite;
    }
    

    public void RenderSpline(bool montrerPoints,Material matériel)
    {
        GameObject spline = new GameObject("Spline");
        var pts = Interpolation;

        var ligne = spline.AddComponent<LineRenderer>();
        
        ligne.positionCount = pts.Length;
        ligne.SetPositions(pts);
        ligne.material = matériel;
        ligne.SetWidth(7,7);
        
        if (montrerPoints)
        {
            foreach (var point in Points)
            {
                var pt = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GameObject.Destroy(pt.GetComponent<SphereCollider>());
                pt.transform.position = point;
                pt.transform.SetParent(spline.transform);
                pt.transform.localScale = Vector3.one * 20;
            }
        }
    }

    public static Vector3[] InterpolerPts(List<Vector3> points, int count)
    {

        float[] xs = new float[points.Count];
        float[] ys = new float[points.Count];
        float[] zs = new float[points.Count];

        for (int i = 0; i < points.Count; i++)
        {
            xs[i] = points[i].x;
            ys[i] = points[i].z;
            zs[i] = points[i].y;
        }

        int inputPointCount = xs.Length;
        float[] inputDistances = new float[inputPointCount];
        
        for (int i = 1; i < inputPointCount; i++)
        {
            Vector3 prevPoint = points[i - 1];
            Vector3 currPoint = points[i];
            float distance = Vector3.Distance(prevPoint, currPoint);
            
            inputDistances[i] = inputDistances[i - 1] + distance;
        }

        float meanDistance = inputDistances.Last() / (count - 1);
        float[] evenDistances = Enumerable.Range(0, count).Select(x => x * meanDistance).ToArray();
        
        float[] xsOut = Interpoler(inputDistances, xs, evenDistances);
        float[] ysOut = Interpoler(inputDistances, ys, evenDistances);
        float[] zsOut = Interpoler(inputDistances, zs, evenDistances);

        Vector3[] pts = new Vector3[xsOut.Length];

        for (int i = 0; i < xsOut.Length; i++)
        {
            pts[i] = new Vector3(xsOut[i], zsOut[i], ysOut[i]);
        }

        return pts;
    }
    

    private static float[] Interpoler(float[] xOrig, float[] yOrig, float[] xInterp)
    {
        (float[] a, float[] b) = FitMatrix(xOrig, yOrig);

        float[] yInterp = new float[xInterp.Length];
        for (int i = 0; i < yInterp.Length; i++)
        {
            int j;
            for (j = 0; j < xOrig.Length - 2; j++)
                if (xInterp[i] <= xOrig[j + 1])
                    break;

            float dx = xOrig[j + 1] - xOrig[j];
            float t = (xInterp[i] - xOrig[j]) / dx;
            float y = (1 - t) * yOrig[j] + t * yOrig[j + 1] +
                t * (1 - t) * (a[j] * (1 - t) + b[j] * t);
            yInterp[i] = y;
        }

        return yInterp;
    }

    private static (float[] a, float[] b) FitMatrix(float[] x, float[] y)
    {
        int n = x.Length;
        float[] a = new float[n - 1];
        float[] b = new float[n - 1];
        float[] r = new float[n];
        float[] A = new float[n];
        float[] B = new float[n];
        float[] C = new float[n];

        float dx1, dx2, dy1, dy2;

        dx1 = x[1] - x[0];
        C[0] = 1.0f / dx1;
        B[0] = 2.0f * C[0];
        r[0] = 3 * (y[1] - y[0]) / (dx1 * dx1);

        for (int i = 1; i < n - 1; i++)
        {
            dx1 = x[i] - x[i - 1];
            dx2 = x[i + 1] - x[i];
            A[i] = 1.0f / dx1;
            C[i] = 1.0f / dx2;
            B[i] = 2.0f * (A[i] + C[i]);
            dy1 = y[i] - y[i - 1];
            dy2 = y[i + 1] - y[i];
            r[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
        }

        dx1 = x[n - 1] - x[n - 2];
        dy1 = y[n - 1] - y[n - 2];
        A[n - 1] = 1.0f / dx1;
        B[n - 1] = 2.0f * A[n - 1];
        r[n - 1] = 3 * (dy1 / (dx1 * dx1));

        float[] cPrime = new float[n];
        cPrime[0] = C[0] / B[0];
        for (int i = 1; i < n; i++)
            cPrime[i] = C[i] / (B[i] - cPrime[i - 1] * A[i]);

        float[] dPrime = new float[n];
        dPrime[0] = r[0] / B[0];
        for (int i = 1; i < n; i++)
            dPrime[i] = (r[i] - dPrime[i - 1] * A[i]) / (B[i] - cPrime[i - 1] * A[i]);

        float[] k = new float[n];
        k[n - 1] = dPrime[n - 1];
        for (int i = n - 2; i >= 0; i--)
            k[i] = dPrime[i] - cPrime[i] * k[i + 1];

        for (int i = 1; i < n; i++)
        {
            dx1 = x[i] - x[i - 1];
            dy1 = y[i] - y[i - 1];
            a[i - 1] = k[i - 1] * dx1 - dy1;
            b[i - 1] = -k[i] * dx1 + dy1;
        }

        return (a, b);
    }
    
}
