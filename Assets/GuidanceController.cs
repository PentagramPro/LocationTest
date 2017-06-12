using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuidanceController : MonoBehaviour
{
    struct LocationDouble
    {
        public double lat, lon;
    }

    public Text StatusText;
    public Text InfoText;
    LocationDouble trackedLocation;
    CTDelay compassFiltered = new CTDelay(0,0.9f);
	// Use this for initialization
	void Start () {
	    Input.location.Start(0.5f,1.0f);
	    Input.compass.enabled = true;

	}

   

    float DegreeBearing(
    double lat1, double lon1,
    double lat2, double lon2)
    {
        var dLon = ToRad(lon2 - lon1);
        var dPhi = Math.Log(
            Math.Tan(ToRad(lat2) / 2.0 + Math.PI / 4.0) / Math.Tan(ToRad(lat1) / 2.0 + Math.PI / 4.0));
        if (Math.Abs(dLon) > Math.PI)
            dLon = dLon > 0 ? -(2.0 * Math.PI - dLon) : (2.0 * Math.PI + dLon);
        return (float)ToBearing(Math.Atan2(dLon, dPhi));
    }

    public static double ToRad(double degrees)
    {
        return degrees * (Mathf.PI / 180.0f);
    }

    public static double ToDegrees(double radians)
    {
        return radians * 180.0f / Mathf.PI;
    }

    public static double ToBearing(double radians)
    {
        // convert radians to degrees (as bearing: 0...360)
        return (ToDegrees(radians) + 360.0) % 360.0;
    }

    LocationDouble GetAccurate(LocationInfo location)
    {
        LocationDouble res = new LocationDouble();
        try
        {
            res.lat = double.Parse(location.latitude.ToString("R"));
            res.lon = double.Parse(location.longitude.ToString("R"));
        }
        catch (Exception e) { }
        return res;
    }
    // Update is called once per frame
    void Update ()
    {
        StatusText.text = Input.location.status.ToString();
        if (Input.location.status != LocationServiceStatus.Running)
        {
            return;
        }
        var curLocation = GetAccurate(Input.location.lastData);
       
        
        float direction = DegreeBearing(curLocation.lat, curLocation.lon, trackedLocation.lat,
            trackedLocation.lon);

        float compass = compassFiltered.Next(Input.compass.trueHeading, Time.deltaTime);
        float localDirection = compass - direction;
        transform.rotation = Quaternion.Euler(0,0,localDirection);


        InfoText.text = string.Format("cur: {0:#.0000000}, {1:#.0000000}\ntar: {2:#.0000000}, {3:#.0000000}\n compass: {4:000.0}, absDir: {5:000.0}\n localdir: {6:000.0}",
            curLocation.lat, curLocation.lon, trackedLocation.lat,
            trackedLocation.lon, compass, direction, localDirection);
    }

    public void OnTrackLocation()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            trackedLocation = GetAccurate(Input.location.lastData);
        }
    }
}
