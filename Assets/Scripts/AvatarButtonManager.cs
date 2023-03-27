using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvatarButtonManager : MonoBehaviour
{
    [SerializeField]
    //[Header("Boutons")]
    public Button button1,
        button2,
        button3,
        button4;

    public Image image;

    [SerializeField] [Header("Avatar")] public GameObject username;

    //public Avatar avatar;
    public Avatar avatar;
    //public Button[] buttons;

    private bool isRotating, isClicked;

    private const int speed = 10;

    private Quaternion rotDépart;
    // Start is called before the first frame update

    private void Start()
    {
        isClicked = false;
        isRotating = false;
        //boutons = this.gameObject;
        //buttons = new Button[5];
        button2.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        username.SetActive(false);
        rotDépart = avatar.transform.rotation;

        // for (int i = 0; i < buttons.Length; i++)
        // {
        //    //buttons[i] = gameObject.transform.GetChild(i).gameObject;
        //
        // }

        button1.onClick.AddListener(ComportementCouleur);
        button2.onClick.AddListener(ComportementChangementCouleur);
        button3.onClick.AddListener(ComportementChangerNom);
        button4.onClick.AddListener(ComportementQuitter);
    }

    private void ComportementCouleur()
    {
        SetObjects(true, true, false, true);
        image.gameObject.SetActive(true);
        isRotating = true;
        isClicked = true;
    }

    private void ComportementChangementCouleur()
    {
        avatar.Couleur = Random.ColorHSV();

        avatar.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = avatar.Couleur;
    }

    private void ComportementChangerNom()
    {
        isClicked = true;
        SetObjects(false, false, true, true);
        username.SetActive(true);
        // avatar.Username = 
    }

    private void ComportementQuitter()
    {
        if (isClicked = true)
        {
            SetObjects(true, false, true, true);
            image.gameObject.SetActive(false);
            isRotating = false;
            isClicked = false;
            avatar.transform.rotation = rotDépart;
        }
        else
        {
            SceneManager.LoadScene("Menu d'entrée");
        }
    }

    private void SetObjects(bool etat1, bool etat2, bool etat3, bool etat4)
    {
        button1.gameObject.SetActive(etat1);
        button2.gameObject.SetActive(etat2);
        button3.gameObject.SetActive(etat3);
        button4.gameObject.SetActive(etat4);
    }

    // Update is called once per frame
    private void Update()
    {
        if (image.IsActive()) image.color = avatar.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color;

        if (isRotating == true) avatar.transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}