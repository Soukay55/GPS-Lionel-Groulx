using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Contraintes : MonoBehaviour
{
    public TMP_Dropdown dropdown1,
        dropdown2,
        dropdown3,
        dropdownLocal1,
        dropdownLocal2,
        dropdownAile,
        dropdownAileEtage,
        dropdownToilettes,
        dropdownEtages;

    public Button button;

    public TMP_Text text1,
        text2,
        text3,
        text4,
        textToilette,
        textEtage,
        textLocal,
        textAile,
        textAileEtage;

    public string choixDropdown2,
        choixDropdown3,
        choixNbToilettes,
        choixEtage,
        choixAile,
        choixEtageDeAile,
        choixAileLocal,
        choixNbLocal;

    private string localChoisi;

    private void SetObjects(bool etat1, bool etat2, bool etat3, bool etat4, bool etat5, bool etat6, bool etat7,
        bool etat8)
    {
        text1.gameObject.SetActive(etat1);
        dropdown1.gameObject.SetActive(etat2);
        text2.gameObject.SetActive(etat3);
        text3.gameObject.SetActive(etat4);
        dropdown2.gameObject.SetActive(etat5);
        dropdown3.gameObject.SetActive(etat6);
        text4.gameObject.SetActive(etat7);
        button.gameObject.SetActive(etat8);
    }

    private void MakeObjectsDisappear()
    {
        SetObjects(false, false, false, false, false, false, false, false);
    }

    private void SetSpecificObjects(bool etat1, bool etat2, bool etat3, bool etat4, bool etat5, bool etat6, bool etat7,
        bool etat8, bool etat9, bool etat10, bool etat11)
    {
        dropdownLocal1.gameObject.SetActive(etat1);
        dropdownLocal2.gameObject.SetActive(etat2);
        textLocal.gameObject.SetActive(etat3);
        dropdownAile.gameObject.SetActive(etat4);
        dropdownAileEtage.gameObject.SetActive(etat5);
        textAileEtage.gameObject.SetActive(etat6);
        textAile.gameObject.SetActive(etat7);
        dropdownEtages.gameObject.SetActive(etat8);
        textEtage.gameObject.SetActive(etat9);
        dropdownToilettes.gameObject.SetActive(etat10);
        textToilette.gameObject.SetActive(etat11);
    }


    public void CreateDropdown(List<string> items, TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();

        foreach (var item in items) dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
    }

    public List<string> CreerListeDropdown3()
    {
        var items = new List<string>();
        items.Add(" ");
        items.Add(" La cafétéria");
        items.Add("Le carrefour étudiant");
        items.Add(" Toilettes");
        items.Add("Un local spécifique");
        items.Add("Un étage spécifique");
        items.Add("Une aile spécifique");
        items.Add("La bibliothèque");
        return items;
    }

    public List<string> CreerListeDropdown2()
    {
        var items = new List<string>();
        items.Add("");
        items.Add("Passer");
        items.Add("Ne pas passer");
        return items;
    }

    public List<string> CreerListeDropdownToilette()
    {
        var tabNbToilettes = new int[20];
        for (var i = 0; i < tabNbToilettes.Length; i++)
            tabNbToilettes[i] = i + 1;

        var nbToilettes = new List<int>();

        nbToilettes.AddRange(tabNbToilettes);

        var items = nbToilettes.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return items;
    }

    public List<string> CreerListeDropdownAiles()
    {
        var items = new List<string>();
        items.Add(" ");
        items.Add("D");
        items.Add("L");
        items.Add("F");
        items.Add("N");
        items.Add("K");
        items.Add("C");
        items.Add("S");
        items.Add("M");
        items.Add("E");
        return items;
    }

    public List<string> CreerListeDropdownEtages()
    {
        var items = new List<string>();
        items.Add("");
        items.Add("0");
        items.Add("1");
        items.Add("2");
        items.Add("3");
        items.Add("4");
        return items;
    }


    private void Start()
    {
        SetObjects(true, true, false, false, false, false, false, false);
        SetSpecificObjects(false, false, false, false, false, false, false, false, false, false, false);

        dropdown1.onValueChanged.AddListener(delegate { Dropdown1ValueChangedHappened(dropdown1); });

        CreateDropdown(CreerListeDropdown2(), dropdown2);
        CreateDropdown(CreerListeDropdown3(), dropdown3);
        CreateDropdown(CreerListeDropdownToilette(), dropdownToilettes);
        CreateDropdown(CreerListeDropdownAiles(), dropdownAile);
        CreateDropdown(CreerListeDropdownEtages(), dropdownEtages);
        CreateDropdown(CreerListeDropdownAiles(), dropdownLocal1);


        dropdown2.onValueChanged.AddListener(delegate { Dropdown2ValueChangedHappened(dropdown2); });
        dropdown3.onValueChanged.AddListener(delegate { Dropdown3ValueChangedHappened(dropdown3); });
    }

    public void Dropdown1ValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        if (newSelectedIndex == 1)
            SetObjects(false, false, true, true, true, true, true, false);
        if (newSelectedIndex == 2)
            SceneManager.LoadScene(5);
    }

    public void Dropdown2ValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixDropdown2 = dropdown.options[newSelectedIndex].text;
    }

    public void Dropdown3ValueChangedHappened(TMP_Dropdown dropdown)
    {
        var indexChoisi = dropdown.value;
        choixDropdown3 = dropdown.options[indexChoisi].text;
        if (indexChoisi == 1 || indexChoisi == 2 || indexChoisi == 7)
            SetObjects(false, false, true, true, true, true, true, true);
        if (indexChoisi == 3 && choixDropdown2 == "Passer")
        {
            MakeObjectsDisappear();
            SetSpecificObjects(false, false, false, false, false, false, false, false, false, true, true);
            dropdownToilettes.onValueChanged.AddListener(delegate
            {
                DropdownToiletteValueChangedHappened(dropdownToilettes);
            });
        }
        else
        {
            SetObjects(false, false, true, true, true, true, true, true);
        }

        if (indexChoisi == 4)
        {
            MakeObjectsDisappear();
            SetSpecificObjects(true, false, true, false, false, false, false, false, false, false, false);
            dropdownLocal1.onValueChanged.AddListener(delegate { DropdownLocal1ValueChangedHappened(dropdownLocal1); });
        }

        if (indexChoisi == 5)
        {
            MakeObjectsDisappear();
            SetSpecificObjects(false, false, false, false, false, false, false, true, true, false, false);
            dropdownEtages.onValueChanged.AddListener(delegate { DropdownEtageValueChangedHappened(dropdownEtages); });
        }

        if (indexChoisi == 6)
        {
            MakeObjectsDisappear();
            SetSpecificObjects(false, false, false, true, false, false, true, false, false, false, false);
            dropdownAile.onValueChanged.AddListener(delegate { DropdownAileValueChangedHappened(dropdownAile); });
        }
    }

    public void DropdownToiletteValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixNbToilettes = dropdown.options[newSelectedIndex].text;
        SetObjects(false, false, false, false, false, false, false, true);
    }

    public void DropdownEtageValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixEtage = dropdown.options[newSelectedIndex].text;
        SetObjects(false, false, false, false, false, false, false, true);
    }

    public void DropdownAileValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixAile = dropdown.options[newSelectedIndex].text;
        if (choixAile == "K" || choixAile == "E" || choixAile == "M")
        {
            SetSpecificObjects(false, false, false, true, false, false, true, false, false, false, false);
            SetObjects(false, false, false, false, false, false, false, true);
            choixEtageDeAile = "0";
        }
        else
        {
            SetSpecificObjects(false, false, false, true, true, true, true, false, false, false, false);
            if (choixAile == "D" || choixAile == "F")
                CreateDropdown(CreerListeEtageAileDEtF(), dropdownAileEtage);
            if (choixAile == "L")
                CreateDropdown(CreerListeEtageAileL(), dropdownAileEtage);
            if (choixAile == "N")
                CreateDropdown(CreerListeEtageAileN(), dropdownAileEtage);
            if (choixAile == "C")
                CreateDropdown(CreerListeEtageAileC(), dropdownAileEtage);
            if (choixAile == "S")
                CreateDropdown(CreerListeEtageAileS(), dropdownAileEtage);

            dropdownAileEtage.onValueChanged.AddListener(delegate
            {
                DropdownAileEtageValueChangedHappened(dropdownAileEtage);
            });
        }
    }

    public void DropdownAileEtageValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixEtageDeAile = dropdown.options[newSelectedIndex].text;
        SetObjects(false, false, false, false, false, false, false, true);
    }

    public void DropdownLocal1ValueChangedHappened(TMP_Dropdown dropdown)
    {
        SetSpecificObjects(true, true, true, false, false, false, false, false, false, false, false);
        var selectedIndex = dropdown.value;
        if (selectedIndex == 1)
            CreateDropdown(CreerListeLocauxAileD(), dropdownLocal2);
        if (selectedIndex == 2)
            CreateDropdown(CreerListeLocauxAileL(), dropdownLocal2);
        if (selectedIndex == 3)
            CreateDropdown(CreerListeLocauxAileF(), dropdownLocal2);
        if (selectedIndex == 4)
            CreateDropdown(CreerListeLocauxAileN(), dropdownLocal2);
        if (selectedIndex == 5)
            CreateDropdown(CreerListeLocauxAileK(), dropdownLocal2);
        if (selectedIndex == 6)
            CreateDropdown(CreerListeLocauxAileC(), dropdownLocal2);
        if (selectedIndex == 7)
            CreateDropdown(CreerListeLocauxAileS(), dropdownLocal2);
        if (selectedIndex == 8)
            CreateDropdown(CreerListeLocauxAileM(), dropdownLocal2);
        if (selectedIndex == 9)
            CreateDropdown(CreerListeLocauxAileE(), dropdownLocal2);

        choixAileLocal = dropdown.options[selectedIndex].text;
        dropdownLocal2.onValueChanged.AddListener(delegate { DropdownLocal2ValueChangedHappened(dropdownLocal2); });
    }

    public void DropdownLocal2ValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixNbLocal = dropdown.options[newSelectedIndex].text;
        SetObjects(false, false, false, false, false, false, false, true);
    }

    public List<string> CreerListeLocauxAileD()
    {
        var tabLocauxetage1 = new int[27];
        for (var i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 101;

        var tabLocauxetage2 = new int[42];
        for (var i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        var tabLocauxetage3 = new int[29];
        for (var i = 0; i < tabLocauxetage3.Length; i++)
            tabLocauxetage3[i] = i + 301;

        var tabLocauxetage4 = new int[37];
        for (var i = 0; i < tabLocauxetage4.Length; i++)
            tabLocauxetage4[i] = i + 401;

        var locaux = new List<int>();

        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);
        locaux.AddRange(tabLocauxetage3);
        locaux.AddRange(tabLocauxetage4);

        var locauxD = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });

        return locauxD;
    }

    public List<string> CreerListeLocauxAileL()
    {
        var tabLocauxetage0 = new int[16];
        for (var i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 6;

        var tabLocauxetage2 = new int[32];
        for (var i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        var locaux = new List<int>();

        locaux.AddRange(tabLocauxetage0);
        locaux.AddRange(tabLocauxetage2);

        var locauxL = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxL;
    }

    public List<string> CreerListeLocauxAileF()
    {
        var tabLocauxetage1 = new int[22];
        for (var i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 101;

        var tabLocauxetage2 = new int[12];
        for (var i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        var tabLocauxetage3 = new int[26];
        for (var i = 0; i < tabLocauxetage3.Length; i++)
            tabLocauxetage3[i] = i + 301;

        var tabLocauxetage4 = new int[14];
        for (var i = 0; i < tabLocauxetage4.Length; i++)
            tabLocauxetage4[i] = i + 401;

        var locaux = new List<int>();

        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);
        locaux.AddRange(tabLocauxetage3);
        locaux.AddRange(tabLocauxetage4);

        var locauxF = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });

        return locauxF;
    }

    public List<string> CreerListeLocauxAileN()
    {
        var tabLocauxetage1 = new int[11];
        for (var i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 104;

        var tabLocauxetage2 = new int[14];
        for (var i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        var locaux = new List<int>();

        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);

        var locauxN = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxN;
    }

    public List<string> CreerListeLocauxAileE()
    {
        var tabLocauxetage0 = new int[19];
        for (var i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 3;

        var locaux = new List<int>();
        locaux.AddRange(tabLocauxetage0);

        var locauxE = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxE;
    }

    public List<string> CreerListeLocauxAileK()
    {
        var tabLocauxetage0 = new int[11];
        for (var i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 2;

        var locaux = new List<int>();
        locaux.AddRange(tabLocauxetage0);

        var locauxK = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxK;
    }

    public List<string> CreerListeLocauxAileC()
    {
        var tabLocauxetage0 = new int[44];
        for (var i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 2;

        var tabLocauxetage1 = new int[20];
        for (var i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 102;

        var locaux = new List<int>();

        locaux.AddRange(tabLocauxetage0);
        locaux.AddRange(tabLocauxetage1);

        var locauxC = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxC;
    }

    public List<string> CreerListeLocauxAileM()
    {
        var tabLocauxetage1 = new int[37];
        for (var i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 102;

        var tabLocauxetage0 = new int[46];
        for (var i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 2;

        var locaux = new List<int>();

        locaux.AddRange(tabLocauxetage0);
        locaux.AddRange(tabLocauxetage1);

        var locauxM = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxM;
    }

    public List<string> CreerListeLocauxAileS()
    {
        var tabLocauxetage1 = new int[38];
        for (var i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 101;

        var tabLocauxetage2 = new int[50];
        for (var i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        var tabLocauxetage3 = new int[50];
        for (var i = 0; i < tabLocauxetage3.Length; i++)
            tabLocauxetage3[i] = i + 301;

        var tabLocauxetage4 = new int[43];
        for (var i = 0; i < tabLocauxetage4.Length; i++)
            tabLocauxetage4[i] = i + 401;

        var tabLocauxetage5 = new int[52];
        for (var i = 0; i < tabLocauxetage5.Length; i++)
            tabLocauxetage5[i] = i + 501;

        var locaux = new List<int>();

        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);
        locaux.AddRange(tabLocauxetage3);
        locaux.AddRange(tabLocauxetage4);
        locaux.AddRange(tabLocauxetage5);

        var locauxS = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });

        return locauxS;
    }

    public List<string> CreerListeEtageAileDEtF()
    {
        var etages = new List<string>();
        etages.Add("");
        etages.Add("1");
        etages.Add("2");
        etages.Add("3");
        etages.Add("4");
        return etages;
    }

    public List<string> CreerListeEtageAileS()
    {
        var etages = new List<string>();
        etages.Add("");
        etages.Add("1");
        etages.Add("2");
        etages.Add("3");
        etages.Add("4");
        etages.Add("5");
        return etages;
    }

    public List<string> CreerListeEtageAileL()
    {
        var etages = new List<string>();
        etages.Add("");
        etages.Add("0");
        etages.Add("2");
        return etages;
    }

    public List<string> CreerListeEtageAileC()
    {
        var etages = new List<string>();
        etages.Add("");
        etages.Add("0");
        etages.Add("1");
        return etages;
    }

    public List<string> CreerListeEtageAileN()
    {
        var etages = new List<string>();
        etages.Add("");
        etages.Add("1");
        etages.Add("2");
        return etages;
    }
}