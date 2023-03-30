using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITéléporteur //ESCALIERS ET ASCENSEURS are tekeporting nodesss
{
   const int COUT_TÉLEPORTATION = 3;//abject
   
   void ChangeFloors(Node currentNode, bool monter)
   {
      if (monter=true)
      {
         if (currentNode.Niveau != Étage.O)
         {
            currentNode.Niveau++;
            return;
            
         }
      }

      if (currentNode.Niveau != Étage.O)
      {
         currentNode.Niveau--;
      }
      return;
      
   }
}
