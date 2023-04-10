using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

namespace Pathfinding
{
    
    //cas possibles de contraintes:
    //une node à éviter
    //plusieurs nodes à éviter
    //une node à passer
    //plusieurs nodes à passer
    //plusieurs à passer ET une à éviter
    //une node à éviter ET une node à passer
    //plusieurs à passer ETplusieurs à éviter
    //plusieurs à éviter ET une à passer
    //
    public class Pathfinder
    {
        //position de l'utilisateur reçue par le GPS
        public PathfindingNode Départ { get; set; }
        
        
        //destination voulue
        public PathfindingNode Arrivée { get; set; }
        
        
        //self explanatory..
        public List<PathfindingNode> OpenList { get; set; }
        
        public List<PathfindingNode> ClosedList{ get; set; }
        
        
        //node que le pathfinder visite à l'instant présent
        public PathfindingNode NodeActuel { get; set; }
        
        
        //les nodes pour lesquelles l'utilisateur a imposé comme
        //contrainte de ne pas vouloir passer
        public List<PathfindingNode>NodesÀÉviter { get; set; }
        
        
        //chemin optimal trouvé par le pathfinder
        public List<PathfindingNode>Path { get; set; }

        //aucune contrainte (constructeur de path "de base")
        public Pathfinder( PathfindingNode départ,PathfindingNode end)
        {
            Départ=départ ;
            Arrivée = end;

        }
        
        //plusieurs nodes à passer OU à éviter
        public Pathfinder( PathfindingNode départ, PathfindingNode end,
            List<PathfindingNode> nodesChoisis )
        {
            Départ=départ ;
            Arrivée = end;
        }

       

       
        public int GetNodePlusPetitCout(List<PathfindingNode> nodeList)
        {
            int indexPlusPetit = 0;
            float plusPetitCout = nodeList[0].FCost;
            for (int i = 1; i < nodeList.Count; i++)
            {
                //this line ugly af
                if (plusPetitCout >nodeList[i].FCost||(plusPetitCout == nodeList[i].FCost
                                                       &&nodeList[indexPlusPetit].HCost>nodeList[i].HCost))
                {
                    plusPetitCout = nodeList[i].FCost;
                    indexPlusPetit = i;
                }
            }
            //PathfindingNode n = nodeList[indexPlusPetit];
            return indexPlusPetit;
        }

        // public enum StatutPathfinder
        // {
        //     PAS_INITIALISÉ,
        //     SUCCES,
        //     ÉCHEC,
        //     EN_MARCHE,
        // }
            
        //verifier si la node est dans
        //la liste et get le index de la node si ell est dedans
            
        public int EstDansLaListe(List<Node> nodeList, Node node)
        {
            for (int i = 0; i < nodeList.Count; ++i)
            {
                if (nodeList[i].Nombre == node.Nombre && nodeList[i].Niveau == node.Niveau)
                    return i;
            }
            return -1;
        }
        //not a real word
        //public CalculerCoût CoûtTransnodal { get; set; }
                
        //a-star brouillon

       
           
           

        // public List<PathfindingNode> FindPathDjikstra()
        // {
        //     
        // }

    }
}