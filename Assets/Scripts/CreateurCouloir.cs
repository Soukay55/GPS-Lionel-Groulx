using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateurCouloir : MonoBehaviour
{
    private GameObject[] noeuds;
    private int nbEnfants;
    private void Start()
    {  
        nbEnfants = this.gameObject.transform.childCount;
        noeuds = new GameObject[nbEnfants];
        for (int i = 0; i < nbEnfants-1; i++)
        {
            CréerCouloir(transform.GetChild(i).localPosition, transform.GetChild(i+1).localPosition);
        }
        //CréerCouloir(transform.GetChild(0).localPosition, transform.GetChild(1).localPosition);
        
       
    }
    
    [SerializeField]
    public GameObject prefabCouloir;

    private float largeurCouloir = 0.1f,
    hauteurCouloir=1f,
    longueurBloc = 1f;

    //prend deux points A et B et génère un path fait d'un prefab allant de A à B
    public void CréerCouloir(Vector3 pointA, Vector3 pointB)
    {
        Vector3 direction = (pointB - pointA).normalized;
        float distance = Vector3.Distance(pointA, pointB);
        int nombreBlocs = Mathf.RoundToInt(distance / (longueurBloc));
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 position = pointA + (longueurBloc / 2 ) * direction;
        print(distance);

        for (int i = 0; i < nombreBlocs; i++)
        {
            GameObject blocCouloir = Instantiate(prefabCouloir, position, rotation);
            blocCouloir.transform.localScale = new Vector3(largeurCouloir, hauteurCouloir, longueurBloc*0.1f);
            position += longueurBloc *direction;
        }
    }




}
