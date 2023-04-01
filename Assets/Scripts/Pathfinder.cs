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
    //plusieurs à passer, une à éviter
    //une node à éviter, une node à passer
    //plusieurs à passer,plusieurs à éviter
    //plusieurs à éviter, une à passer
    //
    
    public class Pathfinder
    {
        public PathfindingNode Départ { get; set; }

        public PathfindingNode Arrivée { get; set; }
            
        public PathfindingNode NodeActuel { get; set; }
        
        public PathfindingNode chosenNode { get; set; }

        public List<PathfindingNode> NodesInévitables { get; set;}
        
        public List<PathfindingNode>NodesÀÉviter { get; set; }

        public Pathfinder(Action PathfindingMethod, PathfindingNode départ,PathfindingNode end)
        {
            Départ=départ ;
            Arrivée = end;

        }

        public Pathfinder(Action PathfindingMethod, PathfindingNode départ, PathfindingNode end,
            List<PathfindingNode> nodesInévitables, List<PathfindingNode>nodesÀÉviter )
        {
            
        }

        public Pathfinder(Action PathfindingMethod, PathfindingNode départ,PathfindingNode end, List<PathfindingNode> nodes, bool ÀÉviter)
        {
            
        }

        public Pathfinder(Action PathfindingMethod, PathfindingNode départ,
            PathfindingNode end, PathfindingNode node, bool ÀÉviter)
        {
            
        }
            
        public List<PathfindingNode> OpenList;
        public List<PathfindingNode> ClosedList;
        //public PathfindingNode ParentNode { get; set; }

       
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