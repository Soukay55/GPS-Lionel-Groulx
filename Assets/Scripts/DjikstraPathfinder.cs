using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
//some of these methods need to go in Pathfinder theres too many lines

//plusieurs nodes à passer
//plusieurs à passer ET une à éviter
//plusieurs à passer ET plusieurs à éviter
//au moins une aile/étage/toilette à passer
//au moins une aile/étage/toilette à passer ET une/plusieurs à éviter
//au moins une aile/étage/toilette à passer
//ET une/plusieurs à éviter ET une/plusieurs à passer
public class DjikstraPathfinder : Pathfinder
{

    //les nodes pour lesquelles l'utilisateur a imposé comme
    //contrainte de vouloir passer
    public List<PathfindingNode> NodesInévitables { get; set; }

    //liste de méthodes bool servant à vérifier si
    //le path trouvé par le pathfinder respecte toutes
    //les conditions de type "doit passer par AU MOINS UN.." de l'utilisateur
    public List<Func<Boolean>> Contraintes { get; set; }

    public float[,] Coûts { get; set; }


    ///plusieurs nodes à passer
    public DjikstraPathfinder(PathfindingNode départ, PathfindingNode end,
        List<PathfindingNode> nodes)
        :base(départ,end,nodes)
    {
        NodesInévitables = nodes;
        Path = FindPathDjikstra1();//!
    }

    //plusieurs nodes à passer ET (une node à éviter OU plusieurs nodes à éviter)
    public DjikstraPathfinder(PathfindingNode départ, PathfindingNode end,
        List<PathfindingNode> nodesàPasser,List<PathfindingNode> nodesÉviter )
        :base(départ,end,nodesàPasser)
    {
        NodesInévitables = nodesàPasser;
        NodesÀÉviter = nodesÉviter;
        Path = FindPathDjikstra1();//!
    }
    
    
    //contrainte(s) de type "doit passer par au moins une.." à respecter
    public DjikstraPathfinder(PathfindingNode départ, PathfindingNode end,
        List<Func<Boolean>> contraintes) : base(départ, end)
    {
        Contraintes = contraintes;
        Path = FindPathDjikstra();//!
    }
    
    //contrainte(s) de type "doit passer par au moins une.." à respecter ET
    //(plusieurs nodes à passer OU plusieurs nodes à éviter)
    public DjikstraPathfinder(PathfindingNode départ, PathfindingNode end, List<PathfindingNode>nodes
        ,List<Func<Boolean>> contraintes) : base(départ, end,nodes)
    {
        if (!nodes[0].EstTraversable)
        {
            NodesÀÉviter = nodes;
        }
        else
        {
            NodesInévitables = nodes;
        }
        Contraintes = contraintes;
        Path = FindPathDjikstra();//!
    }
    
    //contrainte(s) de type "doit passer par au moins une.." à respecter ET
    //une/plusieurs node(s) à passer ET une/plusieurs node(s) à éviter
    public DjikstraPathfinder(PathfindingNode départ, PathfindingNode end, List<PathfindingNode>nodesAÉviter,
        List<PathfindingNode>nodesÀPasser
        ,List<Func<Boolean>> contraintes) : base(départ, end,nodesÀPasser)
    {
        Contraintes = contraintes;
        NodesÀÉviter = nodesAÉviter;
        NodesInévitables = nodesÀPasser;
        Path = FindPathDjikstra();//!
    }
    
    //on veut un cycle hamiltonien, mais le node départ ne doit être atteint qu'une fois
    //en utilisant le FindPathAStar, on évite de repasser deux fois sur un node déja visité
    
    //DjikstraPathfinders
    
      //Pour tous les problèmes de Pathfinding ou il y a des nodes que l'on doit
    //obligatoirement passer 
    
    //Pathfinder pour cas #1 ou cas #2: plusieurs à passer OU (plusieurs à passer ET plusieurs/une à éviter)
    //
    //Maybe create a Path class? easier to manipulate?
    //
    //Approche BRUTE pour le pathfinding: adaptation du "TSP"
    public List<PathfindingNode> FindPathDjikstra1()
    {
        int nombreNodes = NodesInévitables.Count;
        
        int[] nombresÀPermuter = new int[nombreNodes];
              for (int i = nombreNodes; i > 0; i--) nombresÀPermuter[i - 1] = i;
              
        int[,] permutations= new int[FactorielDe(nombreNodes), nombreNodes];
        
        permutations=PermutationsPossibles(nombreNodes, nombresÀPermuter, permutations);
        Coûts=CréerMatriceAdjacence();
        
        NodesInévitables.Insert(0,Départ);
        NodesInévitables.Add(Arrivée);

        //Maybe make these properties of DjikstraPathFindr instead? code getting crowded AF!!!
        List<PathfindingNode> BestPath = NodesInévitables;
        List<PathfindingNode> CurrentPath = new List<PathfindingNode>();
        
        float currentPathLength = 0;
        
        float bestPathLength = Coûts[0,permutations[0,0]]
                               +Coûts[permutations[0,nombreNodes-1],nombreNodes+1];
        
        for (int i=0;i<nombreNodes-1;i++)
        {
            bestPathLength += Coûts[permutations[0, i], permutations[0, i + 1]];
        }

        for (int i = 1; i < FactorielDe(nombreNodes) - 1; i++)
        {
            //Maybe create a Func that removes everything except Départ & Arrivée
            //so you don't have to clear and add at each iteration of the loop
            CurrentPath.Clear();
            CurrentPath.Add(Départ);
            currentPathLength = Coûts[0, permutations[i, 0]]
                                 + Coûts[permutations[i, nombreNodes + 1], nombreNodes + 1];
            
            for (int j = 0; i < nombreNodes; i++)
            {
                CurrentPath.Insert(j+1,NodesInévitables[permutations[i,j]]);
                currentPathLength += Coûts[permutations[i, j], permutations[i, j + i]];
            }
            
            CurrentPath.Add(Arrivée);

            if (currentPathLength < bestPathLength)
            {
                BestPath = CurrentPath;
                bestPathLength = currentPathLength;
            }

        }
        
        //On doit maintenant trouver le meilleur "Path" A-Star entre chacuns de ces nodes
        
        GetPathFromSubPath(BestPath);
        
        return BestPath;
    }
    
    //in the end, arrivée is going to be at the beginning of the Path, so
    //the first position needs to be modified
    //Pathfinder pour le cas #1 et #2 mais pour 6 nodes et plus
    public List<PathfindingNode> FindPathDjikstra()
    {
        NodesInévitables.Add(Arrivée);
        NodesInévitables.Insert(0,Départ);
        int nbNodes = NodesInévitables.Count;

        Coûts = CréerMatriceAdjacence();
        
        //puisqu'on utilise la Hungarian Method, qui sert normalement à régler
        //des problèmes d'assignation, il faut modifier la démarche habituelle
        //pour nos fins. En calculant un cycle hamiltonien optimal, et s'assurant que 
        //la node Arrivée soit parente de la node Départ, on peut ensuite enlever cette
        //arrête du graphe, et obtenir un chemin non-circulaire qui va de départ à arrivée
        
        
        //On minimise le cout pour aller de Départ à Arrivée
        Coûts[nbNodes - 1,0] = 0;
        
        for (int i = 1; i < nbNodes; i++)
        {
            //on ne veut pas qu'une des nodes inévitables revienne à la position
            //de départ, alors on met que des infinis dans la première colonne
            if(i!=nbNodes-1)
                Coûts[i, 0] = Mathf.Infinity;
            
            //on ne veut pas que la node Arrivée soit connectée à une node autre
            //que Départ, alors on met tous les couts pour celui-ci à infini 
            Coûts[nbNodes - 1, i] = Mathf.Infinity;
        }
        RéduireMatrice();
        
        return NodesInévitables;
    }
    
    public int Assigner()
    {
        int nbZéros=0;
        char[,] états = new char[Coûts.GetLength(0), Coûts.GetLength(1)];
        
        
        for (int i = 0; i <Coûts.GetLength(0);i++)
        {
            for (int j=0;j<Coûts.GetLength(1);j++)
            {
                //erreur de précision du float
                if (Coûts[i, j] < 0.0002f)
                {
                    //nbZéros++;
                  //  états[i,j]=""
                }
    
                if (nbZéros > 1)
                {
                    
                }
            }
        }

        return 3;
        
    }

    public bool EstCycleHamiltonien(List<PathfindingNode>Path)
    {
        return Path.IndexOf(Arrivée) < Path.Count - 1;
    }

    public void RéduireMatrice()
    {
        float minR, minC;
        
        
        //réduire les rangées et les colonnes
        for(int i=0;i<Coûts.GetLength(0);i++)
        {
            minR= GetRowMinimum(Coûts, i);
            minC = GetColumnMinimum(Coûts, i);
            
            for (int j = 0; j < Coûts.GetLength(1); j++)
            {
                Coûts[i, j] -= minR;
                
                if(j!=0)
                Coûts[j, i] -= minC;
            }
        }
        
        
    }

    public float GetRowMinimum(float[,]matrice, int rangée)
    {
        float min=Coûts[rangée,0];
        
            for (int j = 1; j < Coûts.GetLength(1); j++)
            {
                if (Coûts[rangée, j] < min) min = Coûts[rangée, j];
            }

            return min;
    }
    
    public float GetColumnMinimum(float[,]matrice, int colonne)
    {
        float min=Coûts[0,colonne];
        
        for (int j = 1; j < Coûts.GetLength(1); j++)
        {
            if (Coûts[j, colonne] < min) min = Coûts[j, colonne];
        }

        return min;
    }

    //crée la matrice d'adjacence pour le sous-graphe contenant que les nodesInévitables.
    //Les cout pour aller d'un node à un autre est calculé avec AStar.
    public float[,] CréerMatriceAdjacence()
    {
        NodesInévitables.Insert(0,Départ);
        NodesInévitables.Add(Arrivée);

        AStarPathfinder pathfinder = new AStarPathfinder();
        
        //On crée la matrice d'adjacence du sous-graphe qui contient la node départ, les
        //nodes inévitables, et la node arrivée
        float[,] matriceAdja = new float[NodesInévitables.Count,NodesInévitables.Count];
        for (int i = 0; i < NodesInévitables.Count - 1; i++)
        {
            pathfinder.Départ = NodesInévitables[i];
            for (int j = 0; j < NodesInévitables.Count - 1; j++)
            {
                pathfinder.Arrivée = NodesInévitables[j];
                if (i!=j)
                {
                    Path=pathfinder.FindPathAStar();
                    matriceAdja[i, j] = CalculatePathLength();
                }
                else
                {
                    matriceAdja[i, j] = float.PositiveInfinity;
                }
            }
        }
        
        return matriceAdja;
    }
    
    //COMMENTS!!
    public void GetPathFromSubPath(List<PathfindingNode> path)
    {
        AStarPathfinder pathfinder = new AStarPathfinder();

        for (int i = 0; i < NodesInévitables.Count-1; i++)
        {
            pathfinder.Départ = path[i];
            pathfinder.Arrivée = path[i + 1];
            
            Insérer(path, path.Count-(NodesInévitables.Count-i)+1,pathfinder.FindPathAStar());

        }

    }
    
    
    //PermutationsPossibles calcule toutes les permutations possibles par lesquelles doivent passer
    //le pathfinder et les range dans un tableau de taille n par n!, n étant le nombre
    //de nodes Inévitables
    
    //FactorielDe prend en intrant un nombre entier et retourne son factoriel
    //(On ne se sert seulement de cette fonction lorsqu'il y a plus qu'une "nodeInévitable",
    // donc in n'a pas besoin de vérifier l'exception 0!)
    public int FactorielDe(int nombre)
    {
        int factoriel = nombre;
        for (int i = nombre - 1; i > 0; i--)
        {
            factoriel *= i;
        }

        return factoriel;
    }
    
    //Intervert les élements à l'index a et b du tableau
    public void Intervertir(int a, int b, int[] tab)
    {
        int placeHolder = tab[a];
        tab[a] = tab[b];
        tab[b] = placeHolder;
    }

    //RemplirRangée remplis la prochaine rangée d'un tableau donné
    public void RemplirRangée(int[] nombresÀPermuter, int[,] permutations)
    {

        int rangée = NbÉlémentsNonNulsDans(permutations)/ nombresÀPermuter.Length;
        for (int i = 0; i < nombresÀPermuter.Length; i++)
        {
            permutations[rangée, i] = nombresÀPermuter[i];
        }

    }
    
    public int NbÉlémentsNonNulsDans(int[,] tab)
    {
        int nb = 0;
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            for (int j = 0; j < tab.GetLength(1); j++)
            {
                if (tab[i, j] == 0)
                {
                    return nb;
                }

                nb++;
            }
        }

        return nb;
    }

    //Pour réduire la mémoire utilisée, on se sert d'un tableau à la place d'une liste,
    //car on connait déja l'espace nécéssaire, et celui-ci est statique
    //maybe create an equatable to add up lines in an array                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    
    public int[,] PermutationsPossibles(int nombreNodes,int[] nombresÀPermuter,int[,] permutations)
    {
        if (nombreNodes == 1)
        {
            RemplirRangée(nombresÀPermuter, permutations);
        }
        else
        {
            for (int i = 0; i < nombreNodes; i++)
            {
                PermutationsPossibles(nombreNodes - 1, nombresÀPermuter, permutations);
                if (nombreNodes % 2 == 0)
                {
                    Intervertir(i, nombreNodes - 1, nombresÀPermuter);
                }
                else
                {
                    Intervertir(0, nombreNodes - 1, nombresÀPermuter);
                }
            }
        }

        return permutations;
    }
    

    //à tester to be sure
    public void Insérer(List<PathfindingNode>path,int index, List<PathfindingNode>listeÀInsérer)
    {
        for (int i = 0; i < listeÀInsérer.Count; i++)
        {
            path.Insert(index+i,listeÀInsérer[i]);
        }
    }

    //hmm 
    public override int GetNodePlusPetitCout(List<PathfindingNode> nodeList)
    {
        int indexPlusPetit = 0;
        float plusPetitCout = nodeList[0].HCost;
        
        for (int i = 1; i < nodeList.Count; i++)
        {
            if (plusPetitCout >nodeList[i].HCost)
            {
                plusPetitCout = nodeList[i].HCost;
                indexPlusPetit = i;
            }
        }
        //PathfindingNode n = nodeList[indexPlusPetit];
        return indexPlusPetit;
    }
   
}
