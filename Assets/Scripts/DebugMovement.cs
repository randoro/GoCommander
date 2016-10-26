using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugMovement : MonoBehaviour {

    public GameObject lat_inputField;
    public GameObject long_inputField;
    public GameObject scale_inputField;
    private GameObject map;
    private GoogleMap gMap;
    private GPSController gps;

    float lat_value;
    float long_value;
    int scale_value;

    // Use this for initialization
    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();
        gps = map.GetComponent<GPSController>();
    }

    public void buttonClicked()
    {
        bool success1 = false;
        bool success2 = false;
        bool success3 = false;
        success1 = float.TryParse(lat_inputField.GetComponent<InputField>().text, out lat_value);
        success2 = float.TryParse(long_inputField.GetComponent<InputField>().text, out long_value);
        success3 = int.TryParse(scale_inputField.GetComponent<InputField>().text, out scale_value);
        if (!success1 || !success2 || !success3)
        {
            print("Use numbers only when cheating.");
        }
        else
        {
            print("cheating lat:"+ lat_value+" long:"+long_value+" scale:"+scale_value);
        }


        gps.StopRefreshing();

        GoogleMap.centerLocation.address = "";
        GoogleMap.centerLocation.latitude = lat_value;
        GoogleMap.centerLocation.longitude = long_value;
        gMap.zoom = scale_value;
        gMap.Refresh();


    }
}
