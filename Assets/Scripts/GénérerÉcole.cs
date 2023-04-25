////using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class GénérerÉcole : MonoBehaviour
{
    [SerializeField]
    public GameObject murExt;
    public GameObject murInt;
    public GameObject prefab;
    
    private const int NB_INFOS_PAR_NODES = 4;
    
    private List<Polygone>Ailes { get; set;}

    void Start()
    {
        List<Node> points = GetListePoints(FileReadingTools.LireFichierTxt("OutsideData.txt"));
        GetPolygons(points);
        GénérerMursExt(); 
        GénérerMursInt();
       // GénérerSols();
        
    }
    
    public void GetPolygons(List<Node>pointList)
    {
        List<Polygone> polygones = new List<Polygone>();

        foreach (var node in pointList)
        {
            if (node.ConnectedNodes[0] != 0)
                polygones.Add( Polygone.GetPolygon(node, pointList));
        }
        Ailes = polygones;
    }
    public List<Node> GetListePoints(List<string>dataTab)
    {
        List<Node> listePoints = new List<Node>();
        
        GPSCoordinate coords;
        int nombre;
        string nom;
        
        List<float> connectedNodes = new List<float>();
        
        for (int i = 0; i < dataTab.Count; i+=NB_INFOS_PAR_NODES)
        {
            nombre = int.Parse(dataTab[i], CultureInfo.InvariantCulture);
            coords = FileReadingTools.ToGpsCoordinate(dataTab[i+1].Replace(" ", ""));
            nom = dataTab[i+2];
            connectedNodes = dataTab[i + 3] == "n" ?  new List<float>() { 0 } :
                FileReadingTools.ToList(dataTab[i + 3]);
            
            listePoints.Add(new Node(nombre,nom,coords, connectedNodes));
        }

        listePoints[15].Position = new Vector3(0, 0, 0);
        int k = 1;
        foreach (var NODE in listePoints)
        {
            NODE.SetPosition(listePoints[15]);
            NODE.Position = GPSCoordinate.RotateAroundOriginZero(NODE.Position, 46.3f);
            k++;
        }

        return listePoints;
    }
    
    public void GénérerMursExt()
    {
        //si deux points appartiennent à deux polygones en même temps,
        //ca veut dire qu'ils connectent une aile à une autre, et il ne
        //faut donc pas de mur entre les deux.
        GameObject outsideWalls = new GameObject("MursExt");
        outsideWalls.transform.position=Vector3.zero;
        outsideWalls.transform.SetParent(transform);
        
        //pour keep track du nombre de murs détruits par couloir, pcq ils restent
        //encore en mémoire après la destruction
        var mursDétruits = CréerListeDIndex();

        foreach (var aile in Ailes)
        {
            GameObject poly = new GameObject(aile.Nom);
            poly.transform.SetParent(outsideWalls.transform);
            aile.DessinerPolygone(murExt, poly);
            CreateurCouloir.DétruireMursIntérieur(poly, mursDétruits);
            
        }

        for (int j = 0; j < outsideWalls.transform.childCount; j++)
        {
            var polygone = outsideWalls.transform.GetChild(j);

            for (int i = 0; i < polygone.childCount; i++)
            {

                var couloir = polygone.GetChild(i);

                if (couloir.childCount != 0)
                {
                    var s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    s.transform.position = couloir.transform.position;
                    s.transform.localScale = Vector3.one * 30;
                }
            }
        }
        //ScaleCouloirs(outsideWalls);
        
        //CreateurCouloir.GénérerCoquilleExterne(outsideWalls, prefab);
    }
    
    //Pour s'assurer que les murs pointent dans la bonne direction, faire un raycast et voir
    //si sa collide avec CoquilleExt
    public void GénérerMursInt()
    {

        GameObject insideWalls = new GameObject("MursInt");
        insideWalls.transform.position=Vector3.zero;
        insideWalls.transform.SetParent(transform);
        
        foreach (var polygone in Ailes)
        {
            GameObject poly = new GameObject(polygone.Nom);
            poly.transform.SetParent(insideWalls.transform);
            polygone.DessinerPolygone(murInt,poly);
            DevancerMurs(poly);
        }
        
    }

    public void GénérerSols()
    {
        foreach (var aile in Ailes)
        {
            aile.CréerSol();
            aile.Sol.transform.SetParent(GameObject.Find(aile.Nom).transform);
        }
    }

    public void DevancerMurs(GameObject polygone)
    {
        float largeurMurExt = murExt.transform.GetChild(0).transform.localScale.x;
        for (int i = 0; i < polygone.transform.childCount; i++)
        {
            Transform mur = polygone.transform.GetChild(i).transform;
            for (int j = 0; j < mur.childCount; j++)
            {
                var blocMur = mur.GetChild(j);
                blocMur.localPosition += new Vector3(largeurMurExt/2 ,0,0);
            }
        }
        
    }

    public List<List<int>> CréerListeDIndex()
    {
        List<List<int>> listeIndex = new List<List<int>>();
        foreach (var aile in Ailes)
        {
            List<int> valeursDépart = new List<int>();
            for (int i = 0; i < aile.Points.Count-1; i++)
            {
                valeursDépart.Add(0);
            }
            listeIndex.Add(valeursDépart);
        }

        return listeIndex;
    }
}
