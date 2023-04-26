using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    List<Vector3> Points { get; set; }

    private void Start()
    {
        Points = GetSplineData(FileReadingTools.LireFichierTxt("DataSpline"));
    }

    public List<Vector3> GetSplineData(List<string> data)
    {
        List<Vector3> points = new List<Vector3>();
        int j = 0;
        foreach (var donnée in data)
        {
            //maybe create a FileReadingTool
            //that converts a 2d vectors string into 3d vectors string
            points.Add(FileReadingTools.ToVector3((donnée.Insert(donnée.IndexOf(",") - 1, ",0"))));
            j++;
        }

        return points;
    }

    // public float[] GetCoeffs(List<Vector3> splinePoints)
    // {
    // }

    
}