using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class FileReadingTools
{
    private const string PATH = "Assets/RessourcesGPS/DataNodes";
    static private string délimiteur = "\t" ;
    static private List<string> dataTab;
    static private StreamReader fluxLecture;
    
    public static List<string> LireFichierTxt(string nomFichier)
    {
        var fichierÀlire = $"{PATH}/{nomFichier}";
        fluxLecture = new StreamReader(fichierÀlire);

        //edit linked nodes:only stairs can move to higher levels
        var données = string.Empty;
        using (fluxLecture)
        {
            string ligne;
            while ((ligne = fluxLecture.ReadLine()) != null)
                données += ligne + délimiteur;
        }

        dataTab = données.Split(délimiteur,
            StringSplitOptions.RemoveEmptyEntries).ToList();
        
        return dataTab;
    }

    public static bool ToBool(string valeurBool)
    {
        if (valeurBool == "true")
            return true;

        return false;
    }

    public static List<string> ToNameList(string listeNoms, ref bool plusQueUnNom)
    {
        var noms = listeNoms.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        if (noms.Count > 1)
            plusQueUnNom = true;
        return noms;
    }

    public static Vector3 ToVector3(string point)
    {
        var pt = point.Split(",", StringSplitOptions.RemoveEmptyEntries);
        
        return new Vector3(float.Parse(pt[0]),float.Parse(pt[1]),float.Parse(pt[2])) ;
    }

    public static GPSCoordinate ToGpsCoordinate(string coordonéeGPS)
    {
        var coord = coordonéeGPS.Split("@", StringSplitOptions.RemoveEmptyEntries);

        return new GPSCoordinate(double.Parse(coord[0]), double.Parse(coord[1]));
    }

    //the nodes input string are in the form 1,2,3
    public static List<float> ToList(string nodes)
    {
        var nodeList = new List<float>();
        var nodeListString = new List<string>();
        nodeListString = nodes.Split("-", StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (var node in nodeListString) nodeList.Add(float.Parse(node, CultureInfo.InvariantCulture));
        return nodeList;
    }

    public static Étage ToÉtage(string étage)
    {
        var étages = "ABGHIO";
        return (Étage)étages.IndexOf(étage);
    }
    
}