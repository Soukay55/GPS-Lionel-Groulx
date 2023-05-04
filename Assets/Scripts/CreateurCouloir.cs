using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateurCouloir : MonoBehaviour
{
    private GameObject[]noeuds;
    [SerializeField] 
    public GameObject prefabCouloir,prefabObjet,parent;
    private int nbEnfants;

    private static GameObject couloir;


    private static float largeurCouloir,
    hauteurCouloir,
    longueurBloc;

    private static float distance;
    private static Vector3 position;
    private static int nombreBlocs;
    private static Quaternion rotation;
    private static Vector3 direction;
    
    
    //Calcule la direction, rotation, distance, et la position du premier objet 
    //pour les objets qui vont de pointA à pointB
    
    //tuple not necessary rn
    public static Tuple<float,Vector3,int,Quaternion, Vector3, GameObject> CalculerDirRotDistPosChemin(Vector3 pointA, Vector3 pointB, GameObject prefab)
    {
        Transform blocCouloir = prefab.transform.GetChild(0).transform;
        largeurCouloir = blocCouloir.localScale.x;
        hauteurCouloir = blocCouloir.localScale.y; 
        longueurBloc = blocCouloir.localScale.z;
        
        couloir = new GameObject("Couloir");
        
        //direction du vecteur unitaire que le couloir suivra
        direction = (pointB - pointA).normalized;
        
        //rotation des blocs de couloir
        rotation = Quaternion.LookRotation(direction);
        
        //longueur du couloir
        distance = Vector3.Distance(pointA, pointB);
        
        //self explanatory...
        nombreBlocs = Mathf.CeilToInt(distance / longueurBloc);
        
        //position du premier blocCouloir
        position = pointA;
        
        couloir.transform.position = pointA;
        couloir.transform.rotation = rotation;
        
        return new Tuple<float, Vector3, int, Quaternion, Vector3,GameObject>
            (distance, position, nombreBlocs, rotation, direction,couloir);
    }

    
    //prend deux points A et B et génère un couloir allant de A à B
    public static GameObject CréerCouloir(Vector3 pointA, Vector3 pointB, GameObject prefab)
    {
        CalculerDirRotDistPosChemin(pointA,pointB, prefab);
       
        for (var i = 0; i < nombreBlocs; i++)
        {
            Instantiate(prefab, position, rotation,couloir.transform);
              // hauteurCouloir, longueurBloc );
            
            position += longueurBloc * direction;
        }
        ScaleCouloir(couloir.transform);
        
        return couloir;
    }

    public static void DétruireMursIntérieur(GameObject poly, List<List<int>> mursDétruits)
    {
        Transform aile = poly.transform;
        List<Transform> couloirsÀDétruire=new List<Transform>();
        
        for (int j=0;j<aile.childCount;j++)
        {
            Transform couloir = aile.GetChild(j);
            for (int i=0;i<couloir.childCount;i++)
            {
                position = couloir.GetChild(i).position;
               
                Collider[] objs=Physics.OverlapSphere(position, 0.00001f);
                
                if (objs.Length > 1)
                {
                    for (int k = 0; k < objs.Length; k++)
                    {
                        var r = objs[k].gameObject.transform.parent.gameObject;
                        var ensembleMurs = r.transform.parent.transform;
                        
                        var indexEnsembleMurs = ensembleMurs.GetSiblingIndex();
                        var indexAile = ensembleMurs.transform.parent.transform.GetSiblingIndex();

                        mursDétruits[indexAile][indexEnsembleMurs]++;
                        Destroy((r));
                    }
                    
                    Array.Clear(objs,0,objs.Length);
                }

            }
            
            if(mursDétruits[aile.GetSiblingIndex()][j]==couloir.childCount)
                couloirsÀDétruire.Add(couloir);

        }

        foreach(var couloir in couloirsÀDétruire)
        {
            // couloir.parent = null;
            // Destroy(couloir.gameObject);
            couloir.gameObject.name = "miso soup";
        }

    }


    //Génère une série d'objets équidistants le long des murs d'un couloir
    //si le "codeCouloir"=-1, les objets sont sur le mur de gauche
    //si le "codeCouloir"=1, les objets sont sur le mur de droite
    //si le "codeCouloir"=0, les objets sont alternés sur les deux murs
    public static void GénérerObjetsMur(Vector3 pointA, Vector3 pointB, GameObject prefab,
        int nbObjets, int codeCouloir, GameObject prefabCouloir)
    {
        
        CalculerDirRotDistPosChemin(pointA,pointB, prefabCouloir);

        int j = -1+(int)MathF.Abs(codeCouloir);

        //si le codeCouloir était égal à zéro, il devient un, pour que le premier
        //objet "alterné" sur le mur soit instancié sur le mur de gauche, et non au 
        //milieu du couloir
        
        //maybe add parentCouloir this way the wall objects move at the same time the couloir moves
        if (j!=0)
        {
            codeCouloir = 1;
        }
        position += distance / (2 * nbObjets) * direction;
        
        var obj = Instantiate(prefab, position, rotation,couloir.transform);
        obj.transform.Translate(codeCouloir * largeurCouloir / 2, 0, 0);

        position = obj.transform.position;
        
        for (int i = 1; i < nbObjets; i++)
        {
            position += direction * distance / (nbObjets);
            position.x += j * largeurCouloir;
            
            Instantiate(prefab, position, rotation,obj.transform);
            j *= -1;
        }
    }

    public static void ScaleCouloir(Transform couloir)
    {
        var c = GetCouloirLength(couloir);
        if (c - distance > 0.001f)
        {
            var blocCouloir = couloir.GetChild(0).GetChild(0);
            var constMultipl = 1;
            for (int i = 0; i < 2; i++)
            {
                blocCouloir.position += direction*constMultipl*((c - distance) / 2)/2;
                blocCouloir.localScale -= new Vector3(0,
                    0, ((c - distance) / 2));
                blocCouloir = couloir.GetChild(couloir.childCount-1).GetChild(0);
                constMultipl *= -1;
            }
        }
    }
    
    //maybe not the best script to put this method but we"ll see
    
    public static Vector3[]RégressionLinéaire(List<Vector3>points)
    {
        Vector3[] ligneÀInterpoler = new Vector3[2];

        float sommeXY = 0;
        float sommeX = 0;
        float sommeY = 0;
        float sommeX2 = 0;
        var n = points.Count;
        
        foreach (var point in points)
        {
            //y=z ici...
            
            sommeXY += point.x * point.z;
            sommeX += point.z;
            sommeY += point.z;
            sommeX2 += Mathf.Pow(point.x, 2);
        }

        var a = (n * sommeXY - sommeX * sommeY) / (n * sommeX2 - Mathf.Pow(sommeX, 2));
        var b = (sommeY - a * sommeX) / n;

        var x1 = points[0].x;
        var y1 = points[0].y;
        var z1 = a * x1 + b;
            
        var x2 = points[n - 1].x;
        var y2 = points[n - 1].y;
        var z2 = a * x2 + b;

        ligneÀInterpoler[0] = new Vector3(x1, y1, z1);
        ligneÀInterpoler[1] = new Vector3(x2, y2, z2);
            
        return ligneÀInterpoler;
    }

    public static float GetCouloirLength(Transform couloir)
    {
        //on veut la somme de longueur tous les blocs de couloirs d'un couloir
        float length = 0;
        for (int i = 0; i < couloir.childCount; i++)
        {
            //cleanup
            length += couloir.GetChild(i).GetChild(0).localScale.z;
        }
        
        return length;
    }

    public static GameObject Dupliquer(GameObject objet)
    {
        return Instantiate(objet, objet.transform.position, objet.transform.rotation);
    }
}