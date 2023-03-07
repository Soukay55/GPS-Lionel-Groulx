using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
public class GPSLocation : MonoBehaviour
{
   public TextMeshPro statusTxt;
   public void GetUserLocation()
   {
      if( !Input.location.isEnabledByUser ) //FIRST IM CHACKING FOR PERMISSION IF "true" IT MEANS USER GAVED PERMISSION FOR USING LOCATION INFORMATION
      {
         statusTxt.text = "No Permission";
         Permission.RequestUserPermission(Permission.FineLocation);
      }
      else
      {
         statusTxt.text = "Ok Permission";
         StartCoroutine("GetLatLonUsingGPS");
      }
   }
 
   IEnumerator GetLatLonUsingGPS()
   {
      Input.location.Start();
      int maxWait = 5;
      while( Input.location.status == LocationServiceStatus.Initializing && maxWait > 0 )
      {
         yield return new WaitForSeconds(1);
         maxWait--;
      }
      if( maxWait < 1 )
      {
         statusTxt.text = "Failed To Iniyilize in 10 seconds";
         yield break;
      }
      if( Input.location.status == LocationServiceStatus.Failed )
      {
         statusTxt.text = "Failed To Initialize";
         yield break;
      }
      else
      {
         statusTxt.text ="waiting before getting lat and lon";
         // yield return new WaitForSeconds(5);
         // Access granted and location value could be retrieve
         double longitude = Input.location.lastData.longitude;
         double latitude = Input.location.lastData.latitude;
 
         //AddLocation(latitude, longitude);
         statusTxt.text="" + Input.location.status + "  lat:"+latitude+"  long:"+longitude;
      }
      //Stop retrieving location
      Input.location.Stop();
      StopCoroutine("Start");
   }
//    public TextMeshPro latitudeValue,
//       longitudeValue,
//       altitudeValue,
//       horizontalAccuracyValue,
//       timeStampValue;
//
//    public bool isUpdating;
//
//    private void Update()
//    {
//       if (!isUpdating)
//       {
//          StartCoroutine(GPSLoc());
//          isUpdating = !isUpdating;
//       }
//    }
//
//    IEnumerator GPSLoc()
//    {
//       //CHECK FOR PERMISSION OBTENTION
//       if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
//       {
//          Permission.RequestUserPermission(Permission.FineLocation);
//          Permission.RequestUserPermission(Permission.CoarseLocation);
//       }
//       
//       if (!Input.location.isEnabledByUser)
//          yield return new WaitForSeconds(9);
//       
//       Input.location.Start();
//       int maxWait = 9;
//
//       while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
//       {
//          yield return new WaitForSeconds(1);
//          maxWait--;
//       }
//
//       if (maxWait < 1)
//       {
//          GPSStatus.text = "Time out";
//          print("Time out");
//          yield break;
//          
//       }
//
//       if (Input.location.status == LocationServiceStatus.Failed)
//       {
//          GPSStatus.text = "Unable to determine location";
//          print("Unable to determine location");
//          yield break;
//          
//       }
//       else
//       {
//          GPSStatus.text = "Runs";
//          latitudeValue.text = Input.location.lastData.latitude.ToString();
//          altitudeValue.text = Input.location.lastData.altitude.ToString();
//          longitudeValue.text = Input.location.lastData.longitude.ToString();
//          horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();      
//          timeStampValue.text = Input.location.lastData.timestamp.ToString();   
//       }
//    }
//
//    private void UpdateGPSData()
//    {
//       if (Input.location.status==LocationServiceStatus.Running)
//       {
//          GPSStatus.text = "Running";
//          Debug.Log(Input.location.lastData.latitude.ToString());
//       }
//       else
//       {
//         
//       }                                                         
//    }
}
