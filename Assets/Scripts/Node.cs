using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
     private const int NB_DONNÉES_PAR_NODE = 6;
     private string[] délimiteurs = {"\t"};
     private List<string>dataTab;
     public GPSCoordinate CoordonéesGPS { get; set; }
     
     public int Nombre { get; set; }
     
     public string Nom { get; set; } /// gnr la cafet s'appele "cafeteria"
     
     public bool EstEndroitPublic { get; set; }//anything that isnt a classsroom
     
     public int Étage { get; set; } //try to get nb noeuds par etage to be constant so tpx calculer sa aek maths

     public Vector3 Position { get; set; } //position aek unity: désigne point(0,0),
                                           //and calculate this pos en faisant (0,0)+(distanceX,distanceY) de GPSCoord
     public List<int>ConnectedNodes { get; set; }//num d nodes qui peuvent y connect

     public void Start()
     {
        // List<nodes>
     }
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
         
         dataTab = données.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
         for (int i = 0; i < dataTab.Count-1; i+=NB_DONNÉES_PAR_NODE)
         {
             int nombre = int.Parse(dataTab[i], CultureInfo.InvariantCulture);
             string nom = dataTab[i + 1];
             bool endroitPublic = ToBool(dataTab[i + 2]);
             int étage=int.Parse(dataTab[i+3], CultureInfo.InvariantCulture);
             List<int> connectedNodes = ToList(dataTab[i + 4]);
             GPSCoordinate coordonéesGps = ToGpsCoordinate(dataTab[i + 5]);
             
             nodes.Add(new Node(nombre,nom,endroitPublic,étage,connectedNodes,coordonéesGps));
         }
         
         return nodes;
         
     }

     public bool ToBool(string valeurBool)
     {
         if (valeurBool == "true")
             return true;

         return false;
     }

     public GPSCoordinate ToGpsCoordinate(string coordonéeGPS)
     {
         var coord=coordonéeGPS.Split(",", StringSplitOptions.RemoveEmptyEntries);
         
         return new GPSCoordinate(double.Parse(coord[0]), double.Parse(coord[1]));
         
     }
        
     //the nodes input string are in the form 1,2,3
     public List<int> ToList(string nodes )
     {
         List<int> nodeList = new List<int>();
         List<string> nodeListString = new List<string>();
         nodeListString=nodes.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
         foreach (var node in nodeListString)
         {
             nodeList.Add(int.Parse(node, CultureInfo.InvariantCulture));
         }
         return nodeList;
     }

     // public void TrouverPosition()
     // {
     //     
     // }
     //
     // public void 


}
