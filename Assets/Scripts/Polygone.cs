
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Polygone
{
    public List<Vector3> Points { get; set; }
    public string Nom { get; set; }

    public GameObject Sol { get; set; }
    
    public Polygone(string nom, List<Vector3> points)
    {
        SetPoints(points);
        Nom = "Aile" + nom;
    }

    public void SetPoints(List<Vector3> points)
    {
        Points = points;
        if (!IsClockwise())
        {
            Points.Reverse();
        }
        
    }

    //instancie un polygone,
    //donc doit être statique
    public static Polygone GetPolygon(Node node, List<Node> nodes)
    {
        List<Vector3> points = new List<Vector3>();
        foreach (var connectedNode
                 in node.ConnectedNodes)
        {
            points.Add(nodes[(int)(connectedNode - 1)].Position);
        }

        return new Polygone(node.Nom, points);
    }

    public void CréerSol()
    {
        Sol = new GameObject("Sol" + " de l'" + Nom);
        var sol=Sol.AddComponent<MeshFilter>();
        Sol.AddComponent<MeshCollider>();
        Sol.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        mesh.vertices = Points.ToArray();
        List<int>triangles=Triangulate();

        int i = 0;
        
        foreach (var triangle in triangles)
        {
            // var c=GameObject.CreatePrimitive(PrimitiveType.Capsule);
            // c.transform.position = Points[triangle];
            // c.transform.SetParent(Sol.transform);
        }
        
        sol.mesh = mesh;
    }

    public void DessinerPolygone(GameObject prefab, GameObject parent)
    {

        for (int i = 0; i < Points.Count - 1; i++)
        {
            var obj = CreateurCouloir.CréerCouloir
                (Points[i], Points[i + 1], prefab);
            obj.transform.SetParent(parent.transform);

        }

    }
    
    public override string ToString()
    {
        int j = 1;
        string s = Nom;
        foreach (Vector3 point in Points)
        {
            s += " Point #" + j + " : " + point;
            s += Environment.NewLine;
            j++;
        }

        return s;
    }

    public bool IsClockwise()
    {
        Vector3 A = CalculerCentroide();

        Vector3 AB = Points[0] - A;
        Vector3 AC = Points[1] - A;

        return Vector3.Cross(AB, AC).y > 0;
    }
    

    public Polygone DupliquerPolygone(Polygone polygone)
    {
        return new Polygone("Copie" + polygone.Nom, polygone.Points);
    }

    public Vector3 CalculerCentroide()
    {
        var sommePoints = Vector3.zero;
        foreach (var point in Points)
        {
            sommePoints += point;

        }

        return sommePoints / Points.Count;
    }
    
    public List <int> Triangulate()
    {
        List<int> triangles = new List<int>();
        List<int> indexes = CreateIndexList();

        while (indexes.Count > 3)
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                var pointA = Points[indexes[(i-1+indexes.Count)%indexes.Count]];
                var pointB = Points[i];
                var pointC = Points[indexes[(i + 1)%indexes.Count]];

                if (IsConvex(pointA, pointB, pointC)&&
                    !ContainsOtherVertices(pointA, pointB, pointC, indexes))
                {
                    triangles.Add(indexes[(i-1+indexes.Count)%indexes.Count]);
                    triangles.Add(i);
                    triangles.Add(indexes[(i + 1)%indexes.Count]);

                    indexes.RemoveAt(i);
                    break;
                }
                
            }
            
        }

        triangles.Add(indexes[0]);
        triangles.Add(indexes[1]);
        triangles.Add(indexes[2]);

        for (int i = 0; i < triangles.Count; i++)
        {
            Debug.Log(Nom+Points[triangles[i]]);
        }

        return triangles;

    }

    //check if maybe it would be useful for other scri
    //nto make static
    public List<int> CreateIndexList()
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < Points.Count-1; i++)
        {
            indexList.Add(i);
        }
        return indexList;
    }

    //not sure
    public bool IsConvex(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 vecteurBA = pointA-pointB;
        Vector3 vecteurBC= pointC - pointB;

        return Vector3.Cross(vecteurBA, vecteurBC).y < 0;
    }
    
    public bool ContainsOtherVertices(Vector3 pointA, Vector3 pointB, Vector3 pointC,List<int>indexList)
    {
        foreach (var point in indexList)
        {
            if (Points[point] != pointA || Points[point] != pointB || Points[point] != pointA)
            {
                return IsInTriangle(pointA, pointB, pointC, Points[point]);
            }
        }
        return false;
    }

    public bool IsInTriangle(Vector3 pointA, Vector3 pointB, Vector3 pointC,Vector3 point)
    {
        Vector3 AÀpoint=point-pointA;
        Vector3 BÀpoint=point-pointB;
        Vector3 CÀpoint=point-pointC;
        
        Vector3 AB = pointB - pointA;
        Vector3 BC = pointC - pointB;
        Vector3 CA = pointA - pointC;

        return Vector3.Cross(AÀpoint, AB).y< 0 && Vector3.Cross(BÀpoint, BC).y < 0 &&  Vector3.Cross(CÀpoint, CA).y < 0;
    }
    
}