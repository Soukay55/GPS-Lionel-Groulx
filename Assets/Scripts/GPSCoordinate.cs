using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GPSCoordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    private static float PROP_DEGRÉ_RAD = Mathf.PI / 180;

    public GPSCoordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
    
    public static float CalculerDistanceEntreDeuxCoordonnées(GPSCoordinate A, GPSCoordinate B)
    {
        return PositionRelativeENUCoords(A, B).magnitude;
    }

    //position relative en coordonnées ENU (East-North-Up)
    public static Vector3 PositionRelativeENUCoords(GPSCoordinate A, GPSCoordinate B)
    {
        float PROP_DEGRÉ_RAD = Mathf.PI / 180;
        
        float latRad = (float)A.Latitude * PROP_DEGRÉ_RAD;
        float lonRad = (float)A.Longitude * PROP_DEGRÉ_RAD;
        
        Vector3 posA = CartesianToECEF(A),
            posB=CartesianToECEF(B);

        float x = -1 * (posB.x - posA.x) * Mathf.Sin(lonRad) + (posB.y - posA.y) *
            Mathf.Cos(lonRad);
        float y = -1 * (posB.x - posA.x) * Mathf.Sin(latRad) * Mathf.Cos(lonRad)
                  - (posB.y - posA.y) * Mathf.Sin(lonRad) * Mathf.Sin(latRad) +
                  (posB.z - posA.z) * Mathf.Cos(latRad);
        float z = 0;
        
        //Le z et le y ne représentent pas la même chose physiquement
        //dans l'espace géographique et dans unity
        return new Vector3(x, z, y);

    }

    //Ceci nous donne les coordonées ENU
    public static Vector3 CartesianToECEF(GPSCoordinate coordonée)
    {
        float RAYON = 6378137, //Longeur de l'Axe principal du système mondial géosétique
        APLATISSEMENT = 1 / 298.257223563f,
        ECCENTRICITÉ_TERRE_CARRÉ = (Mathf.Pow(RAYON,2) //déviation de la terre par rapport à un cercle parfait
                      -  Mathf.Pow(RAYON * (1 - APLATISSEMENT),2)) / Mathf.Pow(RAYON,2) ;
        
        float latRad = (float)coordonée.Latitude * PROP_DEGRÉ_RAD;
        float lonRad = (float)coordonée.Longitude * PROP_DEGRÉ_RAD;
        
        //rayon de courbature vertical
        float N = RAYON / Mathf.Sqrt(1 - ECCENTRICITÉ_TERRE_CARRÉ * 
                              Mathf.Pow((Mathf.Sin(latRad)), 2));
        
        float x = N*Mathf.Cos(latRad) * Mathf.Cos(lonRad);
        float y = N*Mathf.Cos(latRad) * Mathf.Sin(lonRad);
        float z = Mathf.Pow((1 - APLATISSEMENT), 2) * N * Mathf.Sin(latRad);
        
        return new Vector3(x,y,z);
    }

    //utilise la matrice de rotation afin de tourner un point autour de l'origine
    public static Vector3 RotateAroundOriginZero(Vector3 point,float angle)
    {
        float angleRad = angle * PROP_DEGRÉ_RAD;
        
        Vector3 pointRot=new Vector3(point.x * Mathf.Cos(angleRad) - point.z * Mathf.Sin(angleRad),
                0,point.x * Mathf.Sin(angleRad) + point.z * Mathf.Cos(angleRad));
        return pointRot;
    }
  
}