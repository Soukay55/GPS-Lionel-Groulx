using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSCoordinate
{
    private const float PROP_DEGRÉ_RAD = Mathf.PI / 180,
        RAYON_TERRE_METRE = 6371106;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public GPSCoordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    static float CalculerDistanceEntreDeuxCoordonnées(GPSCoordinate A, GPSCoordinate B)
    {
        //la formule haversine va lowkey fuckup l'accuracy pcq c ptit test aek pyth

        float latARad =(float)(A.Latitude * PROP_DEGRÉ_RAD);
        float latBRad =(float) (B.Latitude * PROP_DEGRÉ_RAD);
        float longARad = (float)(A.Longitude * PROP_DEGRÉ_RAD);
        float longBRad = (float)(B.Longitude * PROP_DEGRÉ_RAD);

        float deltaLong = longBRad - longARad;

        float deltaLat = latBRad - latARad;
        
        float a = Mathf.Sin(deltaLat / 2) * Mathf.Sin(deltaLat / 2) +
                   Mathf.Cos(latARad) * Mathf.Cos(latBRad) *
                   Mathf.Sin(deltaLong / 2) * Mathf.Sin(deltaLong / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        return RAYON_TERRE_METRE* c;

    }

        //bearing ou ps?
    static float CalculerDistanceXEntreDeuxCoordonnées(GPSCoordinate A, GPSCoordinate B)
    {
        float deltaLat = (float) (B.Latitude * PROP_DEGRÉ_RAD - A.Latitude * PROP_DEGRÉ_RAD);
        return Mathf.Abs(CalculerDistanceEntreDeuxCoordonnées(A,B)*Mathf.Sin(deltaLat));
    }
    static float CalculerDistanceYEntreDeuxCoordonnées(GPSCoordinate A, GPSCoordinate B)//le y c le z dans vector3
    {
        float deltaLong = (float) (B.Longitude * PROP_DEGRÉ_RAD - A.Longitude * PROP_DEGRÉ_RAD);
        return Mathf.Abs(CalculerDistanceEntreDeuxCoordonnées(A,B)*Mathf.Sin(deltaLong));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
