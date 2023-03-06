using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LocalValidation : MonoBehaviour
{
    public List<string> CreerListeLocauxAileD()
    {

        int[] tabLocauxetage1 = new int[27];
        for (int i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 101;

        int[] tabLocauxetage2 = new int[42];
        for (int i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        int[] tabLocauxetage3 = new int[29];
        for (int i = 0; i < tabLocauxetage3.Length; i++)
            tabLocauxetage3[i] = i + 301;

        int[] tabLocauxetage4 = new int[37];
        for (int i = 0; i < tabLocauxetage4.Length; i++)
            tabLocauxetage4[i] = i + 401;

        List<int> locaux = new List<int>();

        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);
        locaux.AddRange(tabLocauxetage3);
        locaux.AddRange(tabLocauxetage4);

        List<string> locauxD = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });

        return locauxD;

    }
    
    public List<string> CreerListeLocauxAileL()
    {

        int[] tabLocauxetage0 = new int[16];
        for (int i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 6;

        int[] tabLocauxetage2 = new int[32];
        for (int i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        List<int> locaux = new List<int>();
        
        locaux.AddRange(tabLocauxetage0);
        locaux.AddRange(tabLocauxetage2);
        
        List<string> locauxL = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxL;
    }
    
    public List<string> CreerListeLocauxAileF()
    {

        int[] tabLocauxetage1 = new int[22];
        for (int i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 101;

        int[] tabLocauxetage2 = new int[12];
        for (int i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        int[] tabLocauxetage3 = new int[26];
        for (int i = 0; i < tabLocauxetage3.Length; i++)
            tabLocauxetage3[i] = i + 301;

        int[] tabLocauxetage4 = new int[14];
        for (int i = 0; i < tabLocauxetage4.Length; i++)
            tabLocauxetage4[i] = i + 401;

        List<int> locaux = new List<int>();

        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);
        locaux.AddRange(tabLocauxetage3);
        locaux.AddRange(tabLocauxetage4);

        List<string> locauxF = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });

        return locauxF;

    }
    public List<string> CreerListeLocauxAileN()
    {

        int[] tabLocauxetage1 = new int[11];
        for (int i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 104;

        int[] tabLocauxetage2 = new int[14];
        for (int i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        List<int> locaux = new List<int>();
        
        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);
        
        List<string> locauxN = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxN;
    }
    
    public List<string> CreerListeLocauxAileE()
    {

        int[] tabLocauxetage0 = new int[19];
        for (int i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 3;

        List<int> locaux = new List<int>();
        locaux.AddRange(tabLocauxetage0);

        List<string> locauxE = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxE;
    }
    
    public List<string> CreerListeLocauxAileK()
    {

        int[] tabLocauxetage0 = new int[11];
        for (int i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 2;

        List<int> locaux = new List<int>();
        locaux.AddRange(tabLocauxetage0);

        List<string> locauxK = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxK;
    }
    
    public List<string> CreerListeLocauxAileC()
    {

        int[] tabLocauxetage0 = new int[44];
        for (int i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 2;

        int[] tabLocauxetage1 = new int[20];
        for (int i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 102;

        List<int> locaux = new List<int>();
        
        locaux.AddRange(tabLocauxetage0);
        locaux.AddRange(tabLocauxetage1);
        
        List<string> locauxC = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxC;
    }
    
    public List<string> CreerListeLocauxAileM()
    {

        int[] tabLocauxetage1 = new int[37];
        for (int i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 102;

        int[] tabLocauxetage0 = new int[46];
        for (int i = 0; i < tabLocauxetage0.Length; i++)
            tabLocauxetage0[i] = i + 2;

        List<int> locaux = new List<int>();
        
        locaux.AddRange(tabLocauxetage0);
        locaux.AddRange(tabLocauxetage1);
        
        List<string> locauxM = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });
        return locauxM;
    }
    
    public List<string> CreerListeLocauxAileS()
    {

        int[] tabLocauxetage1 = new int[38];
        for (int i = 0; i < tabLocauxetage1.Length; i++)
            tabLocauxetage1[i] = i + 101;

        int[] tabLocauxetage2 = new int[50];
        for (int i = 0; i < tabLocauxetage2.Length; i++)
            tabLocauxetage2[i] = i + 201;

        int[] tabLocauxetage3 = new int[50];
        for (int i = 0; i < tabLocauxetage3.Length; i++)
            tabLocauxetage3[i] = i + 301;

        int[] tabLocauxetage4 = new int[43];
        for (int i = 0; i < tabLocauxetage4.Length; i++)
            tabLocauxetage4[i] = i + 401;
        
        int[] tabLocauxetage5 = new int[52];
        for (int i = 0; i < tabLocauxetage5.Length; i++)
            tabLocauxetage5[i] = i + 501;

        List<int> locaux = new List<int>();

        locaux.AddRange(tabLocauxetage1);
        locaux.AddRange(tabLocauxetage2);
        locaux.AddRange(tabLocauxetage3);
        locaux.AddRange(tabLocauxetage4);
        locaux.AddRange(tabLocauxetage5);

        List<string> locauxS = locaux.ConvertAll<string>(delegate(int i) { return i.ToString(); });

        return locauxS;
    }
    
    public void Start()
    {
        var dropdownStage = transform.GetComponent<TMP_Dropdown>();
        dropdownStage.options.Clear();
        List<string> items = new List<string>();
        items.Add("Aile D");
        items.Add("Aile L");
        items.Add("Aile F");
        items.Add("Aile N");
        items.Add("Aile K"); 
        items.Add("Aile C");
        items.Add("Aile S");
        items.Add("Aile M");
        items.Add("Aile E");
        
        foreach (var item in items)
        {
            dropdownStage.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }

        DropdownItemSelected(dropdownStage);
        dropdownStage.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdownStage); });
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
    }
    
}



