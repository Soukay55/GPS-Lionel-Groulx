using System.Collections;
using System.Collections.Generic;
using GameAI.Pathfinding;
using UnityEngine;

namespace GameAI
{
    namespace Pathfinding
    {
        public class Pathfinder
        {
            public PathfindingNode ParentNode { get; set; }
            
            public PathfindingNode Départ { get; set; }

            public PathfindingNode Arrivée { get; set; }

            /*public enum StatutPathfinder
            {
                PAS_INITIALISÉ,
                SUCCES,
                ÉCHEC,
                EN_MARCHE,
            }
            abstract public class PathFinder<T>
            {
                public Node Départ { get; private set; }
                public Node Destination { get; private set; }
                
                //check what needs to b unprivated/privated
            
                public StatutPathfinder Statut
                {
                    get;
                    set;
                    
                } = StatutPathfinder.PAS_INITIALISÉ;
    
                public delegate float CalculerCoût(Node a, Node b);
                public CalculerCoût CoûtHeuristique { get; set; }
                //not a real word
                public CalculerCoût CoûtTransnodal { get; set; }
                
                
                //pt mettre node class abstraite pr que ca herite de Node
                public class NodePathfinder
                {
                    //la node jst avant quon arrive au nodepathfindr
                    public NodePathfinder Parent { get; set; }
                    public Vector3 Location { get; set; }
                    
                }
    
            }
            */
        }
    }

    
}
