using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public List<Vector3> Points { get; set; }
    private float t;
    private int ptIndex;

    [SerializeField]
    private GameObject pointDéplacement;

    private void Start()
    {
        Points = GetSplineData(FileReadingTools.LireFichierTxt("DataSpline.txt"));
        ptIndex = 0;
        pointDéplacement = Instantiate(pointDéplacement);
        var pts = new List<Vector3>();
        
        foreach (var pt in Points)
        {
            pts.Add(new Vector3 (pt.x,pt.y-0.5F,pt.z - 2)*10);
        }
        CréérLigne();
        Points = pts;

    }

    void CréérLigne()
    {
        LineRenderer ligne=gameObject.GetComponent<LineRenderer>();
        ligne.positionCount = Points.Count;
        ligne.SetPositions(Points.ToArray());
    }

    public List<Vector3> GetSplineData(List<string> data)
    {
        List<Vector3> points = new List<Vector3>();
        int j = 0;
        foreach (var donnée in data)
        {
            points.Add(FileReadingTools.ToVector3(donnée));
            j++;
        }
    
        return points;
    }

    public Vector3 QuadraticLerp(Vector3 ptA,Vector3 ptB,Vector3 ptC,float t)
    {
        var pointA = Vector3.Lerp(ptA, ptB,t);
        var pointB = Vector3.Lerp(ptB, ptC, t);
        
        return Vector3.Lerp(pointA, pointB, t);
        
    }
    private void Update()
    {
        t = (Time.deltaTime + t);
        
        pointDéplacement.transform.position= QuadraticLerp(Points[ptIndex]+Vector3.up,
            Points[ptIndex+1]+Vector3.up,Points[ptIndex+2]+Vector3.up, t);
        pointDéplacement.transform.rotation=Quaternion.LookRotation(Points[ptIndex+2]
                                                                    -Points[ptIndex]);
        if (t - 1>=0)
        {
            t = 0;
            if (Points.Count-ptIndex<5)
            {
                ptIndex = 0;
                
                Destroy(pointDéplacement);
                Wait();
                
                pointDéplacement = Instantiate(pointDéplacement);
            }
            else
            {
                ptIndex+=2;
            }
                
        }
       
    }

    public static IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
    }

}