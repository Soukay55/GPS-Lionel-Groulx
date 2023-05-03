using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCalculation : MonoBehaviour
{
    public TMP_Text texteAffiché;
    public TMP_Text message;
    public  Button naviguer;
    public  Button voirTrajet;
    private PathfindingNode node;

    public  void GenererLoadingScreen(PathfindingNode node, Pathfinder.StatutPathfinder statut)
    {
        if (statut== Pathfinder.StatutPathfinder.EN_MARCHE)
        {
            if(node.Instructions.Contains("Escaliers"))
            {
                texteAffiché.text = "Téléportation";
            }
            else
            {
                texteAffiché.text = "Calculation du trajet";
            }
        }
    }

    //?????????????????????
    public void GenererMessage()
    {
        if (Contraintes.pathfinder.Statut == Pathfinder.StatutPathfinder.SUCCES)
        {
            naviguer.gameObject.SetActive(true);
            naviguer.gameObject.SetActive(true);
        }
        if (Contraintes.pathfinder.Statut == Pathfinder.StatutPathfinder.ÉCHEC)
        {
            message.text =
                "Il est impossible de générer un chemin qui respecte toutes les contraintes que vous avez imposées";
        }
    }

    public void Start()
    {
        naviguer.gameObject.SetActive(false);
        voirTrajet.gameObject.SetActive(false);
        GenererLoadingScreen(node,Contraintes.pathfinder.Statut);
        naviguer.onClick.AddListener(ComportementBouttonNaviguer);
        voirTrajet.onClick.AddListener(ComportementBouttonTrajet);

    }

    public void Update()
    {
        GenererLoadingScreen(node,Contraintes.pathfinder.Statut);
    }

    //à la place, fonction statique
    private static void ComportementBouttonNaviguer()
    {
        SceneManager.LoadScene(7);
    }
    
    private static void ComportementBouttonTrajet()
    {
        SceneManager.LoadScene(8);
    }
    
}

