using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class FloorGraph : MonoBehaviour
{
    private void Start()
    {
        var GrapheÉcole = new FloorGraph(GetNodeData("Nodes"));
    }

    private StreamReader fluxLecture;

    private const string PATH = "Assets/RessourcesGPS/DataNodes";

    //private const int NB_NODES_PAR_ÉTAGE = 9;
    private const int NB_DONNÉES_PAR_NODE = 6;
    private string[] délimiteurs = { "\t" };
    private List<string> dataTab;
    private bool plusQueUnNom = false;
    private List<Node> Nodes;

    private List<Node> nodesToAdd; // pour get les sommets c node.point so faut
    // add le calcul soit ds graphe ou node idk yet
    public FloorGraph(List<PathfindingNode> nodes)
    {
        var Nodes = nodes;
    }

    public List<PathfindingNode> GetNodeData(string fichierNodesName)
    {
        //all of this needs to be done in a separate function
        var fichierÀlire = $"{PATH}/{fichierNodesName}";
        fluxLecture = new StreamReader(fichierÀlire);

        var nodes = new List<PathfindingNode>();
        nodesToAdd = new List<Node>();
        //edit linked nodes:only stairs can move to higher levels
        var données = string.Empty;
        using (fluxLecture)
        {
            string ligne;
            while ((ligne = fluxLecture.ReadLine()) != null)
                données += ligne + "\t";
        }

        dataTab = données.Split("\t", StringSplitOptions.RemoveEmptyEntries).ToList();
        //all of the above in a separate method
        
        var étageComparateur = Étage.A;
        var cpt = 1;

        for (var i = 0; i < dataTab.Count - 1; i += NB_DONNÉES_PAR_NODE)
        {
            var nombre = int.Parse(dataTab[i], CultureInfo.InvariantCulture);

            var nom = ToNameList(dataTab[i + 1])[0];

            var endroitPublic = ToBool(dataTab[i + 2]);
            
            var étage = ToÉtage(dataTab[i + 3]);
            
            var connectedNodes = ToList(dataTab[i + 4]);
            
            var coordonéesGps = ToGpsCoordinate(dataTab[i + 5]);
            
            //lorsqu'on atteint la fin de l'étage, on rajoute les nodesÀAjouter.
            //On doit ajouter chaque node à la liste connectedNode
            if (étage > étageComparateur)
            {
                foreach (var node in nodesToAdd)
                {
                    connectedNodes.Add(node.Nombre);
                    nodes[node.Nombre+nodes.Count-1].ConnectedNodes.Add(cpt);
                    node.Nombre = cpt;
                    cpt++;
                }
                nodes.Concat(nodesToAdd).ToList();
                if (étage != Étage.O)
                    étageComparateur++;
                //cpt représente le numéro du node dans son étage
                cpt = 1;
            }

            nodes.Add(new PathfindingNode(nombre, nom, endroitPublic, étage, connectedNodes, coordonéesGps));
            cpt++;
            if (plusQueUnNom = true)
            { 
                var nameList = ToNameList(dataTab[i + 1]);
                //THE NUMBER ISNT THE SAME FOR ALL OF THEM!
                //WHEN ADAPT NUMBER, THE CONNECTED NODES FOR EACH NODE NEEDS
                foreach (var name in nameList.Skip(1))
                {
                    nodesToAdd.Add(new GameNode(nombre, name, EstEndroitPublic(name),
                        étage, connectedNodes, coordonéesGps));
                }
                    
            }
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
        var noms = listeNoms.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
         if (noms.Count > 1)
            plusQueUnNom = true;
        return noms;
    }

    public GPSCoordinate ToGpsCoordinate(string coordonéeGPS)
    {
        var coord = coordonéeGPS.Split(",", StringSplitOptions.RemoveEmptyEntries);

        return new GPSCoordinate(double.Parse(coord[0]), double.Parse(coord[1]));
    }

    //the nodes input string are in the form 1,2,3
    public List<float> ToList(string nodes)
    {
        var nodeList = new List<float>();
        var nodeListString = new List<string>();
        nodeListString = nodes.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (var node in nodeListString) nodeList.Add(float.Parse(node, CultureInfo.InvariantCulture));
        return nodeList;
    }

    public Étage ToÉtage(string étage)
    {
        var étages = "ABGHIO";
        return (Étage)étages.IndexOf(étage);
    }

    public bool EstEndroitPublic(string nom)
    {
        List<string> endroitsPub
            = new List<string>() { "Escaliers", "Carrefour", "Cafétéria","Entre2" };
        return endroitsPub.Contains(nom);
    }

}