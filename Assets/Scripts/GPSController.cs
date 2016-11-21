using UnityEngine;
using System.Collections;
using System.Net;
using SimpleJSON;
using System.Collections.Generic;
using System.IO;

public class GPSController : MonoBehaviour
{



    public bool autoRefresh = true;
    public int refreshDelay = 20;
    public static float latitude;
    public static float longitude;
    private GoogleMap map;
    



	// Use this for initialization
	void Start ()
	{
	    map = gameObject.GetComponent<GoogleMap>();


        if (autoRefresh)
	    {
            StartCoroutine(RefreshLoop(refreshDelay));
        }
	}

    IEnumerator RefreshLoop(float waitTime)
    {
        while (autoRefresh)
        {
            StopCoroutine(FetchLocation());
            StartCoroutine(FetchLocation());
            StartCoroutine(JsonLocation());
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator FetchLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("GPS not enabled/found");
            yield break;
        }

        // Start service before querying location
        if(Input.location.status != LocationServiceStatus.Running)
        Input.location.Start();

        // Wait until service initializes
        int maxWait = refreshDelay; //20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.LogError("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location, come back to Earth please.");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.LogError("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " +
                  Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " +
                  Input.location.lastData.timestamp);

            GoogleMap.centerLocation.address = "";
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            GoogleMap.centerLocation.latitude = Input.location.lastData.latitude;
            GoogleMap.centerLocation.longitude = Input.location.lastData.longitude;

            

            map.Refresh();
        }

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }


    public void StopRefreshing()
    {
        autoRefresh = false;
        StopCoroutine(RefreshLoop(refreshDelay));
    }


    //JSON OM SPELARENS POSITION
    IEnumerator JsonLocation()
    {
        string city = "";
        string country = "";

        using (WebClient wc = new WebClient())
        {
            double lat = GoogleMap.centerLocation.latitude;
            double lng = GoogleMap.centerLocation.longitude;

            //string json = wc.DownloadString("http://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224%2C-73.961452&sensor=true");
            string json = wc.DownloadString("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat.ToString() + "%2C" + lng.ToString() + "&sensor=true");

            SimpleJSON.JSONNode item = JSON.Parse(json);
            var N = JSON.Parse(json);
            var address_components = N["results"][0]["address_components"];
            int count = N["results"][0]["address_components"].Count;

            for (int i = 0; i < count; i++)
            {
                var longName = address_components[i]["long_name"];
                var type = address_components[i]["types"][0];

                if (type.Value == "country")
                {
                    country = longName.Value;
                    print("country: " + longName);
                }

                if (type.Value == "locality")
                {
                    city = longName.Value;
                    print("city: " + longName);
                }
            }

            string playerURL = "http://gocommander.sytes.net/scripts/get_city_country.php";

            WWWForm form = new WWWForm();
            form.AddField("usernamePost", GoogleMap.username);
            form.AddField("userCityPost", city);
            form.AddField("userCountryPost", country);

            WWW www = new WWW(playerURL, form);

            //InformativeMessage.run = true;

            yield return www;
            //yield return new WaitForSeconds(5);
        }
    }
}

public class AddressComponent
{
    public string long_name { get; set; }
    public string short_name { get; set; }
    public List<string> types { get; set; }
}

public class Location
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Northeast
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Southwest
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Viewport
{
    public Northeast northeast { get; set; }
    public Southwest southwest { get; set; }
}

public class Geometry
{
    public Location location { get; set; }
    public string location_type { get; set; }
    public Viewport viewport { get; set; }
}

public class Result
{
    public List<AddressComponent> address_components { get; set; }
    public string formatted_address { get; set; }
    public Geometry geometry { get; set; }
    public string place_id { get; set; }
    public List<string> types { get; set; }
}

public class RootObject
{
    public List<Result> results { get; set; }
    public string status { get; set; }
}
