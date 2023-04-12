using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingNode : PathfindingNode,ITéléporteur
{
    public TeleportingNode(int nombre, string nom, bool estEndroitPublic, Étage étage,
        List<float> connectedNodes, GPSCoordinate coordonéesGps):
        base(nombre,nom,estEndroitPublic,étage,connectedNodes,coordonéesGps)
    {
        
    }
}
