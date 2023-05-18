using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigateur : MonoBehaviour
{
    [SerializeField] public Button next;
    public Button cacher;
    public Button montrer;
    public Button quitter;
    public Camera camera2d;
    public Button startNav;
    public TMP_Text instructions;
    public Material matérielPath;
    public GameObject panel;
    public TMP_Text position;
    

    private List<PathfindingNode>Chemin { get; set; }
    
    private SplineCubique Path { get; set; }
    
    private int positionPath;
    private int positionInterpol;
    private bool moveToNext;

    private GameObject étage0;
    private GameObject étage1;
    private GameObject étage2;
    private GameObject étage3;
    private GameObject étage4;
    private GameObject étage5;

    private GameObject[] étages;


    private void Start()
    {
        positionPath = 0;
        positionInterpol = 0;
        
        startNav.gameObject.SetActive(true);
        startNav.onClick.AddListener(ComportementBouttonStartNav);
        next.gameObject.SetActive(false);
        next.onClick.AddListener(ComportementBouttonNext);
        instructions.gameObject.SetActive(false);
        montrer.gameObject.SetActive(false);
        cacher.gameObject.SetActive(true);
        quitter.gameObject.SetActive(false);
        panel.SetActive(false);
        cacher.onClick.AddListener(ComportementBouttonCacher);
        montrer.onClick.AddListener(ComportementBouttonMontrer);
        quitter.onClick.AddListener(ComportementBouttonQuitter);
        
        Chemin = Contraintes.Pathfinder.Path;
        Path = new SplineCubique(Chemin, 1500);
        GénérerInstructions(Chemin);
        
    }

    void FindÉtages()
    {
        étage0 = GameObject.Find("Étage0");
        étage1 = GameObject.Find("Étage1");
        étage2= GameObject.Find("Étage2");
        étage3 = GameObject.Find("Étage3");
        étage4 = GameObject.Find("Étage4");
        étage5 = GameObject.Find("Étage5");
    }
    

    private void Update()
    {
        //position.text="Vous êtes dans l'aile"+

        camera2d.transform.rotation = Quaternion.Euler(90,0,0);
        
        if (moveToNext)
        {
            if (positionInterpol!=Path.Interpolation.Length-1)
            {
                transform.rotation=Quaternion.LookRotation(Path.Interpolation[positionInterpol+1]-
                                                           Path.Interpolation[positionInterpol]);
                positionInterpol++;
                transform.position = Path.Interpolation[positionInterpol]+Vector3.up*8;
                print(transform.position);
            }
            
            SetCurrentÉtage(GetÉtage(transform.position));
            
            if (Path.Stops[positionPath]==positionInterpol)
            {
                moveToNext = false;
                if (positionPath==Chemin.Count-1)
                {
                    panel.gameObject.SetActive(true);
                    quitter.gameObject.SetActive(true);
                    
                }
                else
                {
                    next.gameObject.SetActive(true);
                    //stairs menu
                    if (Chemin[positionPath].Nombre == Chemin[positionPath + 1].Nombre)
                    {
                        
                    }
                }
            }

        }
        
    }

    public int GetÉtage(Vector3 position)
    {
        return (int)(position.y / 82f);
    }

    private void ComportementBouttonNext()
    {
        instructions.text = Chemin[positionPath].Instructions;
        next.gameObject.SetActive(false);
        moveToNext = true;
        positionPath++;
    }
    
    private void ComportementBouttonCacher()
    {
        instructions.gameObject.SetActive(false);
        cacher.gameObject.SetActive(false);
        montrer.gameObject.SetActive(true);
    } 
    
    private void ComportementBouttonMontrer()
    {
        instructions.gameObject.SetActive(true);
        cacher.gameObject.SetActive(true);
        montrer.gameObject.SetActive(false);
    }

    void ComportementBouttonQuitter()
    {
        SceneManager.LoadScene(0);
    }

    void ComportementBouttonStartNav()
    {
        Path.Interpoler();
        Path.RenderSpline(true,matérielPath);

        transform.position = Chemin[0].Position;
        transform.rotation=Quaternion.LookRotation(Path.Interpolation[positionInterpol+1]-
                                                   Path.Interpolation[positionInterpol]);
        
        startNav.gameObject.SetActive(false);
        next.gameObject.SetActive(true);
        instructions.gameObject.SetActive(true);
        instructions.text = Chemin[0].Instructions;
        
        FindÉtages();
        étages = GameObject.FindGameObjectsWithTag("Étage");
        
    }

    void SetCurrentÉtage(int currentÉtage)
    {
        foreach (var étage in étages)
        {
            if (étage.name.Contains(currentÉtage.ToString()))
            {
                étage.SetActive(true);
                continue;
            }
            
            étage.SetActive(false);
        }
        
    }
    
    public static void GénérerInstructions(List<PathfindingNode> trajetUtilisateur)
    {
        
        trajetUtilisateur[0].Instructions = "Étape#0-Aller vers" + trajetUtilisateur[1].Noms[0];
        
        for (int i = 1; i < trajetUtilisateur.Count-1; i++)
        {
            var niveau1 = trajetUtilisateur[i].Niveau;
            var niveau2 = trajetUtilisateur[i+1].Niveau;
            
            if (GénérerÉcole.ContientUn("Escalier",trajetUtilisateur[i])&&
            GénérerÉcole.ContientUn("Escalier",trajetUtilisateur[i+1])&&(niveau1!=niveau2))
            {
                var upOrDown = niveau1 > niveau2 ? "Descendre à l'étage inférieur" 
                    : "Monter à l'étage supérieur";
                
                trajetUtilisateur[i].Instructions ="Étape#"+i+"-" + upOrDown;
                
                continue;
            }

            var nom = trajetUtilisateur[i].Noms[0];
            var direction = string.Empty;
            trajetUtilisateur[i].Instructions = "Étape #" + (i + 1);

            var tournerÀGauche = DoitTourner(trajetUtilisateur[i - 1].Position
                , trajetUtilisateur[i].Position,
                trajetUtilisateur[i + 1].Position);
            
            if (tournerÀGauche== -1)
            {
                direction = "Continuer tout droit ";
            }
            else
            {
                direction = tournerÀGauche == 0 ? "Tourner à droite " : "Tourner à gauche";
            }

            trajetUtilisateur[i].Instructions += direction + "pour aller à" + nom;
        }
        

        trajetUtilisateur[trajetUtilisateur.Count - 1].Instructions = "Vous êtes arrivés";
        
    }

    public static int DoitTourner(Vector3 a,Vector3 b,Vector3 c)
    {
        Vector3 bc = c- b;
        Vector3 ba = a- b;

        bool doitTourner= Mathf.Acos(Vector3.Dot(ba, bc) / (bc.magnitude * ba.magnitude))<3*Mathf.PI/4;
        bool àGauche = Polygone.IsClockwise(new List<Vector3>() { a, b, c });

        if (doitTourner)
        {
            return Convert.ToInt32(doitTourner && àGauche);
        }

        return -1;
    }
}
