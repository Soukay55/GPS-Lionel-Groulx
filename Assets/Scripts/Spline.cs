using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MapleLibrary
{
    [DllImport("MapleLibrary.dll")]
    public static extern void SplineCoeffs();
}

public class SplineCubique
{
    public SplineCubique(List<Vector3> points)
    {
        
    }
}
