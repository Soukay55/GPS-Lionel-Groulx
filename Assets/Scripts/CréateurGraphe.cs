using System.Collections;
using System.Collections.Generic;
using Graphs;
using UnityEngine;

public class CréateurGraphe : MonoBehaviour
{
    private List<Vertex> sommets;
    void Start()
    {
        
    }

    public void CréerGraphe()
    {
        Delaunay2D.Triangulate(sommets);
    }
}
