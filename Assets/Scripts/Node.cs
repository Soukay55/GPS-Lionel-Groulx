using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Graphs;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class Node
{

     private StreamReader fluxLecture;
     private const string PATH = "Assets/RessourcesGPS/DataNodes";
     private const int NB_NODES_PAR_ÉTAGE = 9;
     private string[] délimiteurs = {"\t"};
     private List<string>dataTab;
     public GPSCoordinate CoordonéesGPS { get; set; }
     
     public int Nombre { get; set; }
     
     public string Nom { get; set; } /// gnr la cafet s'appele "cafeteria"
     
     public bool EstEndroitPublic { get; set; }//anyhing that isnt a classsroom
     
     public int Étage { get; set; } //try to get nb noeuds par etage to be constant so tpx calculer sa aek maths

     public Vector3 Position { get; set; } //position aek unity: désigne point(0,0),
                                           //and calculate this pos en faisant (0,0)+(distanceX,distanceY) de GPSCoord
     public List<int>ConnectedNodes { get; set; }//num d nodes qui peuvent y connect

     public void Start()
     {
        // List<nodes>
     }
     public Node(int nombre, string nom, bool estEndroitPublic, int étage, List<int>connectedNodes)
     {
         Nombre = nombre;
         Nom = nom;
         EstEndroitPublic = estEndroitPublic;
         Étage = étage;
         List<int> ConnectedNodes = connectedNodes;


     }

     public double CalculateEdgeLength(Node node1, Node node2)
     {

         return GPSCoordinate.
             CalculerDistanceEntreDeuxCoordonnées(node1.CoordonéesGPS, node2.CoordonéesGPS);
     }

     public List<Node>GetNodeData(string fichierNodesName, List<Node>nodes)
     {
         var fichierÀlire = $"{PATH}/{fichierNodesName}";
         fluxLecture = new StreamReader(fichierÀlire);
         
         nodes = new List<Node>();
         
         var données = string.Empty;
         using (fluxLecture)
         {
             string ligne;
             while ((ligne = fluxLecture.ReadLine()) != null)
                 données += ligne + "\t";
         }
         
         dataTab = données.Split(délimiteurs, StringSplitOptions.RemoveEmptyEntries).ToList();
         
         return nodes;
         
     }

     public bool ToBool(string valeurBool)
     {
         if (valeurBool == "true")
             return true;

         return false;
     }

     // public void TrouverPosition()
     // {
     //     
     // }
     //
     // public void 


}
