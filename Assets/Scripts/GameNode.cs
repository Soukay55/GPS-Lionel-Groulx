using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class GameNode : Node
{
    public bool EstEndroitPublic { get; set; } //anything that isnt a classsroom

    public GameObject Objet;


    public void Start()
    {
        // List<nodes>
    }

    public GameNode(int nombre, string nom, bool estEndroitPublic, Étage étage,
        List<float> connectedNodes, GPSCoordinate coordonéesGps) : base(nombre, nom, étage, coordonéesGps)
    {
        EstEndroitPublic = estEndroitPublic;
    }

    public double CalculateEdgeLength(GameNode node1, GameNode node2)
    {
        return GPSCoordinate.CalculerDistanceEntreDeuxCoordonnées(node1.CoordonéesGPS, node2.CoordonéesGPS);
    }


    // public void TrouverPosition()
    // {
    //     
    // }
    //
    // public void 
}