using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        nombreBlocs = Mathf.FloorToInt(distance / longueurBloc);
        
        //position du premier blocCouloir
        position = pointA;
        
        couloir.transform.position = pointA;
        couloir.transform.rotation = rotation;
        return new Tuple<float, Vector3, int, Quaternion, Vector3,GameObject>
            (distance, position, nombreBlocs, rotation, direction,couloir);
    }

    //clean up the getParent lines ewwwwwwwwwww
    //prend deux points A et B et génère un couloir allant de A à B
    public static GameObject CréerCouloir(Vector3 pointA, Vector3 pointB, GameObject prefab)
    {
    
       CalculerDirRotDistPosChemin(pointA,pointB, prefab);
        for (var i = 0; i < nombreBlocs; i++)
        {
            var blocCouloir= Instantiate(prefab, position, rotation,couloir.transform);
            //blocCouloir.transform.localScale = new Vector3(largeurCouloir,
              // hauteurCouloir, longueurBloc );
            
            position += longueurBloc * direction;
        }

        return couloir;
    }

    public static void DétruireMursIntérieur(GameObject murs)
    {
        Transform parent = murs.transform;
        for (int j=0;j<parent.childCount;j++)
        {
            Transform child = parent.GetChild(j).transform;
            
            for (int i=0;i<child.childCount;i++)
            {
                position = child.GetChild(i).position;
                Collider[] objs=Physics.OverlapSphere(position, 0.0001f);
                if (objs.Length > 1)
                {
                    for (int k = 0; k < objs.Length; k++)
                    {
                        Destroy((objs[k].gameObject.transform.parent.gameObject));
                    }
                    Array.Clear(objs,0,objs.Length);
                }
            }   
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

    public static void GénérerCoquilleExterne(GameObject mursExt, GameObject prefab)
    {
        
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < mursExt.transform.childCount; i++)
        {
            var poly = mursExt.transform.GetChild(i).transform;
            for (int j = 0; j < poly.childCount; j++)
            {
                points.Add(poly.GetChild(j).transform.position);
            }
        }
        
        
        Polygone coqExt = new Polygone("CoquilleExterne", points);
        Vector3 centroide = coqExt.CalculerCentroide();
        List<Vector3> newPoints = new List<Vector3>();
        
        foreach (var point in coqExt.Points)
        {
            print("hi");
            newPoints.Add(centroide + (point - centroide) * 1.2f);
        }
        
        coqExt.SetPoints(newPoints);
        coqExt.DessinerPolygone(prefab,mursExt);
    }

    public static void ScaleCouloir(Transform couloir)
    {
        
        var c = GetCouloirLength(couloir);
        if (c != distance)
        {
            print(distance);
            print(c);
            couloir.transform.localScale = new Vector3(couloir.transform.localScale.x, couloir.transform.localScale.y,
                couloir.transform.localScale.z* (distance / c));
        }
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