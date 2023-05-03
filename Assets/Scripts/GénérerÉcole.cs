using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GénérerÉcole : MonoBehaviour
{
    [SerializeField]
    public GameObject murExt;
    public GameObject murInt;
    public Material matérielSol;
    public Material matérielPath;
    public GameObject couloir;
    
    private const int NB_INFOS_PAR_NODES = 4;
    public List<Node>Points { get; set; }
    public École CollègeLio { get; set; }
    
    //organize this it's ugly
    void Start()
    {
        //maybe this should be done in École so that SetPoints isnt dependent on GetPolygons
        List<Node> points = GetListePoints(FileReadingTools.LireFichierTxt("OutsideData.txt"));
        CollègeLio = new École("Nodes.txt",points[15]);
        //should both be done in École ctor. Bring Points property into école,
        //along with GetListePoints
        
       // print(CollègeLio.Floors[0].Nodes[0].Noms[0]);
        
        CollègeLio.GetPolygons(points);
        CollègeLio.SetPositionsAndGetAiles();
        
        GénérerMursExt(); 
        GénérerMursInt();
        GénérerSols();
        GénérerCouloirs();
        OrganizeHierarchy();
        CréerGrapheÉcole();
        //TestPathfinder();

    }

    public void TestPathfinder()
    {
        List<PathfindingNode> nodesToAvoid = new List<PathfindingNode>();
        nodesToAvoid.Add(CollègeLio.Floors[0].Nodes[25]);

        List<PathfindingNode> nodesToPass = new List<PathfindingNode>();
        nodesToPass.Add(CollègeLio.Floors[0].Nodes[43]);
        nodesToPass.Add(CollègeLio.Floors[0].Nodes[44]);
        nodesToPass.Add(CollègeLio.Floors[0].Nodes[45]);

        DjikstraPathfinder pathfinder =
            new DjikstraPathfinder(CollègeLio.Floors[0].Nodes[2], CollègeLio.Floors[0].Nodes[20], nodesToPass);
        
        // foreach (var node in CollègeLio.Floors[2].Nodes)
        // {
        //     nodesToAvoid.Add(node);
        // }

        // PathfindingNode nodeÀPasser = CollègeLio.Floors[3].Nodes[25];
        // AStarPathfinder pathfinder = new AStarPathfinder(CollègeLio.Floors[0].Nodes[26], CollègeLio.Floors[0].Nodes[41],nodesToAvoid,nodeÀPasser);
        //
        
        int i = 0;
        
        if (pathfinder.Statut == Pathfinder.StatutPathfinder.SUCCES)
        {
            foreach (var node in pathfinder.Path)
            {
                var n = node.DrawNode();
                n.GetComponent<MeshRenderer>().material = matérielPath;
                print("Le node numéro"+i+"du path est le node"+node.Nombre+"de l'étage"+(int)node.Niveau);
                i++;
            }
        }
        else
        {
            Debug.Log("Impossible");
        }
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
            connectedNodes = dataTab[i + 3] == "n" ?  new List<float> { 0 } :
                FileReadingTools.ToList(dataTab[i + 3]);
            
            listePoints.Add(new Node(nombre,nom,coords, connectedNodes));
        }

        //mettre un point arbitraire de l'école à l'origine
        listePoints[15].Position = new Vector3(0, 0, 0);
        
        int k = 1;
        
        foreach (var NODE in listePoints)
        {
            NODE.SetPosition(listePoints[15]);
            NODE.Position = GPSCoordinate.RotateAroundOriginZero(NODE.Position, 46.3f);
            k++;
        }
        
        GetNodes(listePoints[15]);

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

        foreach (var aile in CollègeLio.Ailes)
        {
            GameObject poly = new GameObject(aile.Nom);
            poly.transform.SetParent(outsideWalls.transform);
            aile.DessinerPolygone(murExt, poly);
            CreateurCouloir.DétruireMursIntérieur(poly, mursDétruits);
        }
        
        //ScaleCouloirs(outsideWalls);
        //CreateurCouloir.GénérerCoquilleExterne(outsideWalls, prefab);
    }
    
    //Pour s'assurer que les murs pointent dans la bonne direction, faire un raycast et voir
    //si sa collide avec 
    
    public void GénérerMursInt()
    {

        GameObject insideWalls = new GameObject("MursInt");
        insideWalls.transform.position=Vector3.zero;
        insideWalls.transform.SetParent(transform);
        
        foreach (var polygone in CollègeLio.Ailes)
        {
            DevancerMurs(polygone);
            GameObject poly = new GameObject(polygone.Nom);
            poly.transform.SetParent(insideWalls.transform);
            polygone.DessinerPolygone(murInt,poly);
            
        }
        
    }
    
    public void DevancerMurs(Polygone polygone)
    {
        float largeurMurExt = murExt.transform.GetChild(0).transform.localScale.x;
        Vector3 centroide =polygone.CalculerCentroide();
        List<Vector3> nvPoints = new List<Vector3>();

        foreach (var point in polygone.Points)
        {
            var vecteurPtCentre = (centroide - point).normalized;
            nvPoints.Add(point+vecteurPtCentre*(largeurMurExt));
        }

        polygone.SetPoints(nvPoints);

    }

    //use matrix so it's automatically init with zeros
    public List<List<int>> CréerListeDIndex()
    {
        List<List<int>> listeIndex = new List<List<int>>();
        foreach (var aile in CollègeLio.Ailes)
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
    
    public void GénérerSols()
    {
        //Sol de base
        foreach (var aile in CollègeLio.Ailes)
        {
            aile.CréerSol(matérielSol); 
            aile.Sol.transform.SetParent(GameObject.Find(aile.Nom).transform);
            
        }
        
        //tous les autres étages
        for (int j = 0; j < CollègeLio.Ailes.Count; j++)
        {
            var currentAile = CollègeLio.Ailes[j];
            List<Vector3> pointsDépart = currentAile.Points;
            
            for (int i=1;i<(int)Étage.NombreÉtages&&CollègeLio.Niveaux[i, j] != 0; i++)
            {
                List<Vector3> pointsÀÉtage = new List<Vector3>();
               
                for ( int z = 0; z<pointsDépart.Count; z++)
                {
                    pointsÀÉtage.Add
                        (pointsDépart[z] +
                         Vector3.up*CollègeLio.Niveaux[i, j]);
                }
                currentAile.SetPoints(pointsÀÉtage);
                currentAile.CréerSol(matérielSol);
                currentAile.Sol.transform.SetParent
                    (GameObject.Find(currentAile.Nom).transform);
            }
            currentAile.SetPoints(pointsDépart);
            
        }

    }

    public void OrganizeHierarchy()
    {
        
    }

    //constant for earth rotation (46,3deg)
    //this func should b in école
    public void GetNodes(Node origine)
    {
        int i = 1;
        var points = new List<Node>();
        var coords=FileReadingTools.LireFichierTxt("InsideData.txt");
        
        foreach (var coord in coords)
        {
           var c=new Node(i,FileReadingTools.ToGpsCoordinate(coord));
           c.SetPosition(origine);
           c.Position = GPSCoordinate.RotateAroundOriginZero(c.Position, 46.3f);
           points.Add(c);
           i++;
        }

        Points = points;
    }
    
    //maybe create delegate for this instead
    
    
    public void GénérerCouloirs()
    {
        for (int i = 0; i < CollègeLio.Floors.Count; i++)
        {
            var currentFloor = CollègeLio.Floors[i];
            
            for(int j=0;j<currentFloor.Nodes.Count;j++)
            {
                var currentNode = currentFloor.Nodes[j];
                print(currentNode.Nombre);
                if (!EstCageDEscalier(currentNode))
                {
                    foreach (var voisin in currentNode.Voisins)
                    {
                        CreateurCouloir.CréerCouloir(currentNode.Position
                            , voisin.Position, couloir);
                    }  
                }
            }
        }
    }

    //both of these in the PathfindinNode class
    public bool EstCageDEscalier(PathfindingNode node)
    {
        if (AUnEscalier(node))
        {
            foreach (var voisin in node.Voisins)
            {
                if (AUnEscalier(voisin)) return true;
            }
        }
        
        return false;
    }

    public bool AUnEscalier(PathfindingNode node)
    {
        foreach (var nom in node.Noms)
        {
            if (nom.Contains("Escalier"))
            {
                return true;
            }
            
        }

        return false;
    }
    //well see ://///
    public void CréerGrapheÉcole()
    {
        GameObject graphe = new GameObject("GrapheÉcole");
        
        for (int i = 0; i < CollègeLio.Floors.Count; i++)
        {
            FloorGraph floor = CollègeLio.Floors[i];

            for (int j = 0; j < floor.Nodes.Count;j++)
            {
                var node = floor.Nodes[j];
                
                var point = node.DrawNode();
                point.transform.SetParent(graphe.transform);
                point.name = node.Nom + " étage: " + (int)node.Niveau + "numéro: " + node.Nombre;
                var l1 = point.AddComponent<LineRenderer>();
                l1.positionCount = node.Voisins.Count * 2;
                var k = 0;
                foreach(var voisin in node.Voisins)
                {
                    // Debug.Log("les voisins de "+(int)node.Niveau+"au numéro"+node.Nombre+"ont"+
                    //           "détage"+(int)voisin.Niveau+"numéro"+voisin.Nombre);
                    
                    l1.SetPosition(k,point.transform.position);
                    l1.SetPosition(k+1,voisin.Position);
                    l1.SetWidth(6,6);
                    
                    k += 2;
                }
            }
        }
    }
}
