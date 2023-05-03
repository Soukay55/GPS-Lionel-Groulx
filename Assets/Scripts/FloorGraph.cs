using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
//Floor Generation tools
public class FloorGraph
{
    public List<PathfindingNode> Nodes { get; set; }
    
    //private const int NB_NODES_PAR_ÉTAGE = 9;
    
    public FloorGraph()
    {
        Nodes = new List<PathfindingNode>();
    }

    public void Ajouter(PathfindingNode node)
    {
        Nodes.Add(node);
    }
    


    // public void SetPositions()
    // {
    //     foreach (var VARIABLE in COLLECTION)
    //     {
    //         
    //     }
    // }



}