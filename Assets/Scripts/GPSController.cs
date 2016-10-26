using UnityEngine;
using System.Collections;

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

    
}
