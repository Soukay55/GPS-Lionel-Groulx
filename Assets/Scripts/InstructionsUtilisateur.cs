using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionsUtilisateur : MonoBehaviour
{

    public static string instructionsUtilisateur = String.Empty;

    public static TMP_Text instructions;

    public static void GénérerInstructions(List<PathfindingNode> trajetUtilisateur)
    {
        for (int i = 0; i < trajetUtilisateur.Count - 2; i += 2)
            CalculerAngle(trajetUtilisateur[i], trajetUtilisateur[i + 1],
            trajetUtilisateur[i + 2]);


        instructionsUtilisateur += "Continuer tout droit";

        instructionsUtilisateur += PourDMètres(trajetUtilisateur[trajetUtilisateur.Count - 1],
            trajetUtilisateur[trajetUtilisateur.Count - 2]);

        trajetUtilisateur[trajetUtilisateur.Count - 2].Instructions = instructionsUtilisateur;

        trajetUtilisateur[trajetUtilisateur.Count - 1].Instructions = "Vous êtes arrivés.";
        
        

        instructionsUtilisateur = String.Empty;
    }

    public static void CalculerAngle(PathfindingNode nodeA, PathfindingNode nodeB, PathfindingNode nodeC)
    {
        Vector3 vecteurAB = nodeB.Position - nodeA.Position;
        Vector3 vecteurBC = nodeC.Position - nodeB.Position;
        float angleRad;
        float angleDegre;
        angleRad = Mathf.Acos(
            ((vecteurAB.x * vecteurBC.x) + (vecteurAB.y * vecteurBC.y) + (vecteurAB.z * vecteurBC.z)) /
            (vecteurAB.magnitude * vecteurBC.magnitude));
        angleDegre = (angleRad * 180f) / Mathf.PI;
        instructionsUtilisateur += "Continuer tout droit";
        instructionsUtilisateur += PourDMètres(nodeA, nodeB);
        nodeA.Instructions = instructionsUtilisateur;
        AfficherInstructions(nodeA);
        instructionsUtilisateur = String.Empty;
        if (angleDegre < 120)
        {
            Vector3 vecteurAC = vecteurAB + vecteurBC;
            if (vecteurAC.x > 0)
                nodeB.Instructions = "Tourner à gauche";
            else
            {
                nodeB.Instructions = "Tourner à droite";
            }
        }
        AfficherInstructions(nodeB);
    }

    public static string PourDMètres(PathfindingNode nodeB, PathfindingNode nodeA)
    {
        float d;
        d = Mathf.Sqrt(Mathf.Pow((nodeB.Position.x - nodeA.Position.x), 2) +
                       Mathf.Pow((nodeB.Position.y - nodeA.Position.y), 2) +
                       Mathf.Pow((nodeB.Position.z - nodeA.Position.z), 2));
        string nbMetres = $" pour {d} mètres";
        return nbMetres;
    }

    public static void AfficherInstructions(PathfindingNode currentNode)
    {
        instructions.text = currentNode.Instructions;
    }
}