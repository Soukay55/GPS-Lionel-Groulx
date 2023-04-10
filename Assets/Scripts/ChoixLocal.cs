using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChoixLocal : MonoBehaviour
{
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

    [SerializeField] public TMP_Dropdown dropdown1,
        dropdown2;

    public Button button;
    public string userInput1;
    public string userInput2;

    public void Start()
    {
        dropdown2.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        dropdown1.options.Clear();
        var items = new List<string>();
        items.Add(" ");
        items.Add("Aile D");
        items.Add("Aile L");
        items.Add("Aile F");
        items.Add("Aile N");
        items.Add("Aile K");
        items.Add("Aile C");
        items.Add("Aile S");
        items.Add("Aile M");
        items.Add("Aile E");


        foreach (var item in items) dropdown1.options.Add(new TMP_Dropdown.OptionData() { text = item });

        dropdown1.onValueChanged.AddListener(delegate { Dropdown1ValueChangedHappened(dropdown1); });
    }

    public void Dropdown1ValueChangedHappened(TMP_Dropdown dropdown)
    {
        SetObjects(true, true, false);
        var selectedIndex = dropdown.value;
        if (selectedIndex == 1)
            CreateDropdown(CreerListeLocauxAileD(), dropdown2);
        if (selectedIndex == 2)
            CreateDropdown(CreerListeLocauxAileL(), dropdown2);
        if (selectedIndex == 3)
            CreateDropdown(CreerListeLocauxAileF(), dropdown2);
        if (selectedIndex == 4)
            CreateDropdown(CreerListeLocauxAileN(), dropdown2);
        if (selectedIndex == 5)
            CreateDropdown(CreerListeLocauxAileK(), dropdown2);
        if (selectedIndex == 6)
            CreateDropdown(CreerListeLocauxAileC(), dropdown2);
        if (selectedIndex == 7)
            CreateDropdown(CreerListeLocauxAileS(), dropdown2);
        if (selectedIndex == 8)
            CreateDropdown(CreerListeLocauxAileM(), dropdown2);
        if (selectedIndex == 9)
            CreateDropdown(CreerListeLocauxAileE(), dropdown2);

        userInput1 = dropdown.options[selectedIndex].text;
        dropdown2.onValueChanged.AddListener(delegate { Dropdown2ValueChangedHappened(dropdown2); });
    }

    public void Dropdown2ValueChangedHappened(TMP_Dropdown dropdown)
    {
        SetObjects(true, true, true);
        var newSelectedIndex = dropdown.value;
        userInput2 = dropdown.options[newSelectedIndex].text;
        Debug.Log(userInput1);
        Debug.Log(userInput2);
    }


    private void SetObjects(bool etat1, bool etat2, bool etat3)
    {
        dropdown1.gameObject.SetActive(etat1);
        dropdown2.gameObject.SetActive(etat2);
        button.gameObject.SetActive(etat3);
    }

    public void CreateDropdown(List<string> items, TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();

        foreach (var item in items) dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
    }
}