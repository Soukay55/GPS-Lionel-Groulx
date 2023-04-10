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

    [Range(0.0f, 30.0f)] public float longueur,
        largeur;
    
    [SerializeField] public GameObject coinSalle; 
    
    public GameObject cotéSalle;
    public GameObject solSalle;
    public GameObject tablesEtChaises;
    
    // Start is called before the first frame update
    private void Start()
    {
        CréerSalle(transform.position,largeur,longueur,solSalle,coinSalle,cotéSalle);
    }
    
    //Crée une salle de longueur "longueurSalle" et de largeur "largeurSalle" à partir du centre
    
    public static void
        CréerSalle(Vector3 positionCentre, float largeurSalle, float longueurSalle,
            GameObject prefabSol, GameObject prefabCoin, GameObject prefabCôté )
    {
        
        
        //le sol de la salle
        var solMilieu = positionCentre;
        var sol = Instantiate(prefabSol, positionCentre, Quaternion.identity);
        sol.transform.localScale = new Vector3(largeurSalle - 1.9f, 0, longueurSalle - 1.9f);
                                                        //1.9 au lieu de 2 pour éviter les petits "gaps"
        
        //les côtés de la salle
        
        //haut
        var cotéSupHorz = solMilieu + new Vector3(0, 0, longueurSalle / 2 - 0.5f);
        var cotéHaut = Instantiate(prefabCôté, cotéSupHorz, Quaternion.Euler(0, 90, 0));
        cotéHaut.transform.localScale=new Vector3(1, 1, largeurSalle - 1.9f);
        
        //bas
        var cotéInfHorz = solMilieu + new Vector3(0, 0, -(longueurSalle / 2 - 0.5f));
        var cotéBas = Instantiate(prefabCôté, cotéInfHorz, Quaternion.Euler(0, -90, 0));
        cotéBas.transform.localScale=new Vector3(1, 1, largeurSalle - 1.9f);
        
        //gauche
        var cotéGauche = solMilieu + new Vector3(-(largeurSalle / 2 - 0.5f), 0, 0);
        var cotéLeft = Instantiate(prefabCôté, cotéGauche, Quaternion.identity);
        cotéLeft.transform.localScale=new Vector3(1, 1, longueurSalle - 1.9f);
        
        //droite
        var cotéDroit = solMilieu + new Vector3((largeurSalle / 2 - 0.5f), 0, 0);
        var cotéRight = Instantiate(prefabCôté, cotéDroit, Quaternion.Euler(0,180,0));
        cotéRight.transform.localScale=new Vector3(1, 1, longueurSalle - 1.9f);
        
        //les coins de la salle --what if instead of corners we just made the sides overlap
        
        //haut-gauche
        var coinSupGauche=cotéSupHorz+new Vector3(-(largeurSalle / 2 - 0.5f), 0, 0);
        var coinHautGauche = Instantiate(prefabCoin, coinSupGauche, Quaternion.Euler(0, 90, 0));
        coinHautGauche.transform.localScale = new Vector3(1.1f, 1, 1.1f);
        
        //haut-droite
        var coinSupDroite=cotéSupHorz+new Vector3(largeurSalle / 2 - 0.5f, 0, 0);
        var coinHautDroite = Instantiate(prefabCoin, coinSupDroite, Quaternion.Euler(0, 180, 0));
        coinHautDroite.transform.localScale = new Vector3(1.1f, 1, 1.1f);
        
        //bas-gauche
        var coinInfGauche=cotéInfHorz+new Vector3(-(largeurSalle / 2 - 0.5f), 0, 0);
        var coinBasGauche = Instantiate(prefabCoin, coinInfGauche, Quaternion.Euler(0, 0, 0));
        coinBasGauche.transform.localScale = new Vector3(1.1f, 1, 1.1f);
        
        //bas-droite
        var coinInfDroite=cotéInfHorz+new Vector3((largeurSalle / 2 - 0.5f), 0, 0);
        var coinBasDroite = Instantiate(prefabCoin, coinInfDroite, Quaternion.Euler(0, -90, 0));
        coinBasDroite.transform.localScale = new Vector3(1.1f, 1, 1.1f);



    }
    
    //pour le carrefour étudiant, faut les chaises se forment autour d'un point
    // central et font gnr un pattern de crecles autours
    
    
    //public void degrés
    
    public static void GénérerTablesEtChaises(bool IsRandom, Vector3 pointDépart, int dimensionX, int dimensionY,
        float nbTables,GameObject prefab)
    {
        Vector3 position;
        Quaternion rotation;
        //float nbTables = 8;
        if (IsRandom)
        {
            float angle;
            var rayonMax = dimensionX / 2f;
    
            var tableCentre = Instantiate(prefab, pointDépart, Quaternion.identity);
    
            for (float j = 1; j < rayonMax + 1; j += 1)
            {
                for (float i = 1; i < nbTables + 1; i++)
                {
                    angle = i / nbTables * (2 * Mathf.PI);
                    rotation = Quaternion.Euler(0, angle, 0);
                    position = pointDépart+ new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * j;
                    var r = Instantiate(prefab, position, rotation);
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