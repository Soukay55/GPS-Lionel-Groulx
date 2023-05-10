using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MenuChoixDépart : MonoBehaviour
{
    // Start is called before the first frame update
    public École CollègeLio { get; set; }
    
    [SerializeField] public TMP_Dropdown dropdown1,
        dropdown2;

    public Button button;
    public string userInput1;
    public string userInput2;

    public const string AILES = " DLFNKCSME";
    
    public static string Départ { get; set; }
    
    public List<string>Étages { get; set; }
    void Start()
    {
        List<Node> points = GénérerÉcole.GetListePoints(FileReadingTools.LireFichierTxt("OutsideData.txt"));
        CollègeLio = new École("Nodes.txt",points[15]);
        
        dropdown2.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        dropdown1.options.Clear();

        Étages = new List<string>();
        
        for (int i = 0; i < (int)Étage.NombreÉtages; i++)
        {
            Étages.Add("Étage"+i);
        }
        
        MenuChoixDestination.CreateDropdown(MenuChoixDestination.CreerListeAile(),dropdown1);
        dropdown1.onValueChanged.AddListener(delegate { Dropdown1ValueChangedHappened(dropdown1); });
        
    }
    
    public void Dropdown1ValueChangedHappened(TMP_Dropdown dropdown)
    {
        SetObjects(true, true, false);
        var selectedIndex = dropdown.value;
        if (selectedIndex == 1)


        userInput1 = dropdown.options[selectedIndex].text;
        dropdown2.onValueChanged.AddListener(delegate { Dropdown2ValueChangedHappened(dropdown2); });
    }

    public void Dropdown2ValueChangedHappened(TMP_Dropdown dropdown)
    {
        SetObjects(true, true, true);
        var newSelectedIndex = dropdown.value;
        userInput2 = dropdown.options[newSelectedIndex].text;
        Départ = $"{userInput1}{userInput2}";
    }


    private void SetObjects(bool etat1, bool etat2, bool etat3)
    {
        dropdown1.gameObject.SetActive(etat1);
        dropdown2.gameObject.SetActive(etat2);
        button.gameObject.SetActive(etat3);
    }
    
    
    

}
