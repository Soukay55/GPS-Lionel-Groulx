using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//get the right assets automatically with no dragging
public class CréateurSalle : MonoBehaviour //pr le carrefour étudiant, cafet mod, et carrefour
{
    public static void CréerSalleRectangulaire(float width, float length, float heigth ,Vector3 centre, GameObject prefabSalle)
    {
        var salle=Instantiate(prefabSalle, centre, Quaternion.identity);
        salle.transform.localScale = new Vector3(width, heigth, length);

    }

    public static void CréerSallePolygonale()
    {
        
    }
    public static void GénérerTablesEtChaises( Vector3 pointDépart, int dimensionX, int dimensionY,
        float nbTables,GameObject prefab)
    {
        Vector3 position;
        Quaternion rotation;
        float variation = prefab.transform.localScale.x;
        
        //float nbTables = 8;
        if (dimensionY==0)
        {
            float angle;
            var rayonMax = dimensionX / 2f;
            bool tableDevant=false;
            var tableCentre = Instantiate(prefab, pointDépart, Quaternion.identity);
    
            for (float j = 1; j < rayonMax + 1; j+=25)
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

        var largeurTable = prefab.transform.localScale.x;
        var longueurTable = prefab.transform.localScale.z;
        var nombreRangées = dimensionY / largeurTable;
        
        Vector3 posDébut = pointDépart-new Vector3(dimensionX/4,0,dimensionY/2);  
        Vector3 currentPosition = posDébut;

        GameObject tables = new GameObject("Tables");

        for (int i = 0; i < nombreRangées; i++) 
        {
            for (int j = 0; j< nbTables;j++)
            {
                Instantiate(prefab, currentPosition, Quaternion.identity,tables.transform);
                currentPosition += new Vector3(largeurTable, 0.0f, 0.0f);
            }
            
            currentPosition = new Vector3(posDébut.x, 0, currentPosition.z);
            currentPosition += new Vector3(0.0f, 0.0f, longueurTable);
        }
        
    }
}