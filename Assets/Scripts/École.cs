using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class École
{
    //take care of commented part of GetNodeData
    public List<FloorGraph> Floors { get; set; }
    public List<Node>CoordonéesÉcole{ get; set; }
    
    public const int HAUTEUR_ÉCOLE = 500;

    private const int NB_DONNÉES_PAR_NODE = 5;
    public int[,]Niveaux { get; set; }
    public List<Polygone>Ailes { get; set; }
    
    public École(string fichierNom,Node origine)
    {
        Floors = new List<FloorGraph>();
        Niveaux = new int[(int)Étage.NombreÉtages, (int)Aile.NombreAiles];
        
        for (int i = 0; i <(int) Étage.NombreÉtages; i++)
        {
            Floors.Add(new FloorGraph());
        }
        
        RemplirMatriceÉtages();
        GetCoordonéesÉcole(origine);
        GetNodeData(fichierNom);
        
    }
    
    public void GetPolygons(List<Node>pointList)
    {
        List<Polygone> polygones = new List<Polygone>();

        foreach (var node in pointList)
        {
            if (node.ConnectedNodes[0] != 0)
            {
                polygones.Add( Polygone.GetPolygon(node, pointList));
            }
        }
        
        Ailes = polygones;
    }
    
    public void GetNodeData(string fichierNodesName)
    {
        var dataTab = FileReadingTools.LireFichierTxt(fichierNodesName);

        var étageComparateur = Étage.A;
        
        
        //voisins conversion problem
        for (var i = 0; i < dataTab.Count - 1; i += NB_DONNÉES_PAR_NODE)
        {
            var nombre = int.Parse(dataTab[i], CultureInfo.InvariantCulture);

            var noms = FileReadingTools.ToNameList(dataTab[i + 1]);

            noms = noms.Concat(CoordonéesÉcole[nombre - 1].Noms).ToList();

            var endroitPublic = FileReadingTools.ToBool(dataTab[i + 2]);

            var étage = FileReadingTools.ToÉtage(dataTab[i + 3]);

            var connectedNodes = FileReadingTools.ToList(dataTab[i + 4]);

            var position= CoordonéesÉcole[nombre-1].Position;

            var coordonéesGPS = CoordonéesÉcole[nombre - 1].CoordonéesGPS;
            
            //make them game nodes they are not used for pathfinding here
            Floors[(int)étage].Ajouter
                (new PathfindingNode(nombre, noms, endroitPublic, étage, connectedNodes, position,coordonéesGPS));
            
        }
        
    }
    
    
    //explain this in comments
    public void RemplirMatriceÉtages()
    {
        int[,] matriceÉtages = new int[(int)Étage.NombreÉtages,(int)Aile.NombreAiles];
        
        //étage auquel s'arrête chaque aile, selon index
        
        int[] étagesMax = { 3, 4, 4, 1, 0, 0, 0, 5, 1, 0, 4 };
        
        //irrégularité de l'AileC
        matriceÉtages[1,4]=HAUTEUR_ÉCOLE/((int)Étage.NombreÉtages*2);
        
        for (int i = 1; i < (int)Étage.NombreÉtages; i++)
        {
            for (int j = 0; j < (int)Aile.NombreAiles; j++)
            {
                
                if (étagesMax[j] >= i)
                {
               
                    //exceptions Sauvé-Frenette
                    if(i>2&&j>6)
                    {
                        matriceÉtages[i, j] = matriceÉtages[i - 1, j] + HAUTEUR_ÉCOLE
                            / (int)Étage.NombreÉtages-1;
                    }
                    else
                    {
                        if(i==2&&j<6)
                        {
                            matriceÉtages[i, j] = matriceÉtages[i - 1, j] + 2*HAUTEUR_ÉCOLE
                                / ((int)Étage.NombreÉtages);
                            continue;
                        }
                        matriceÉtages[i, j] = matriceÉtages[i - 1, j] + HAUTEUR_ÉCOLE
                            / ((int)Étage.NombreÉtages);
                    }
                }
            }
        }

        Niveaux = matriceÉtages;

    }
    
    public void GetCoordonéesÉcole(Node origine)
    {
        
        var points = new List<Node>();
        var coords=FileReadingTools.LireFichierTxt("InsideData.txt");
        
        for(int i=0,j = 1;i<coords.Count-1;i+=2,j++)
        {
            var c=new Node(j,FileReadingTools.ToGpsCoordinate(coords[i]),FileReadingTools.ToNameList(coords[i+1]));
            c.SetPosition(origine);
            c.Position = GPSCoordinate.RotateAroundOriginZero(c.Position, 46.3f);
            points.Add(c);
          
        }

        CoordonéesÉcole = points;
    }

    //can only be called AFTER SetPolygons. Modify code in GénérerÉcole
    //so that you can get polygons here
    public void SetPositionsAndGetAiles()
    {
        for (int i = 0; i < Floors.Count; i++)
        {
            FloorGraph floor = Floors[i];
            for (int j = 0; j < floor.Nodes.Count; j++)
            {
                var node = floor.Nodes[j];
                node.GetAile(Ailes);
                GetNeighbours(node);   
                
                if(i==0&&(j==51||j==63))
                {
                    node.Position +=                             
                        Vector3.up * Niveaux[i+1, (int)Aile.C]; 
                    continue;
                }  
                
                node.Position +=
                    Vector3.up * Niveaux[i, (int)node.Aile];
            }

        }
    }
    
    public void GetNeighbours(PathfindingNode node)
    {
        foreach (var connectedNode in node.ConnectedNodes)
        {
            var floor = Mathf.RoundToInt((connectedNode-(int)connectedNode)*10);
            int index;

            for (int i = 0; i <Floors[floor].Nodes.Count; i++)
            {
                if (Floors[floor].Nodes[i].Nombre==(int)(connectedNode))
                {
                    index = i;
                    node.Voisins.Add(Floors[floor].Nodes[index]);
                    break;
                }
            
            }
        }
    }

}

public enum Étage
{
    A,
    B,
    G,
    H,
    I,
    O,
    NombreÉtages
}

public enum Aile
{
    N,
    D,
    L,
    E,
    C,
    CM,
    X,
    S,
    M,
    K,
    F,
    NombreAiles
}