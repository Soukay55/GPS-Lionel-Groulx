using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CréateurSalle : MonoBehaviour //pr le carrefour étudiant, cafet mod, et carrefour
{
        //things to fix:
        //-doit get dun point début"debutcouloir" en intrant,in addition to width et lenght 
        //-->(needs specific prefab, because wall needs to have doorhole)
        //-
        //-
        //-
        //-
        
    [Range(0.0f,10.0f)] public float longueurSalle,
        largeurSalle;

    [SerializeField] public GameObject coinSalle; //Le prefab coinSalle part en bas
                                                  //à gauche so le prefab doit ressembler à :
                                                  //   |
    private Random random;
    public GameObject cotéSalle; //à gauche
    public GameObject solSalle;
    public GameObject tablesEtChaises;

    // Start is called before the first frame update
    void Start()
    {
        CréerSalle();
       // GénérerTablesEtChaises(false,Vector3.zero, 10,9,8);
    }
     void CréerSalle()//peutetre le partir du milieu a la place du top or calculer coin en bas a gauche(coingInfGauche)
                      //à partir de node gnr si node c (3,0,1), coinbasgauche c (3-longueur salle/2,0,1-largeursalle/2)
     {
         
         Vector3 coinInfGauche= transform.position;
         Vector3 coinInfDroite =  transform.position + new Vector3(longueurSalle, 0f, 0f);
         Vector3 coinSupGauche = transform.position + new Vector3(0f, 0f, largeurSalle);
         Vector3 coinSupDroite = transform.position + new Vector3(longueurSalle, 0f, largeurSalle);

         
         Instantiate(coinSalle, coinInfGauche, Quaternion.identity);
         Instantiate(coinSalle, coinInfDroite, Quaternion.Euler(0f, -90f, 0f));
         Instantiate(coinSalle, coinSupGauche, Quaternion.Euler(0f, 90f, 0f));
         Instantiate(coinSalle, coinSupDroite, Quaternion.Euler(0f, 180f, 0f));

         
         Vector3 cotéHorzInf = transform.position + new Vector3(longueurSalle / 2f, 0f, 0f);
         Vector3 cotéHorzSup = transform.position + new Vector3(longueurSalle / 2f, 0f, largeurSalle);
         Vector3 cotéGauche = transform.position + new Vector3(0f, 0f, largeurSalle / 2f);
         Vector3 cotéDroite = transform.position + new Vector3(longueurSalle, 0f, largeurSalle / 2f);

         
         GameObject cotéHaut=Instantiate(cotéSalle, cotéHorzInf, Quaternion.Euler(0f, -90f, 0f));
         cotéHaut.transform.localScale = new Vector3(largeurSalle - 2f * coinSalle.transform.localScale.x,1,1);
             
         GameObject cotéBas=Instantiate(cotéSalle, cotéHorzSup, Quaternion.Euler(0f, 90f, 0f));
         cotéBas.transform.localScale = new Vector3(largeurSalle - 2f,1,1);
         
         GameObject cotéLeft=Instantiate(cotéSalle, cotéGauche, Quaternion.Euler(0f, 0f, 0f));
         cotéLeft.transform.localScale = new Vector3(1,1,longueurSalle - 2f * coinSalle.transform.localScale.z);
         
         GameObject cotéRight=Instantiate(cotéSalle, cotéDroite, Quaternion.Euler(0f, 180f, 0f));
         cotéRight.transform.localScale = new Vector3(1,1,longueurSalle - 2f * coinSalle.transform.localScale.z);
         
         Vector3 floorPos = transform.position + new Vector3(longueurSalle / 2f, -0.5f, largeurSalle / 2f);
         Instantiate(solSalle, floorPos, Quaternion.Euler(0, 0f, 0f));
     }
     
     //pour le carrefour étudiant, faut les chaises se forment autour d'un point
     // central et font gnr un pattern de crecles autours
     
     
     //public void degrés

     public void GénérerTablesEtChaises(bool IsRandom, Vector3 pointDépart, int dimensionX, int dimensionY, float nbTables)
     {
         Vector3 position;
         Quaternion rotation;
         //float nbTables = 8;
         if (IsRandom)
         {
             float angle;
             var rayonMax = dimensionX / 2f;

             GameObject tableCentre = Instantiate(tablesEtChaises, transform.position, Quaternion.identity);

             for (float j = 2; j < rayonMax + 1; j += 2)
             {

                 for (float i = 1; i < nbTables + 1; i++)
                 {
                     angle = i / nbTables * (2 * Mathf.PI);
                     rotation = Quaternion.Euler(0, angle, 0);
                     position = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * j;
                     GameObject r = Instantiate(tablesEtChaises, position, rotation);


                 }

                 nbTables = nbTables * 1.66f;

             }



             return;
         }
         //comme dans la cafet, on veut des chaises et des tables en longueur
         //selon rangées et colonnes et faire des "blocs" de tables et chaises organisées
         rotation=Quaternion.identity;
         for (int i = 0; i < dimensionX; i++)
         {
             for (int j = 0; j < dimensionY; j++)
             {
                 position = new Vector3(j, 0, i);
                 Instantiate(tablesEtChaises, position, rotation);
             }

         }


         return;
     }
}