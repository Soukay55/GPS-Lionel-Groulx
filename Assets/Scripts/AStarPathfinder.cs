using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
//cas possibles de contraintes avec cet algorithme:
//une node à éviter
//plusieurs nodes à éviter
//une node à passer
//une node à éviter ET une node à passer
//plusieurs à éviter ET une à passer
//aucune contrainte

//un AStarPathfinder retourne un path.
public class AStarPathfinder : Pathfinder
{
    public PathfindingNode ChosenNode { get; set; }
    
    //aucune contrainte, mais plusieurs paths différents à créer
    public AStarPathfinder() { }
    
    //aucune contrainte
    public AStarPathfinder(PathfindingNode départ, PathfindingNode end)
    :base(départ,end)
    {
        Path = FindPathAStar();
    }
    
    //plusieurs nodes à éviter OU une node à éviter
    public AStarPathfinder(PathfindingNode départ, PathfindingNode end,
        List<PathfindingNode> nodes)
        :base(départ,end,nodes)
    {
        NodesÀÉviter = nodes;
        
        //ecq ça devrait être fait avant d'appeler le constructeur?..
        foreach (var node in nodes)
        {
            node.EstTraversable = false;
        }
        Path = FindPathAStar();//!
    }
    
    //une node à passer
    public AStarPathfinder( PathfindingNode départ,
        PathfindingNode end, PathfindingNode node):base(départ,end)
    {
        //ChosenNode property is redundant rn
        ChosenNode = node;
        Path = FindPathAStar(ChosenNode);
    }
    
    //(une node à éviter OU plusieurs node à éviter) ET une node à passer
    public AStarPathfinder(PathfindingNode départ,
        PathfindingNode end, List<PathfindingNode> nodesÀÉviter,PathfindingNode nodeÀPasser)
    :base(départ, end,nodesÀÉviter)
    {
        foreach (var node in nodesÀÉviter)
        {
            node.EstTraversable = false;
        }
        
        NodesÀÉviter = nodesÀÉviter;
        ChosenNode = nodeÀPasser;
        Path = FindPathAStar(ChosenNode);
    }


    public List<PathfindingNode> FindPathAStar()
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        OpenList.Add(Départ);
        while (NodeActuel!=Arrivée)
        {
            if (OpenList.Count == 0)
            {
                Statut = StatutPathfinder.ÉCHEC;
                return null;
            }

            Statut = StatutPathfinder.EN_MARCHE;
            
            NodeActuel = OpenList[GetNodePlusPetitCout(OpenList)];
            OpenList.Remove(NodeActuel);
            ClosedList.Add(NodeActuel);
            
            
            //optimization?
            foreach (var voisin in NodeActuel.Voisins)
            {
                if (voisin.EstTraversable&&!ClosedList.Contains(voisin))
                {
                    if (voisin.GCost >
                        NodeActuel.GCost + Node.CalculerDistanceNodes
                            (NodeActuel, voisin)|| !OpenList.Contains(voisin))
                    {
                        voisin.Parent = NodeActuel;
                        voisin.GCost = voisin.GetGCost();

                        if (!OpenList.Contains(voisin))
                        {
                            OpenList.Add(voisin);
                        }
                    }
                    
                }
            }
        }

        Statut = StatutPathfinder.SUCCES;
        
        while (NodeActuel.Parent != null)
        {
            path.Add(NodeActuel.Parent);
            NodeActuel = NodeActuel.Parent;
        }
        
        path.Reverse();
        path.Add(Arrivée);
        
        OpenList.Clear();
        ClosedList.Clear();
        
        Unparent(path);
        
        return path; 
    }

    public void Unparent(List<PathfindingNode>nodes)
    {
        foreach (var node in nodes)
        {
            if (node.Parent != null) node.Parent = null;
        }
    }

    public List<PathfindingNode> FindPathAStar(PathfindingNode nodeÀPasser)
    {
        //la destination finale de l'utilisateur est préservée
        PathfindingNode contenantTempo = Arrivée;
        
        //on doit "séparer" le chemin en deux
        List<PathfindingNode> path1 = new List<PathfindingNode>();
        List<PathfindingNode> path2 = new List<PathfindingNode>();
        
        //on trouve le premier chemin qui part
        //de la position de l'utilisateur à la position du point à passer
        Arrivée = nodeÀPasser;
        path1 = FindPathAStar();
        
        //on trouve le deuxième chemin,
        //qui part de la position de la node à passer, et qui arrive
        //à la position finale
        Départ = nodeÀPasser;
        Arrivée = contenantTempo;

        //before doing this:bool Repasser
        foreach (var node in path1.Skip(1))
        {
            if((path1.Count-1)-path1.IndexOf(node)<3)
            node.EstTraversable = false;
        }
        
        
        path2 = FindPathAStar();
        
        //on enlève la nodeÀPasser, puisqu'elle apparaît dans les deux chemins
        Debug.Log(Statut);
        path2.Remove(nodeÀPasser);
        
        //on colle les deux chemins ensemble
        return path1.Concat(path2).ToList();
    }
    
    public override int GetNodePlusPetitCout(List<PathfindingNode> nodeList)
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
}
