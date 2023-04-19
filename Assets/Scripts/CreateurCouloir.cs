using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateurCouloir : MonoBehaviour
{
    private GameObject[]noeuds;
    [SerializeField] 
    public GameObject prefabCouloir,prefabObjet,parent;
    private int nbEnfants;


    private static GameObject couloir;
   
   
    private static float largeurCouloir = 2f,
        hauteurCouloir  =5f,
        longueurBloc = 1f;

    private static float distance;
    private static Vector3 position;
    private static int nombreBlocs;
    private static Quaternion rotation;
    private static Vector3 direction;
    
    //Calcule la direction, rotation, distance, et la position du premier objet 
    //pour les objets qui vont de pointA à pointB
    public static Tuple<float,Vector3,int,Quaternion, Vector3, GameObject> CalculerDirRotDistPosChemin(Vector3 pointA, Vector3 pointB)
    {
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
       CalculerDirRotDistPosChemin(pointA,pointB);
        for (var i = 0; i < nombreBlocs; i++)
        {
            var blocCouloir= Instantiate(prefab, position, rotation,couloir.transform);
            blocCouloir.transform.localScale = new Vector3(largeurCouloir,
                hauteurCouloir, longueurBloc );
            position += longueurBloc * direction;
        }

        return couloir;
    }
    
    //Génère une série d'objets équidistants le long des murs d'un couloir
    //si le "codeCouloir"=-1, les objets sont sur le mur de gauche
    //si le "codeCouloir"=1, les objets sont sur le mur de droite
    //si le "codeCouloir"=0, les objets sont alternés sur les deux murs
    public static void GénérerObjetsMur(Vector3 pointA, Vector3 pointB, GameObject prefab,
        int nbObjets, int codeCouloir)
    {
        
        CalculerDirRotDistPosChemin(pointA,pointB);

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
    
}