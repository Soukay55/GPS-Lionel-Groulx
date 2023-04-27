using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class MenuNavigateur : MonoBehaviour
{
    private Transform joueur;
    public Button personneUn;
    public Button personneTrois;
    private void Start()
    {
        personneUn.gameObject.SetActive(true);
        personneTrois.gameObject.SetActive(false);
        personneUn.onClick.AddListener(ComportementBouttonUn);
        personneTrois.onClick.AddListener(ComportementBouttonTrois);
    }

    private void Update()
    {
        if (personneUn.gameObject.activeSelf == true)
        {
            Camera.main.transform.position= joueur.transform.position + new Vector3(0, 5, 0);
        }
        else
        {
            Camera.main.transform.position= joueur.transform.position;
        }
    }

    private void ComportementBouttonUn()
    {
        personneUn.gameObject.SetActive(false);
        personneTrois.gameObject.SetActive(true);
    }
    private void ComportementBouttonTrois()
    {
        personneUn.gameObject.SetActive(true);
        personneTrois.gameObject.SetActive(false);
    }
    
    
}
