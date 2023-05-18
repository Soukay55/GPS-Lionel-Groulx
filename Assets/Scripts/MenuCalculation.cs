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
        if (Contraintes.Pathfinder.Statut == Pathfinder.StatutPathfinder.SUCCES)
        {
            texteAffiché.text = "Votre trajet optimal a été généré";
            voirTrajet.gameObject.SetActive(true);
        }
        if (Contraintes.Pathfinder.Statut == Pathfinder.StatutPathfinder.ÉCHEC)
        {
            texteAffiché.text =
                "Il n'existe aucun chemin qui respecte" +
                " toutes les contraintes que vous avez imposées";
            
            retour.gameObject.SetActive(true);
        }
    }

    public void Start()
    {
        voirTrajet.gameObject.SetActive(false);
        retour.gameObject.SetActive(false);
        voirTrajet.onClick.AddListener(ComportementBouttonTrajet);
        retour.onClick.AddListener(ComportementBouttonRetour);
        GenererLoadingScreen();
    }

    //à la place, fonction statique
    private static void ComportementBouttonRetour()
    {
        SceneManager.LoadScene(3);
    }
    
    private static void ComportementBouttonTrajet()
    {
        SceneManager.LoadScene(11);
    }
    
}

