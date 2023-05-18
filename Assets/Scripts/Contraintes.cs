using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Contraintes : MonoBehaviour
{

    public École CollègeLio { get; set; }
    public static  PathfindingNode Départ { get; set; }
    public  static PathfindingNode  Arrivée{ get; set; }
    private  static List<PathfindingNode> NodesÀÉviter { get; set; }
    
    private  static List<PathfindingNode>  NodesInévitables { get; set; }
    private static  PathfindingNode ChosenNode { get; set; }
    
    private static  List<Func<Boolean>> contraintes { get; set; }
    public static  Pathfinder Pathfinder { get; set; }
    
    public  List<string> Phrases = new (9);

    public TMP_Dropdown dropdown1,
        dropdown2,
        dropdown3,
        dropdownLocal1,
        dropdownLocal2,
        dropdownAile,
        dropdownAileEtage,
        dropdownToilettes,
        dropdownEtages;

    public Button button,
        bouttonSuivant;

    public TMP_Text text1,
        text2,
        text3,
        text4,
        textToilette,
        textEtage,
        textLocal,
        textAile,
        textContraintes1,
        textContraintes2,
        textContraintes3,
        textContraintes4,
        textContraintes5,
        textContraintes6,
        textContraintes7,
        textContraintes8,
        textContraintes9,
        messageContraintes,
        textAileEtage;


    public   string choixDropdown2,
        choixDropdown3,
        choixNbToilettes,
        choixAile,
        choixEtageDeAile,
        choixAileLocal,
        choixNbLocal;

    private string localChoisi;

    private void Start()
    {
        CollègeLio = new École("Nodes.txt");

        Départ = GetNode(MenuChoixLocation.Départ);
        Arrivée = GetNode(MenuChoixLocation.Destination);
        print(Arrivée);
            
       
        SetObjects(true, true, false, false, false, false, false, false);
        SetSpecificObjects(false, false, false, false, false, false, false, false, false, false, false);
        bouttonSuivant.gameObject.SetActive(false);

        dropdown1.onValueChanged.AddListener(delegate { Dropdown1ValueChangedHappened(dropdown1); });

        CreateDropdown(CreerListeDropdown2(), dropdown2);
        CreateDropdown(CreerListeDropdown3(), dropdown3);
        CreateDropdown(CreerListeDropdownToilette(), dropdownToilettes);
        CreateDropdown(CreerListeDropdownAiles(), dropdownAile);
        CreateDropdown(CreerListeDropdownEtages(), dropdownEtages);
        CreateDropdown(CreerListeDropdownAiles(), dropdownLocal1);


        dropdown2.onValueChanged.AddListener(delegate { Dropdown2ValueChangedHappened(dropdown2); });
        dropdown3.onValueChanged.AddListener(delegate { Dropdown3ValueChangedHappened(dropdown3); });
        button.onClick.AddListener(ComportementBoutton);
        bouttonSuivant.onClick.AddListener(ComportementBouttonSuivant);


        Phrases = new List<string>();
        NodesÀÉviter= new List<PathfindingNode>();
        NodesInévitables= new List<PathfindingNode>();


    }

    void SetObjects(bool etat1, bool etat2, bool etat3, bool etat4, bool etat5, bool etat6, bool etat7, bool etat8)
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

    private void ResetDropdown(TMP_Dropdown dropd1)
    {
        dropd1.value = 0;
    }

    private void ResetAllDropdowns()
    {
        ResetDropdown(dropdown1);
        ResetDropdown(dropdown2);
        ResetDropdown(dropdown3);
        ResetDropdown(dropdownLocal1);
        ResetDropdown(dropdownLocal2);
        ResetDropdown(dropdownAile);
        ResetDropdown(dropdownEtages);
        ResetDropdown(dropdownAileEtage);
        ResetDropdown(dropdownToilettes);
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
        items.Add(" la cafétéria");
        items.Add("le carrefour étudiant");
        items.Add(" toilette/s");
        items.Add("un local spécifique");
        items.Add("une aile spécifique");
        items.Add("la bibliothèque");
        
        var contraintes = Phrases;
        
        //optimizable 
        foreach (var phrase in contraintes)
        {
            if (phrase.Contains("par l'aile"))
                items.Remove("une aile spécifique");
            if (phrase.Contains(" toilette/s"))
                items.Remove(" toilette/s");
            if (phrase.Contains("la bibliothèque"))
                items.Remove("la bibliothèque");
            if (phrase.Contains("le carrefour étudiant"))
                items.Remove("le carrefour étudiant");
            if (phrase.Contains(" la cafétéria"))
                items.Remove(" la cafétéria");
        }
        return items;
    }

    public List<string> CreerListeDropdown2()
    {
        var items = new List<string>();
        items.Add("");
        items.Add("passer");
        items.Add("ne pas passer");
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
    
    //optimizable
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
    
    //optimizable
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

    private void CreerPhrase()
    {
        var phrase=String.Empty;
        
        if (choixDropdown3 == " la cafétéria" || choixDropdown3 == "le carrefour étudiant" ||
            choixDropdown3 == "la bibliothèque")
            phrase = $"Je voudrais {choixDropdown2} par {choixDropdown3}";
        if (choixDropdown3 == "un local spécifique")
            phrase = $"Je voudrais {choixDropdown2} par le local {choixAileLocal}{choixNbLocal}";
        if (choixDropdown3 == "une aile spécifique")
            phrase = $"Je voudrais {choixDropdown2} par l'aile {choixAile}{choixEtageDeAile}00";
        if (choixDropdown2 == "passer" && choixDropdown3 == " toilette/s")
            phrase = $"Je voudrais passer par {choixNbToilettes} {choixDropdown3}";
        if (choixDropdown2 == "ne pas passer" && choixDropdown3 == " toilette/s")
            phrase = $"Je voudrais éviter des toilettes";
        
        Phrases.Add(phrase);
    }

    private void AfficherPhrase()
    {
        List<TMP_Text> textes = new List<TMP_Text>();
        textes.Add(textContraintes1);
        textes.Add(textContraintes2);
        textes.Add(textContraintes3);
        textes.Add(textContraintes4);
        textes.Add(textContraintes5);
        textes.Add(textContraintes6);
        textes.Add(textContraintes7);
        textes.Add(textContraintes8);
        textes.Add(textContraintes9);

        for (int i = 0; i < Phrases.Count; i++)
        {
            textes[i].text = Phrases[i];
        }
    }

    private void AfficherMessage()
    {
        messageContraintes.text = "Vous avez atteint la limite de 9 contraintes";
    }

    public void ComportementBoutton()
    {
        bouttonSuivant.gameObject.SetActive(true);
        
        if (Phrases.Count == 9)
        {
            MakeObjectsDisappear(); // sauf la liste et le bouton suivant
            SetSpecificObjects(false,false,false,false,false,false,false,false,false,false,false);
            AfficherMessage(); 
        }
        else
        {
            CreerPhrase();
            CreateDropdown(CreerListeDropdown3(),dropdown3);
            ResetAllDropdowns();
            SetObjects(false, false, true, true, true, true, true, false);
            SetSpecificObjects(false, false, false, false, false, false, false, false, false, false, false);
            AfficherPhrase();
        }
    }

    public void ComportementBouttonSuivant()
    {
        AnalyserContraintes();
        SceneManager.LoadScene(5);
    }

    void Dropdown1ValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        if (newSelectedIndex == 1)
            SetObjects(false, false, true, true, true, false, true, false);
        if (newSelectedIndex == 2)
            ComportementBouttonSuivant();
    }

    void Dropdown2ValueChangedHappened(TMP_Dropdown dropdown)
    {
        SetObjects(false, false, true, true, true, true, true, false);
        var newSelectedIndex = dropdown.value;
        choixDropdown2 = dropdown.options[newSelectedIndex].text;
    }

    void Dropdown3ValueChangedHappened(TMP_Dropdown dropdown)
    {
        var indexChoisi = dropdown.value;
        choixDropdown3 = dropdown.options[indexChoisi].text;
        if (choixDropdown3 == " la cafétéria" || choixDropdown3=="le carrefour étudiant" || choixDropdown3=="la bibliothèque")
            SetObjects(false, false, true, true, true, true, true, true);
        if (choixDropdown3== " toilette/s" && choixDropdown2 == "passer")
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

        if (choixDropdown3== "un local spécifique")
        {
            MakeObjectsDisappear();
            SetSpecificObjects(true, false, true, false, false, false, false, false, false, false, false);
            dropdownLocal1.onValueChanged.AddListener(delegate { DropdownLocal1ValueChangedHappened(dropdownLocal1); });
        }
        
        if (choixDropdown3 == "une aile spécifique")
        {
            MakeObjectsDisappear();
            SetSpecificObjects(false, false, false, true, false, false, true, false, false, false, false);
            dropdownAile.onValueChanged.AddListener(delegate { DropdownAileValueChangedHappened(dropdownAile); });
        }
    }

    void DropdownToiletteValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixNbToilettes = dropdown.options[newSelectedIndex].text;
        SetObjects(false, false, false, false, false, false, false, true);
    }
    

    //À OPTIMISER
    void DropdownAileValueChangedHappened(TMP_Dropdown dropdown)
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
            }
                );
        }
    }

    void DropdownAileEtageValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixEtageDeAile = dropdown.options[newSelectedIndex].text;
        SetObjects(false, false, false, false, false, false, false, true);
    }

    //à optimiser ASFFFFFFFFFF
    void DropdownLocal1ValueChangedHappened(TMP_Dropdown dropdown)
    {
        SetSpecificObjects(true, true, true, false, false, false, false, false, false, false, false);
        var selectedIndex = dropdown.value;
        if (selectedIndex == 1)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileD(), dropdownLocal2);
        if (selectedIndex == 2)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileL(), dropdownLocal2);
        if (selectedIndex == 3)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileF(), dropdownLocal2);
        if (selectedIndex == 4)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileN(), dropdownLocal2);
        if (selectedIndex == 5)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileK(), dropdownLocal2);
        if (selectedIndex == 6)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileC(), dropdownLocal2);
        if (selectedIndex == 7)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileS(), dropdownLocal2);
        if (selectedIndex == 8)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileM(), dropdownLocal2);
        if (selectedIndex == 9)
            CreateDropdown(MenuChoixLocation.CreerListeLocauxAileE(), dropdownLocal2);

        choixAileLocal = dropdown.options[selectedIndex].text;
        dropdownLocal2.onValueChanged.AddListener(delegate { DropdownLocal2ValueChangedHappened(dropdownLocal2); });
    }


    public void DropdownLocal2ValueChangedHappened(TMP_Dropdown dropdown)
    {
        var newSelectedIndex = dropdown.value;
        choixNbLocal = dropdown.options[newSelectedIndex].text;
        SetObjects(false, false, false, false, false, false, false, true);
    }

    //tte ces fonctions peuvent êtres remplacées par une matrice
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

    //enlève
    private bool ContientPasPasser(string contrainte)
    {
        return contrainte.Contains("pas passer");
    }



    public void AnalyserContraintes()
    {
        if (Phrases.Count == 0)
        {
            Pathfinder = new AStarPathfinder(Départ, Arrivée);
        }
        else
        {
            for (int i = 0; i < Phrases.Count; i++)
            {
                if (Phrases[i].Contains("pas passer"))
                { 
                    CreerListeNodesÀÉviter();
                    // si il ya juste des pas passer
                
                    if (Phrases.TrueForAll(ContientPasPasser))
                    {
                        Pathfinder = new AStarPathfinder(Départ,  Arrivée, NodesÀÉviter);
                    }
                    else
                    {
                        if (Phrases.Count > 1 && Phrases[i].Contains("pas passer") &&
                            Phrases.Where(x => !(x.Contains("pas "))).Count() == 1)
                        {
                            CreerListeNodesÀÉviter();
                            string localACroiser =Phrases[i].Substring(Phrases[i].IndexOf("local") + 6, 4);
                            ChosenNode = GetNode(localACroiser);
                            Pathfinder =  new AStarPathfinder(Départ,  Arrivée, NodesÀÉviter, ChosenNode);
                        } 
                    }
                }

                if (!(Phrases[i].Contains("pas ")))
                {
                    if (Phrases.Count == 1 && Phrases[0].Contains("local"))
                    {
                        string localACroiser =Phrases[i].Substring(Phrases[i].IndexOf("local") + 6, 4);
                        ChosenNode = GetNode(localACroiser);
                        Pathfinder = new AStarPathfinder(Départ,  Arrivée, ChosenNode);
                    }
                    
                }
            }
        }
        
    }
    

    public void CreerListeNodesÀÉviter()
    {

        for (int i = 0; i < Phrases.Count; i++)
        {
            if (Phrases[i].Contains("pas passer"))
            {
                if (Phrases[i].Contains("par l'aile"))
                {
                    FloorGraph floor = CollègeLio.Floors[int.Parse(choixEtageDeAile)];
                    
                    foreach (var node in floor.Nodes)
                    {
                        if ((char)node.Aile == choixAile[0])
                            NodesÀÉviter.Add(node);
                    }
                }

                if (Phrases[i].Contains("toilette/s"))
                {
                    foreach (var floor in CollègeLio.Floors)
                    { 
                        foreach (var node in floor.Nodes)
                        {
                            if (GénérerÉcole.ContientUn( "Toilettes",node))
                                NodesÀÉviter.Add(node);
                        }
                    }
                }
                if (Phrases[i].Contains("local"))
                {
                    string localAEviter = Phrases[i].Substring(Phrases[i].IndexOf("local") + 6, 4);
                    NodesÀÉviter.Add(GetNode(localAEviter));
                }
            }
        }
    }

    public void CréerListeNodesÀPasser()
    {
        foreach (var phrase in Phrases)
        {
            if (!phrase.Contains("pas "))
            {
                if (phrase.Contains("par l'aile"))
                {
                    
                }   
                
                if (phrase.Contains("local"))
                {
                    
                }         
                
                if (phrase.Contains("toilette/s"))
                {
                    
                }
            }
        }
    }

    public PathfindingNode GetNode(string nodeToGet)
    {
        FloorGraph floor = CollègeLio.Floors[GetÉtage(nodeToGet)];
        PathfindingNode noeud = floor.Nodes[0];
        
        foreach (var node in floor.Nodes)
        {
            if (!EstLocal(nodeToGet))
            {
                if (GénérerÉcole.ContientUn(nodeToGet,node))
                    noeud = node; 
            }
            else
            {
                if (GénérerÉcole.ContientUn(nodeToGet.Substring(0,2),node))
                {
                    
                    if (GénérerÉcole.ContientUn("À",node))
                    {
                        var locaux= node.Noms.FindAll(FindAllLocaux);
                        foreach (var localSet in locaux )
                        {
                            string[] nombres = localSet.Split('À');
                            for (int i = 0; i < nombres.Length; i++)
                            {
                                nombres[i] = nombres[i].Remove(0,1);
                            }
                            if (int.Parse(nodeToGet.Remove(0,1)) >= int.Parse(nombres[0]) ||
                                int.Parse(nodeToGet.Remove(0,1)) >= int.Parse(nombres[1]))
                            {
                                noeud = node;
                            }
                        }

                    }
                    else
                    {
                        if (GénérerÉcole.ContientUn(nodeToGet, node))
                        noeud = node;
                    }
                   
                }
            }
        }

        return noeud;
    }

    public bool FindAllLocaux(string nom)
    {
        return nom.Contains("À");
    }

    public int GetÉtage(string node)
    {
        int etage;
        if (EstLocal(node))
            etage = int.Parse(node[1].ToString());
        else
        {
            if (node[0] == 'B')
                etage = 3;
            else
            {
                etage = 0;
            }
        }
        
        return etage;
    }

    public bool EstLocal(string node)
    { 
        return MenuChoixLocation.AILES.Contains(node[0])&&node[1]!='a';
    }

    public   bool PasseParEtage(List<PathfindingNode> path, Étage étage)
    {
        bool passeParEtage = false;
        foreach (var node in path)
        {
            if (node.Niveau == étage)
                passeParEtage = true;
        }

        return passeParEtage;
    }
    public   bool PasseParAile(List<PathfindingNode> path, Aile aile)
    {
        bool passeParAile = false;
        foreach (var node in path)
        {
            if (node.Aile == aile)
                passeParAile = true;
        }

        return passeParAile;
    }

    public   bool PasseParToilette(List<PathfindingNode> path, int nbToilettes)
    {
        return path.Where(x => (x.Nom.Contains("Toilettes"))).Count() == nbToilettes;


    }
}