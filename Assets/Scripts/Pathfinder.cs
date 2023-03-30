using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameAI.Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

namespace GameAI
{
    namespace Pathfinding
    {
        public class Pathfinder
        {
            public List<PathfindingNode> OpenList;
            public List<PathfindingNode> ClosedList;
            //public PathfindingNode ParentNode { get; set; }

            public PathfindingNode Départ { get; set; }

            public PathfindingNode Arrivée { get; set; }
            
            public PathfindingNode NodeActuel { get; set; }
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

            public enum StatutPathfinder
            {
                PAS_INITIALISÉ,
                SUCCES,
                ÉCHEC,
                EN_MARCHE,
            }
            
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

           public List<PathfindingNode> FindPathAStar()
           {
               List<PathfindingNode> path = new List<PathfindingNode>();
               OpenList.Add(Départ);
               path.Add(Départ);
               while (NodeActuel!=Arrivée)
               {
                   NodeActuel = OpenList[GetNodePlusPetitCout(OpenList)];
                   OpenList.Remove(NodeActuel);
                   ClosedList.Add(NodeActuel);
                   foreach (var voisin in NodeActuel.Voisins)
                   {
                       if (!voisin.EstTraversable&&!OpenList.Contains(voisin))
                       {
                           //voisin.Parent = NodeActuel;
                           //float nouveauGCost
                           if (voisin.GCost > 
                               NodeActuel.GCost + Node.CalculerDistanceNodes(NodeActuel, voisin)||
                               !ClosedList.Contains(voisin))
                               
                               voisin.Parent = NodeActuel;
                       }
                   }
               }

               return path;
           }

           // public List<PathfindingNode> FindPathDjikstra()
           // {
           //     
           // }

        }
    }
}