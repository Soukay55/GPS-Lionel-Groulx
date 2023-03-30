using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserMovementNode : PathfindingNode
{
    [SerializeField] public static int nbFPS;
    // Start is called before the first frame update
    public UserMovementNode(int nombre, string nom, bool estEndroitPublic, Étage étage,
        List<int> connectedNodes, GPSCoordinate coordonéesGps) : base(nombre, nom, estEndroitPublic, étage,
        connectedNodes, coordonéesGps)
    {
        
    }

    static List<UserMovementNode>GénérerPointsEntreNodes(List<PathfindingNode>path)
    {
        List<UserMovementNode> userMovement = new List<UserMovementNode>();
        for (int i = 0; i <path.Count-1; i++)
        {
            
            // for (int j = 0; j < nbFPS; j++)
            // {
            //     userMovement[j]=
            // }
            
        }
        return userMovement;
    }
    
    // public List<PathfindingNode>GetNodesBetween(Node A, Node B, int nbNodes)
    // {
    //     float distance = CalculerDistanceNodes(A, B);
    //     for (int j = 0;j<nbNodes;j++)
    //     {
    //         nodes.Add(CopierNode(Node A))
    //     }
    //     List<PathfindingNode> nodes = new List<PathfindingNode>();
    //     
    //
    //     return nodes;
    //
    // }

    public PathfindingNode CopierNode( PathfindingNode àCopier)
    {
        PathfindingNode node = new PathfindingNode
        (àCopier.Nombre, àCopier.Nom, àCopier.EstEndroitPublic, àCopier.Niveau,
            àCopier.ConnectedNodes, àCopier.CoordonéesGPS);
        return node;

    }
}