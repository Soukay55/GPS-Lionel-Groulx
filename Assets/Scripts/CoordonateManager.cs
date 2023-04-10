using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoordonateManager : MonoBehaviour
{
    //retourne l'index du node le plus près de l'utilisateur
    public int FindStartNode(GPSCoordinate coordonéesUtilisateur, List<Node> Nodes, Étage étageUtilisateur)
    {
        float dist;
        Node plusProche = Nodes[0];
        float distPlusProche=GPSCoordinate.
            CalculerDistanceEntreDeuxCoordonnées(coordonéesUtilisateur, plusProche.CoordonéesGPS);
        foreach (var node in Nodes.Skip(1))
        {
           dist= GPSCoordinate.CalculerDistanceEntreDeuxCoordonnées(coordonéesUtilisateur, node.CoordonéesGPS);
           if (dist < distPlusProche)
           {
               distPlusProche = dist;
               plusProche = node;
           }
        }

        return Nodes.IndexOf(plusProche);
    }
}