using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node 
{
    public GPSCoordinate CoordonéesGPS { get; set; }
     
    public int Nombre { get; set; }
     
    public string Nom { get; set; } /// gnr la cafet s'appele "cafeteria"
     
    public bool EstEndroitPublic { get; set; }//anything that isnt a classsroom

    public bool EstTraversable { get; set; }
     
    public int Étage { get; set; } //try to get nb noeuds par etage to be constant so tpx calculer sa aek maths

    public Vector3 Position { get; set; } //position aek unity: désigne point(0,0),
    //and calculate this pos en faisant (0,0)+(distanceX,distanceY) de GPSCoord
    public List<int>ConnectedNodes { get; set; }//num d nodes qui peuvent y connect

    public Node( int nombre, string nom, bool estEndroitPublic, int étage, 
        List<int>connectedNodes,GPSCoordinate coordonéesGps)
    {
         
        Nombre = nombre;
        Nom = nom;
        EstEndroitPublic = estEndroitPublic;
        Étage = étage;
        List<int> ConnectedNodes = connectedNodes;
        CoordonéesGPS = coordonéesGps;

    }
    
}
