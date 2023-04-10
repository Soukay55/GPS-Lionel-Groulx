using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyseurContraintes : MonoBehaviour
{
   public List<string> PointsÀÉviter;
   public List<PathfindingNode> PointsInévitables;

   public void AnalyserContraintes(List<string> contraintes)
   {
      foreach (var contrainte in contraintes)
      {
         if (contrainte.Contains("pas"))
         {
           // List<string>
         }
      }
   }

}
