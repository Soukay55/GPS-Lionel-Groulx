using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mapbox.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GénérerÉcole : MonoBehaviour
{
    [SerializeField]
    public GameObject murExt;
    public GameObject murInt;
    public Material matérielSol;
    public Material matérielPlafonds;
    public Material matérielPath;
    public GameObject couloirPrefab;
    public GameObject salle;
    public GameObject tablesEtChaises;
    public GameObject tablesRondesEtChaises;
    public Material testCollider;
    public GameObject portes;
    public GameObject casiers;
    
    private const int NB_INFOS_PAR_NODES = 4;
    public École CollègeLio { get; set; }
    
    //organize this it's ugly
    void Start()
    {
        List<Node> points = GetListePoints(FileReadingTools.LireFichierTxt("OutsideData.txt"));
        CollègeLio = new École("Nodes.txt",points[15]);

        CollègeLio.GetPolygons(points);
        CollègeLio.SetPositionsAndGetAiles();
        
        GénérerMursExt();
        GénérerMursInt();
        GénérerSols();
        GénérerPlafonds();
        GénérerCouloirs();
        GénérerSallesPubliques();
        CréerGrapheÉcole();
        OrganizeHierarchy();
        
        //TestPathfinder();

    }
    

    public void TestPathfinder()
    {
        List<PathfindingNode> nodesToAvoid = new List<PathfindingNode>();
        nodesToAvoid.Add(CollègeLio.Floors[0].Nodes[25]);

        List<PathfindingNode> nodesToPass = new List<PathfindingNode>();
        nodesToPass.Add(CollègeLio.Floors[0].Nodes[43]);
        nodesToPass.Add(CollègeLio.Floors[0].Nodes[44]);
        nodesToAvoid.Add(CollègeLio.Floors[2].Nodes[4]);

       // DjikstraPathfinder pathfinder =
         //   new DjikstraPathfinder(CollègeLio.Floors[0].Nodes[2], CollègeLio.Floors[0].Nodes[20], nodesToPass);
        
        foreach (var node in CollègeLio.Floors[2].Nodes)
        {
            //nodesToAvoid.Add(node);
        }

        PathfindingNode nodeÀPasser = CollègeLio.Floors[3].Nodes[25];
        AStarPathfinder pathfinder = new AStarPathfinder(CollègeLio.Floors[0].Nodes[18], CollègeLio.Floors[2].Nodes[28],nodesToPass[0]);
        
        
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
            aile.Sol.transform.SetParent(GameObject.Find("Étage0").transform);
            
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
                    (GameObject.Find("Étage"+i).transform);
            }
            currentAile.SetPoints(pointsDépart);
            
        }

    }
    
    public void GénérerPlafonds()
    {
        
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
                currentAile.CréerPlafonds(matérielPlafonds);
                currentAile.Plafond.transform.SetParent
                    (GameObject.Find("Étage"+(i-1)).transform);
            }
            currentAile.SetPoints(pointsDépart);
            
        }
    }

    //s'assurer d'un chemin clair entre chaque node et son voisin


    public void OrganizeHierarchy()
    {
        // for (int i = 0; i <(int) Étage.NombreÉtages; i++)
        // {
        //     var cuurentFloot=GameObject.Find
        // }
        //
    }
    

    public void GénérerCouloirs()
    {
        
        for (int i = 0; i < CollègeLio.Floors.Count; i++)
        {

            var couloirsÉtage = GameObject.Find("Étage" + i);
            
            var currentFloor = CollègeLio.Floors[i];
            
            List<string> couloirsLeft = new List<string>();
            
            foreach (var node in currentFloor.Nodes)
            {
                if (EstUn("Couloir", node))
                {
                    couloirsLeft=couloirsLeft.Concat(node.Noms.FindAll(FindCouloirs)).ToList();
                }
                
            }
            couloirsLeft=couloirsLeft.Distinct().ToList();
            
            
            foreach (var couloir in couloirsLeft)
            {
                List<Vector3> currentCouloir = new List<Vector3>();

                foreach (var node in currentFloor.Nodes)
                {

                    if (EstUn(couloir, node) && !EstUneSalle(node))
                    {
                        if (i >= 2 &&(couloir.Contains("S")||couloir.Contains("F")))
                        {
                            if (node.Aile == Aile.L)
                            {
                                if (i == 2) continue;
                                
                                var lowerNode=FindLowerNode(CollègeLio.Floors[i-1].Nodes,node);
                                if (lowerNode != null && EstUn(couloir, lowerNode))
                                {
                                    currentCouloir.Add(lowerNode.Position);
                            
                                }
                                continue;
                            }
                            
                            if (i == 5)
                            {
                                foreach (var voisin in node.Voisins)
                                {
                                    var lowerNode=FindLowerNode(CollègeLio.Floors[i-1].Nodes,voisin);
                                    if (lowerNode != null && lowerNode.Aile==Aile.L&&EstUn(couloir, lowerNode))
                                        currentCouloir.Add(lowerNode.Position);
                                }
                            }

                            
                        }
                        currentCouloir.Add(node.Position);
                        
                    }
                   
                }

                if(currentCouloir.Count<2)continue;

                if (couloir == "CouloirC2"||couloir.Contains("CouloirM"))
                {
                    if (couloir.Contains("CouloirM")) continue;
                    
                    currentCouloir = currentCouloir.OrderBy(p => p.z).ToList();
                
                    //func
                    var r=CreateurCouloir.CréerCouloir(currentCouloir[0],
                        currentCouloir[currentCouloir.Count - 1], couloirPrefab);
                    r.transform.SetParent(couloirsÉtage.transform);
                    r.name = couloir;
                    
                    CreateurCouloir.DétruireIntersections(r,testCollider);

                    continue;
                
                }
                
                //func
                var pts=CreateurCouloir.RégressionLinéaire(currentCouloir);
                var b=CreateurCouloir.CréerCouloir(pts[0], pts[1], couloirPrefab);
                b.transform.SetParent(couloirsÉtage.transform);
                b.name = couloir;
                //CreateurCouloir.GénérerObjetsMur(pts[0],pts[1],portes,-1,couloirPrefab,i);
                //reateurCouloir.GénérerObjetsMur(pts[0],pts[1],portes,1,couloirPrefab,i);
                
                CreateurCouloir.DétruireIntersections(b,testCollider);

        
            }

            var couloirsInd = new GameObject("CouloirsInd");
            couloirsInd.transform.SetParent(couloirsÉtage.transform);  
            
            for (int j = 0; j < currentFloor.Nodes.Count; j++)
            {
                var currentNode = currentFloor.Nodes[j];
                if(currentNode.EstEndroitPublic)continue;

                string v=string.Empty;
                int c = 0;
                foreach (var voisin in currentNode.Voisins)
                {
                    var couloirsVoisin = voisin.Noms.FindAll(FindCouloirs);
                    var couloirsCurrentNode = currentNode.Noms.FindAll(FindCouloirs);
                
                    //vérifie si les deux nodes sont dans le même couloir
                    if (!couloirsCurrentNode.Intersect(couloirsVoisin).Any()&&!voisin.EstEndroitPublic
                                                                            &&SontAuMêmeNiveau(voisin,currentNode)&&!voisin.Noms.Contains("Sortie"))
                    {
                        //func
                        var k=CreateurCouloir.CréerCouloir
                            (currentNode.Position, voisin.Position, couloirPrefab);
                        k.transform.SetParent(couloirsInd.transform);
                        k.name = "CouloirIndAile"+Enum.GetName(typeof(Aile),(int)currentNode.Aile)+v;
                        CreateurCouloir.DétruireIntersections(k,testCollider);
                        
                        v=c++.ToString();
                    }
                }
            }

        }
        
        CreateurCouloir.DétruireObstacles(CollègeLio);
        EnleverIncongruités();
    }

   
    public void EnleverIncongruités()
    {
        for (int i = 0; i <(int) Étage.NombreÉtages; i++)
        {
            
            EnleverDerniersBlocs(16,i,"CouloirC1");
            EnleverDerniersBlocs(13,i,"CouloirK");
            EnleverDerniersBlocs(15,i,"CouloirS");
            EnleverDerniersBlocs(14,i,"CouloirCM");
            if (i != 0) 
            EnleverDerniersBlocs(27,i,"CouloirF"); 
            EnleverCouloir("CouloirsInd/CouloirIndAileS",i);
            EnleverCouloir("CouloirsInd/CouloirIndAileM",i);
            EnleverCouloir("CouloirsInd/CouloirIndAileE",i);
            EnleverCouloir("CouloirC2",i);


        }
        EnleverDerniersBlocs(37,0,"CouloirF");
        EnleverDerniersBlocs(10, 0, "CouloirS", 127);
        DestroyAll("CouloirIndAileN"); 
        EnleverCouloir("CouloirsInd/CouloirIndAileL",4);

        var coulD=FindAll("CouloirIndAileD");
        coulD.Reverse();
        
         //when you optimize, use RemoveRange
            for (int i = 0; i < coulD.Count - 3; i+=7)
            {
                if (i == coulD.Count - 4)
                {
                    EnleverDerniersBlocs(11,coulD[i]);
                    EnleverPremiersBlocs(11,coulD[i+1]); 
                    EnleverDerniersBlocs(8,coulD[i+2]);
                    EnleverPremiersBlocs(8,coulD[i+3]);
                    break;
               }
                
                Destroy(coulD[i]);
                EnleverDerniersBlocs(11,coulD[i+1]);
                EnleverPremiersBlocs(11,coulD[i+2]); 
                EnleverPremiersBlocs(13,coulD[i+3]);
                EnleverDerniersBlocs(13,coulD[i+4]);
                EnleverDerniersBlocs(8,coulD[i+5]);
                EnleverPremiersBlocs(8,coulD[i+6]);
                
            }
        
            EnleverCouloir("CouloirsInd/CouloirIndAileL",4);
            EnleverDerniersBlocs(17,4,"CouloirD",153);
    }
    
    //à optimiser you dont need all three funcs
    public void EnleverDerniersBlocs(int nbÀEnlever,int étage, string couloir)
    {
        var currentFloor ="Étage" + étage;
        var coul = GameObject.Find(currentFloor + "/" + couloir);
        
        if (coul==null)return;
        
        for (int i = coul.transform.childCount - 1, réduction = 0; réduction < nbÀEnlever; i--)
        {
            var bloc=coul.transform.GetChild(i).gameObject;
            Destroy(bloc);
            réduction++;
        }
    }

    public void EnleverDerniersBlocs(int nbÀEnlever, int étage, string couloir,int startIndex)
    {
        var currentFloor ="Étage" + étage;
        var coul = GameObject.Find(currentFloor + "/" + couloir);
        
        if (coul==null)return;
        
        for (int i = startIndex , add = 0; add <= nbÀEnlever; i++)
        {
            var bloc=coul.transform.GetChild(i).gameObject;
            Destroy(bloc);
            add++;
        }
    }


    public void EnleverDerniersBlocs(int nbÀEnlever, GameObject couloir)
    {
        for (int i = couloir.transform.childCount - 1, réduction = 0; réduction < nbÀEnlever; i--)
        {
            var bloc=couloir.transform.GetChild(i).gameObject;
            Destroy(bloc);
            réduction++;
        }
        
    }
    
    public void EnleverPremiersBlocs(int nbÀEnlever, GameObject couloir)
    {
        for (int i = 0; i < nbÀEnlever; i++)
        {
            var bloc = couloir.transform.GetChild(i).gameObject;
            Destroy(bloc);
        }
    }

    public static List<GameObject> FindAll(string nom)
    {
        GameObject[] objs = FindObjectsOfType<GameObject>();
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < objs.Length; i++)
        {
            if(objs[i].name.Contains(nom))
            {
                objects.Add(objs[i]);
            }
           
        }

        return objects;
    }
    

    public static void DestroyAll(string nom)
    {
        var objs=FindAll(nom);

        for (int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
        }
        
    }
    
    public void EnleverCouloir(string couloir, int étage)
    {
        var currentFloor ="Étage" + étage;
        var coul = GameObject.Find(currentFloor + "/" + couloir);
        if (coul==null)return;
        Destroy(coul);
        
    }

    // public void EnleverPremiersBlocs(int nbÀEnlever,string étage, string couloir)
    // {
    //     var currentFloor ="Étage" + étage;
    //     var coul = GameObject.Find(currentFloor + "/" + couloir);
    //     if (coul==null)return;
    //     
    //     for (int i = 0; i < nbÀEnlever; i++)
    //     {
    //         var bloc = coul.transform.GetChild(i);
    //         Destroy(bloc);
    //     }
    //     
    // }

    
    //maybe have "Find" methods where you can obtain nodes by their names or their numbers
    public void GénérerSallesPubliques()
    {
        //carrefour étudiant
        //CréateurSalle.CréerSalleRectangulaire(250,250,100,CollègeLio.Floors[0].Nodes[65].Position,salle);
        CréateurSalle.GénérerTablesEtChaises(CollègeLio.Floors[0].Nodes[65].Position,250,0,4,tablesRondesEtChaises);
        
        //cafétéria
        var caf1 = CollègeLio.Floors[0].Nodes[18].Position;
        var caf2 = CollègeLio.Floors[0].Nodes[16].Position;
        
        //CréateurSalle.CréerSalleRectangulaire(100,225,100,(caf1-caf2)/2+caf2,salle);
        CréateurSalle.GénérerTablesEtChaises((caf1-caf2)/2+caf2,150,200,8,tablesEtChaises);
        
    }
    

    public PathfindingNode FindLowerNode(List<PathfindingNode>nodes,Node nodeToFindLowerOf)
    {
        foreach (var node in nodes)
        {
            if(node.Nombre==nodeToFindLowerOf.Nombre)return node;
        }

        return null;
    }

    public bool SontAuMêmeNiveau(PathfindingNode node1, PathfindingNode node2)
    {
        return Mathf.Abs(node1.Position.y - node2.Position.y) < 5;
    }

    //add all this to CréateurCouloir
    public bool FindCouloirs(string nom)
    {
        return nom.Contains("Couloir");
    }

    //both of these in the PathfindinNode class
    public bool EstUn(string ÀÊtre,PathfindingNode node)
    {
        if (ContientUn(ÀÊtre,node))
        {
            foreach (var voisin in node.Voisins)
            {
                if (ContientUn( ÀÊtre,voisin)) return true;
            }
        }
        
        return false;
    }

    public bool EstUneSalle(PathfindingNode node)
    {
        return (ContientUn("Salle", node) || ContientUn("Ca", node));
    }

    public bool ContientUn(string àContenir,PathfindingNode node)
    {
        foreach (var nom in node.Noms)
        {
            if (nom.Contains(àContenir))
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
