using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuCalculation : MonoBehaviour
{
    public TMP_Text texteAffiché;
    private LoadingScreen teleportation;
    private LoadingScreen calulation;
    public void GenererLoadingScreen(List<PathfindingNode> nodes )
    {
        teleportation.gameObject.SetActive(false);
        calulation.gameObject.SetActive(false);
        foreach (var node in nodes)
        {
            if (node.Instructions.Contains("escaliers"))
            {
                teleportation.gameObject.SetActive(true);
                texteAffiché.text = "Téléportation";
            }
            else
            {
                calulation.gameObject.SetActive(true);
                texteAffiché.text = "Calculation du trajet";
            }
        }
    }
}
