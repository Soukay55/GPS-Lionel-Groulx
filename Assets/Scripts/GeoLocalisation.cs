using System;
using UnityEngine;
using System.Collections;

public class GeoLocalisation : MonoBehaviour
{
    private void Start()
    {
        //throw new NotImplementedException();
        StartCoroutine(GPSLoc());
    }

    private IEnumerator GPSLoc()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            yield break;

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        var maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            print("Location: " +
                  Input.location.lastData.latitude + " "
                  + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " "
                  + Input.location.lastData.horizontalAccuracy
                  + " " + Input.location.lastData.timestamp);
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }
}