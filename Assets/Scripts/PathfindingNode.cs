using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

public class PathfindingNode : Node
{
    public float value;
    
    public PathfindingNode Parent { get; set; }
    public bool EstEndroitPublic { get; set; } //anything that isnt a classsroom

    public bool EstTraversable { get; set; }

    public string Instructions { get; set; }

    public List<float> ConnectedNodes { get; set; }

    public List<PathfindingNode>Voisins { get; set; }//les voisins doivent
                                          //être calculés à partir d connectdnodes

    //public bool EstDansClosedList { get; set; } = false;
    
    public bool ÀÉviter { get; set; }

    //public bool EstDansOpenList { get; set; } = false;


    public float FCost
    {
        get { return HCost + GCost;}
        
    }
    public float HCost { get; set; }

    public float GCost
    {
        get;
        set;
    }


    // Start is called before the first frame update
    public PathfindingNode(int nombre, string nom, bool estEndroitPublic, Étage étage,
        List<float> connectedNodes, GPSCoordinate coordonéesGps) : base(nombre, nom, étage, coordonéesGps)
    {
        ConnectedNodes = connectedNodes;
        EstEndroitPublic = estEndroitPublic;
    }

    public float GetGCost()
    {
        return Parent.GCost +GPSCoordinate.
                   CalculerDistanceEntreDeuxCoordonnées(Parent.CoordonéesGPS, CoordonéesGPS);
    }

    public void SetGCost()
    {
        GCost = GetGCost();
    }

    public float GetHCost(Node pointArrivée)
    {
        return GPSCoordinate.CalculerDistanceEntreDeuxCoordonnées
            (pointArrivée.CoordonéesGPS, CoordonéesGPS);
    }
    
    public void SetHCost()
    {
        //GCost = GetHCost();
    }
}