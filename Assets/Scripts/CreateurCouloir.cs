using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateurCouloir : MonoBehaviour
{
    private GameObject[] noeuds;
    [SerializeField] 
    public GameObject prefabCouloir,prefabPorte;
    private int nbEnfants;

    private void Start()
    {
        nbEnfants = gameObject.transform.childCount;
        noeuds = new GameObject[nbEnfants];
        for (var i = 0; i < nbEnfants - 1; i++)
            CréerCouloir(transform.GetChild(i).position,
                transform.GetChild(i + 1).position);
        //CréerCouloir(transform.GetChild(0).localPosition, transform.GetChild(1).localPosition);
    }

   
    private float largeurCouloir = 2f,
        hauteurCouloir = 3f,
        longueurBloc = 1f;

    //prend deux points A et B et génère un path fait d'un prefab allant de A à B
    public void CréerCouloir(Vector3 pointA, Vector3 pointB)
    {
        var direction = (pointB - pointA).normalized;
        var distance = Vector3.Distance(pointA, pointB);
        var nombreBlocs = Mathf.RoundToInt(distance / longueurBloc);
        var rotation = Quaternion.LookRotation(direction);
        var position = pointA + longueurBloc / 2 * direction;
        print(distance);

        for (var i = 0; i < nombreBlocs; i++)
        {
            var blocCouloir = Instantiate(prefabCouloir, position, rotation);
            blocCouloir.transform.localScale = new Vector3(largeurCouloir,
                hauteurCouloir, longueurBloc );
            position += longueurBloc * direction;
        }
        
        AjouterPortes(5,rotation,pointA,1,distance,direction);
    }

    // code coté :1=droite, -1=gauche,0=both
    //you deffo dont need this many params girllllll
    public void AjouterPortes(int nbPortes,Quaternion rotation,Vector3 centreCouloir, int codeCôté,float distance, Vector3 direction)
    {
        Vector3 position = centreCouloir;
        position.x = centreCouloir.x - largeurCouloir / 2 * codeCôté;
        position.z = centreCouloir.z += distance / nbPortes / 2;
        for (var i = 0; i < nbPortes; i++)
        {
            Instantiate(prefabPorte, position, rotation);
            position += (distance / nbPortes) * direction;

        }
    }

    // public void RemoveOverlap(GameObject objet)
    // {
    //     while (Physics.CheckSphere(objet.transform.position,largeurCouloir/2))
    //     {
    //         
    //     }
    //
    //     
    // }
}