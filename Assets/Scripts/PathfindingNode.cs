using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

public class PathfindingNode : Node
{
    //they aint needa name they for pathfinding
    
    public List<string>Noms { get; set; }
    public PathfindingNode Parent { get; set; }
    public bool EstEndroitPublic { get; set; } //anything that isnt a classsroom

    public bool EstTraversable { get; set; } = true;

    public string Instructions { get; set; }

    public List<float> ConnectedNodes { get; set; }

    public List<PathfindingNode> Voisins
    {
        get; set;
    }
            
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
        Voisins = new List<PathfindingNode>();
    }

    public PathfindingNode(int nombre, List<string> noms ,bool estEndroitPublic,
        Étage étage,  List<float> connectedNodes,Vector3 position,GPSCoordinate coordonéesGps)
        :base(nombre,noms,étage,position,coordonéesGps)
    {
        ConnectedNodes = connectedNodes;
        EstEndroitPublic = estEndroitPublic;
        Voisins = new List<PathfindingNode>();
        Noms = noms;
    }

    public float GetGCost()
    {
        return Parent.GCost +GPSCoordinate.
                   CalculerDistanceEntreDeuxCoordonnées
                       (Parent.CoordonéesGPS, CoordonéesGPS);
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
    
    //this should take care of the Niveaux[,]adjustment ??

    
    
}