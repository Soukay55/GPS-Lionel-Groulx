using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
//get the right assets automatically with no dragging
public class CréateurSalle : MonoBehaviour //pr le carrefour étudiant, cafet mod, et carrefour
{
    

    [Range(0.0f, 30.0f)] public float longueur,
        largeur;

    [SerializeField] public GameObject salle,
    tablesEtChaises;
    
    // Start is called before the first frame update
    private void Start()
    {
        //GénérerTablesEtChaises(transform.position,12,0,6,tablesEtChaises);
        CréerSalleRectangulaire(60,90,30,transform.position,salle);
    }
    
   

    public static void CréerSalleRectangulaire(float width, float length, float heigth ,Vector3 centre, GameObject prefabSalle)
    {
        var salle=Instantiate(prefabSalle, centre, Quaternion.identity);
        salle.transform.localScale = new Vector3(width, heigth, length);

    }

    public static void CréerSallePoly()
    {
        
    }
    public static void GénérerTablesEtChaises( Vector3 pointDépart, int dimensionX, int dimensionY,
        float nbTables,GameObject prefab)
    {
        Vector3 position;
        Quaternion rotation;
        //float nbTables = 8;
        if (dimensionY==0)
        {
            Debug.Log("bonjour");
            float angle;
            var rayonMax = dimensionX / 2f;
            bool tableDevant=false;
            var tableCentre = Instantiate(prefab, pointDépart, Quaternion.identity);
    
            for (float j = 1; j < rayonMax + 1; j += 1)
            {
                var rangée = new GameObject("Rangée");
                rangée.transform.SetParent(tableCentre.transform);
                rangée.transform.position = pointDépart;
                for (float i = 1; i < nbTables + 1; i++)
                {
                    angle = i / nbTables * (2 * Mathf.PI);
                    rotation = Quaternion.Euler(0, angle, 0);
                    position = pointDépart+ new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * j;
                     Instantiate(prefab, position, rotation,rangée.transform);
                    
                    //pour donner l'air "éparpillé"
                     if(Physics.Raycast(position, (position - pointDépart),1.0f))
                        rangée.transform.Rotate(Vector3.up,70);;

                     var r = Physics.OverlapSphere(position, 0.15f);
                     if (r.Length != 1)
                     {
                         for (int k = 0; k < r.Length - 1; k++)
                         {
                             Destroy(r[k].gameObject);
                         }
                         
                     }
                }
                
                //maintenant va rangée par rangée, et on supprime à chaque deux tables,
                //puis trois, puis deux
                int cpt = 0;
                for (int i = 0; i < rangée.transform.childCount; i+=3)
                {
                    if (cpt % 2 != 0) i--;
                    Destroy(rangée.transform.GetChild(i).gameObject);
                    cpt++;
                }
                
                
                nbTables = nbTables * 1.66f;
            }
    
    
            return;
        }
    
        //comme dans la cafet, on veut des chaises et des tables en longueur
        //selon rangées et colonnes et faire des "blocs" de tables et chaises organisées
        rotation = Quaternion.identity;
        for (var i = 0; i < dimensionX; i++)
        for (var j = 0; j < dimensionY; j++)
        {
            position = new Vector3(j, 0, i);
            Instantiate(prefab, position, rotation);
        }
    
        return;
    }
}