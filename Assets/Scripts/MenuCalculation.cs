using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCalculation : MonoBehaviour
{
    public TMP_Text texteAffiché;
    public  Button voirTrajet;
    public Button retour;
    private PathfindingNode node;

    public  void GenererLoadingScreen()
    {
        GénererMessage();
    }

    //?????????????????????
    public void GénererMessage()
    {
        if (Contraintes.pathfinder.Statut == Pathfinder.StatutPathfinder.SUCCES)
        {
            texteAffiché.text = "Votre trajet optimal a été généré";
            voirTrajet.gameObject.SetActive(true);
        }
        if (Contraintes.pathfinder.Statut == Pathfinder.StatutPathfinder.ÉCHEC)
        {
            texteAffiché.text =
                "Il est impossible de générer un chemin qui respecte toutes les contraintes que vous avez imposées";
            voirTrajet.gameObject.SetActive(false);
            retour.gameObject.SetActive(true);
        }
    }

    public void Start()
    {
        voirTrajet.gameObject.SetActive(false);
        retour.gameObject.SetActive(false);
        GenererLoadingScreen();
        voirTrajet.onClick.AddListener(ComportementBouttonTrajet);
        retour.onClick.AddListener(ComportementBouttonRetour);

    }

   

    //à la place, fonction statique
    private static void ComportementBouttonRetour()
    {
        SceneManager.LoadScene(2);
    }
    
    private static void ComportementBouttonTrajet()
    {
        SceneManager.LoadScene(8);
    }
    
}

