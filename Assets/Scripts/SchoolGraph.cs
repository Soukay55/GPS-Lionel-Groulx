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
using Graphs;
public class SchoolGraph : MonoBehaviour
{
    private void Start()
    {
        SchoolGraph GrapheÉcole = new SchoolGraph(GetNodeData("Nodes"));
    }

    private StreamReader fluxLecture;
    private const string PATH = "Assets/RessourcesGPS/DataNodes";
    //private const int NB_NODES_PAR_ÉTAGE = 9;
    private const int NB_DONNÉES_PAR_NODE = 6;
    private string[] délimiteurs = {"\t"};
    private List<string>dataTab;
    
    private List<Node>Nodes;
    private List<Node>nodesToAdd;// pour get les sommets c node.point so faut
    // add le calcul soit ds graphe ou node idk yet

    public SchoolGraph(List<Node>nodes)
    {
        List<Node> Nodes = nodes;
    }
     public List<Node>GetNodeData(string fichierNodesName)
    {
        var fichierÀlire = $"{PATH}/{fichierNodesName}";
        fluxLecture = new StreamReader(fichierÀlire);
         
        List<Node>nodes = new List<Node>();
         
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
            string nom = ToNameList((dataTab[i + 1]))[0];
            bool endroitPublic = ToBool(dataTab[i + 2]);
            Étage étage = ToÉtage(dataTab[i + 3]);
            List<int> connectedNodes = ToList(dataTab[i + 4]);
            GPSCoordinate coordonéesGps = ToGpsCoordinate(dataTab[i + 5]);
            
            nodes.Add(new GameNode(nombre,nom,endroitPublic,étage,connectedNodes,coordonéesGps));
        }
         
        return nodes;
         
    }

    public bool ToBool(string valeurBool)
    {
        if (valeurBool == "true")
            return true;

        return false;
    }

    public List<string> ToNameList(string listeNoms)
    {
        return listeNoms.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
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

    public Étage ToÉtage(string étage)
    {
        string étages = "ABGHIO";
        return (Étage)étages.IndexOf(étage);    


    }
}
