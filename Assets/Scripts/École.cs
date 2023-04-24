using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class École
{
    //GetNodeData should be in here and not in FloorGraph???
    public List<FloorGraph> Floors { get; set; }

    public List<string> dataTab;
    private const int NB_DONNÉES_PAR_NODE = 6;
    public bool plusQueUnNom = false;

    private List<PathfindingNode> nodes;
    private List<PathfindingNode> nodesToAdd;
    public FloorGraph floor;
    

    void Awake()
    {
        //CréerÉcole();
    }

    public École()
    {
        Floors = new List<FloorGraph>();
        
        for (int i = 0; i <(int) Étage.NombreÉtages; i++)
        {
            Floors.Add(new FloorGraph());
        }
    }

    public List<PathfindingNode> GetNodeData(string fichierNodesName)
    {
        dataTab = FileReadingTools.LireFichierTxt(fichierNodesName);

        var étageComparateur = Étage.A;
        var cpt = 1;
        
        
        for (var i = 0; i < dataTab.Count - 1; i += NB_DONNÉES_PAR_NODE)
        {
            var nombre = int.Parse(dataTab[i], CultureInfo.InvariantCulture);

            var nom = FileReadingTools.ToNameList(dataTab[i + 1], ref plusQueUnNom)[0];

            var endroitPublic = FileReadingTools.ToBool(dataTab[i + 2]);

            var étage = FileReadingTools.ToÉtage(dataTab[i + 3]);

            var connectedNodes = FileReadingTools.ToList(dataTab[i + 4]);

            var coordonéesGps = FileReadingTools.ToGpsCoordinate(dataTab[i + 5]);
            
            floor = Floors[(int)étage];

            //lorsqu'on atteint la fin de l'étage, on rajoute les nodesÀAjouter.
            //On doit ajouter chaque node à la liste de connectedNodes
            if (étage > étageComparateur)
            {
                foreach (var node in nodesToAdd)
                {
                    connectedNodes.Add(node.Nombre);
                    nodes[node.Nombre + nodes.Count - 1].ConnectedNodes.Add(cpt);
                    node.Nombre = cpt;
                    cpt++;
                }
                
                Floors[(int)étage-1].Ajouter(nodesToAdd);
                
                //O as in the letter
                if (étage != Étage.O)
                    étageComparateur++;
                
                //cpt représente le numéro du node dans son étage respectif
                cpt = 1;
                

            }
            
            floor.Ajouter(new PathfindingNode(nombre, nom, endroitPublic, étage, connectedNodes, coordonéesGps));
            cpt++;
            if (plusQueUnNom = true)
            {
                var nameList = FileReadingTools.ToNameList(dataTab[i + 1], ref plusQueUnNom);
                //THE NUMBER ISNT THE SAME FOR ALL OF THEM!
                //WHEN ADAPT NUMBER, THE CONNECTED NODES FOR EACH NODE NEEDS
                foreach (var name in nameList.Skip(1))
                {
                    nodesToAdd.Add(new PathfindingNode(nombre, name, EstEndroitPublic(name),
                        étage, connectedNodes, coordonéesGps));
                }
            }
        }

        return nodes;
    }

    public void AjouterÉtage()
    {
    }

    public bool EstEndroitPublic(string nom)
    {
        List<string> endroitsPub
            = new List<string>() { "Escaliers", "Carrefour", "Cafétéria", "Entre2" };
        return endroitsPub.Contains(nom);
    }
}