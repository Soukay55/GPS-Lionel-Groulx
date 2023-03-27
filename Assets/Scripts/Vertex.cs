using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : Node
{
    public string Code { get; set; }
    // Start is called before the first frame update
    public Vertex(int nombre, string nom, Étage étage,
        List<int> connectedNodes, GPSCoordinate coordonéesGps) : base(nombre, nom, étage, coordonéesGps)
    {
    }

    public void SetCode(string code)
    {
        Code = code;
    }
}
