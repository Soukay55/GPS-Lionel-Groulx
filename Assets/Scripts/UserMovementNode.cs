using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovementNode : PathfindingNode
{
    // Start is called before the first frame update
    public UserMovementNode(int nombre, string nom, bool estEndroitPublic, Étage étage,
        List<int> connectedNodes, GPSCoordinate coordonéesGps) : base(nombre, nom, estEndroitPublic, étage,
        connectedNodes, coordonéesGps)
    {
        
    }

}
