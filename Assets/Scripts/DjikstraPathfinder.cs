using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


//plusieurs nodes à passer
//plusieurs à passer ET une à éviter
//plusieurs à passer ETplusieurs à éviter
//au moins une aile/étage/toilette à passer
//au moins une aile/étage/toilette à passer ET une/plusieurs à éviter
//au moins une aile/étage/toilette à passer
//                      ET une/plusieurs à éviter ET une/plusieurs à passer
public class DjikstraPathfinder : Pathfinder
{
    
    //les nodes pour lesquelles l'utilisateur a imposé comme
    //contrainte de vouloir passer
    public List<PathfindingNode> NodesInévitables { get; set;}
    
    //liste de méthodes bool servant à vérifier si
    //le path trouvé par le pathfinder respecte toutes
    //les conditions de type "doit passer par AU MOINS UN.." de l'utilisateur
    public List<Func<Boolean>>Contraintes { get; set; }
    
    
    //plusieurs nodes à passer
    public DjikstraPathfinder(PathfindingNode départ, PathfindingNode end,
        List<PathfindingNode> nodes)
        :base(départ,end,nodes)
    {
        NodesInévitables = nodes;
        Path = FindPathDjikstra();//!
    }

    //plusieurs nodes à passer ET (une node à éviter OU plusieurs nodes à éviter)
    public DjikstraPathfinder(PathfindingNode départ, PathfindingNode end,
        List<PathfindingNode> nodesàPasser,List<PathfindingNode> nodesÉviter )
        :base(départ,end,nodesàPasser)
    {
        NodesInévitables = nodesàPasser;
        NodesÀÉviter = nodesÉviter;
        Path = FindPathDjikstra();//!
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
    
   
    //on veut un cycle hamiltonien, mais le node départ ne doit être atteint qu'une fois
    //en utilisant le FindPathAStar, on évite de repasser deux fois sur un node déja visité
    
    //DjikstraPathfinders
    
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
    
    //Pour tous les problèmes de Pathfinding ou il y a des nodes que l'on doit
    //obligatoirement passer 
    
    //Pathfinder pour cas #1 ou cas #2: plusieurs à passer OU (plusieurs à passer ET plusieurs/une à éviter)
    //
    //Maybe create a Path class? easier to manipulate?
    //
    //Approche naive pour le pathfinding: adaptation du "TSP"
    public List<PathfindingNode> FindPathDjikstra1()
    {
        int nombreNodes = NodesInévitables.Count;
        int[] nombresÀPermuter = new int[nombreNodes];
              for (int i = nombreNodes; i > 0; i--) nombresÀPermuter[i - 1] = i;
        int[,] permutations= new int[FactorielDe(nombreNodes), nombreNodes];
        
        permutations=PermutationsPossibles(nombreNodes, nombresÀPermuter, permutations);
        float[,]distances=CréerMatriceAdjacence();
        
        NodesInévitables.Insert(0,Départ);
        NodesInévitables.Add(Arrivée);

        //Maybe make these properties of DjikstraPathFindr instead? code getting crowded AF!!!
        List<PathfindingNode> BestPath = NodesInévitables;
        List<PathfindingNode> CurrentPath = new List<PathfindingNode>();
        float currentPathLength = 0;
        float bestPathLength = distances[0,permutations[0,0]]
                               +distances[permutations[0,nombreNodes-1],nombreNodes+1];
        
        for (int i=0;i<nombreNodes;i++)
        {
            bestPathLength += distances[permutations[0, i], permutations[0, i + 1]];
        }

        for (int i = 1; i < FactorielDe(nombreNodes) - 1; i++)
        {
            //Maybe create a Func that removes everything except Départ & Arrivée
            //so you don't have to clear and add at each iteration of the loop
            CurrentPath.Clear();
            CurrentPath.Add(Départ);
            currentPathLength = distances[0, permutations[i, 0]]
                                 + distances[permutations[i, nombreNodes + 1], nombreNodes + 1];
            
            for (int j = 0; i < nombreNodes; i++)
            {
                CurrentPath.Insert(j+1,NodesInévitables[permutations[i,j]]);
                currentPathLength += distances[permutations[i, j], permutations[i, j + i]];
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

    //à tester to be sure
    public void Insérer(List<PathfindingNode>path,int index, List<PathfindingNode>listeÀInsérer)
    {
        for (int i = 0; i < listeÀInsérer.Count; i++)
        {
            path.Insert(index+i,listeÀInsérer[i]);
        }
    }

    public List<PathfindingNode> FindPathDjikstra()
    {
        return NodesInévitables;
    }

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
