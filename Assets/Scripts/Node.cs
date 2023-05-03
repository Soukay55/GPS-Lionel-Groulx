using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Animations;

public class Node 
{
    public GPSCoordinate CoordonéesGPS { get; set; }
    public int Nombre { get; set; }
    public string Nom { get; set; }
    public List<string> Noms { get; set; }
    public Étage Niveau { get; set; } //try to get nb noeuds par etage to be constant so tpx calculer sa aek maths

    public Aile Aile { get; set; }
    public Vector3 Position { get; set; } //position aek unity: désigne point(0,0),
    //and calculate this pos en faisant (0,0)+(distanceX,distanceY) de GPSCoord
    

    public List<float> ConnectedNodes { get; set; }
    
    //clean up constructors
    public Node(int nombre, string nom, GPSCoordinate coordonéesGps, List<float>connectedNodes )
    {
        Nombre = nombre;
        Nom = nom;
        CoordonéesGPS = coordonéesGps;
        ConnectedNodes = connectedNodes;
    }

    public Node(int nombre, GPSCoordinate coordonéesGps)
    {
        Nombre = nombre;
        CoordonéesGPS = coordonéesGps;

    }
    public Node(int nombre, string nom, Étage étage, GPSCoordinate coordonéesGps)
    {
        Nombre = nombre;
        Nom = nom;
        Niveau = étage;
        CoordonéesGPS = coordonéesGps;
    }
    
    public Node(int nombre, List<string> noms, Étage étage, Vector3 position,GPSCoordinate coordonéesGps)
    {
        Nombre = nombre;
        Noms = noms;
        Niveau = étage;
        Position = position;
        CoordonéesGPS = coordonéesGps;
    }
  
    public static float CalculerDistanceNodes(Node A, Node B)
    {
        return GPSCoordinate.CalculerDistanceEntreDeuxCoordonnées(A.CoordonéesGPS, B.CoordonéesGPS);
    }

    public GameObject DrawNode()
    {
        var node=GameObject.CreatePrimitive(PrimitiveType.Sphere);
        node.transform.position = Position;
        node.transform.localScale = Vector3.one * 30;
        return node;
    }
    

    public void SetPosition(Node origine)
    {
        Position = GPSCoordinate.PositionRelativeENUCoords
            (origine.CoordonéesGPS, CoordonéesGPS) * 9;
    }

    public void GetAile(List<Polygone>ailes)
    {
        int i = 0;
        foreach (var aile in ailes)
        {
            //seule exception qui ne marche pas avec l'algorithme
            if (Nombre == 6)
            {
                Aile = Aile.D;
                break;
            }
            
            if (aile.EstDansAile(Position))
            {
                Aile = (Aile)i;
                break;
            }
            i++;
        }
    }
    
     
}