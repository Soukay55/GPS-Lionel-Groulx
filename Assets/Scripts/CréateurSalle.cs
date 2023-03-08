using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CréateurSalle : MonoBehaviour //pr le carrefour étudiant, cafet mod, et carrefour
{
    [Range(0.0f,10.0f)] public float longueurSalle,
        largeurSalle;

    [SerializeField] public GameObject coinSalle; //Le prefab coinSalle part en bas
                                                  //à gauche so le prefab doit ressembler à :
                                                  //   |
                                                  //   |_____
     public GameObject cotéSalle; //à gauche
    public GameObject solSalle;

    // Start is called before the first frame update
    void Start()
    {
        CréerSalle();
    }

    void CréerSalle()//peutetre le partir du milieu a la place du top or calculer coin en bas a gauche(coingInfGauche)
                     //à partir de node gnr si node c (3,0,1), coinbasgauche c (3-longueur salle/2,0,1-largeursalle/2)
    {
        
        Vector3 coinInfGauche= transform.position;
        Vector3 coinInfDroite = transform.position + new Vector3(longueurSalle, 0f, 0f);
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
        
        GameObject cotéBas=Instantiate(cotéSalle, cotéHorzSup, Quaternion.Euler(0f, 90f, 0f));
        
        GameObject cotéLeft=Instantiate(cotéSalle, cotéGauche, Quaternion.Euler(0f, 0f, 0f));
        
        GameObject cotéRight=Instantiate(cotéSalle, cotéDroite, Quaternion.Euler(0f, 180f, 0f));

        
        Vector3 floorPos = transform.position + new Vector3(longueurSalle / 2f, -0.5f, largeurSalle / 2f);
        Instantiate(solSalle, floorPos, Quaternion.Euler(0, 0f, 0f));
    }
}