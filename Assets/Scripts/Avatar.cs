using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public string Username;
    public Color Couleur;
    

    // Update is called once per frame
    public Avatar(string username, Color couleur)
    {
        Username = username;
        Couleur = couleur;
    }
}
