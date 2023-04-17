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
    public Étage Niveau { get; set; } //try to get nb noeuds par etage to be constant so tpx calculer sa aek maths

    public Vector3 Position { get; set; } //position aek unity: désigne point(0,0),
    //and calculate this pos en faisant (0,0)+(distanceX,distanceY) de GPSCoord
    

    public List<float> ConnectedNodes { get; set; }
    
    public Node(int nombre, string nom, GPSCoordinate coordonéesGps, List<float>connectedNodes )
    {
        Nombre = nombre;
        Nom = nom;
        CoordonéesGPS = coordonéesGps;
        ConnectedNodes = connectedNodes;
    }
    public Node(int nombre, string nom, Étage étage, GPSCoordinate coordonéesGps)
    {
        Nombre = nombre;
        Nom = nom;
        Niveau = étage;
        CoordonéesGPS = coordonéesGps;
    }

    public static float CalculerDistanceNodes(Node A, Node B)
    {
        return GPSCoordinate.CalculerDistanceEntreDeuxCoordonnées(A.CoordonéesGPS, B.CoordonéesGPS);
    }

    public void DrawNode()
    {
        
    }
    

    public void SetPosition(Node origine)
    {
        Position = GPSCoordinate.PositionRelativeENUCoords
            (origine.CoordonéesGPS, CoordonéesGPS);
    }
    
     
}