using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoogleMap : MonoBehaviour {

    public enum MapType
    {
        RoadMap,
        Satellite,
        Terrain,
        Hybrid
    }
    
    public static GoogleMapLocation centerLocation = new GoogleMapLocation();
    public int zoom = 16;
    public MapType mapType;
    public int size = 640;
    public static int id;
    public bool doubleResolution = true;
    public Color landscapeColor = Color.green;
    public Color roadColor = Color.white;
    public Color waterColor = Color.blue;
    public static string username;
    public static string groupName;
    public static int completedMinigames = 0;
    public static int groupScore = 0;
    //public static bool lastCommander = true;
    bool settled;
    bool isPaused;


    void Start()
    {
        settled = false;
    }

    public void Refresh()
    {
        StopCoroutine(_Refresh());
        StartCoroutine(_Refresh());
    }

    IEnumerator SendPlayerGPS()
    {
        string sendGPSURL = "http://gocommander.sytes.net/scripts/get_gps.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("userLatPost", centerLocation.latitude.ToString());
        form.AddField("userLongPost", centerLocation.longitude.ToString());
        form.AddField("userGroupPost", groupName);
        WWW www = new WWW(sendGPSURL, form);
        yield return www;

        
    }

    IEnumerator CheckActive()
    {
        //yield return new WaitForSeconds(5000);

        string sendGPSURL = "http://gocommander.sytes.net/scripts/check_player.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);

        WWW www = new WWW(sendGPSURL, form);
        yield return www;

        string result = www.text;

        //if (result != null)
        //{
        //    if (result.Contains("INACTIVE"))
        //    {
        //        SceneManager.LoadScene("login");
        //    }
        //}


    }

    IEnumerator _Refresh()
    {
        if (settled)
        {
            StartCoroutine(CheckActive());
        }

        if (!isPaused)
        {

            var url = "http://maps.googleapis.com/maps/api/staticmap";
            var qs = "";

            // (!autoLocateCenter)
            //{


            //Check for string location
            if (centerLocation.address != "")
                qs += "center=" + WWW.UnEscapeURL(centerLocation.address);
            else
            {
                //Otherwise go with decimal location
                qs += "center=" +
                      WWW.UnEscapeURL(string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude));
            }


            //Add zoom argument
            qs += "&zoom=" + zoom.ToString();


            //Add size argument
            qs += "&size=" + WWW.UnEscapeURL(string.Format("{0}x{0}", size));

            //Add scale argument
            qs += "&scale=" + (doubleResolution ? "2" : "1");

            //Add maptype argument
            qs += "&maptype=" + mapType.ToString().ToLower();


            //Add custom style
            qs += "&style=feature:all|element:labels|visibility:off";
            qs += "&style=feature:road|element:geometry|visibility:on|color:" + ColorTypeConverter.ToHTMLRGBHex(roadColor);
            qs += "&style=feature:landscape|element:geometry.fill|visibility:on|color:" + ColorTypeConverter.ToHTMLRGBHex(landscapeColor);
            qs += "&style=feature:water|element:geometry.fill|visibility:on|color:" + ColorTypeConverter.ToHTMLRGBHex(waterColor);

            //Added project key
            qs += "&key=AIzaSyBhEI422RQO4o7HJKWL2seCkkootwbRMfU";

            WWW req = new WWW(url + "?" + qs);

            while (!req.isDone)
                yield return null;
            if (req.error == null)
            {
                var tex = new Texture2D(size, size);
                tex.LoadImage(req.bytes);
                GetComponent<Renderer>().material.mainTexture = tex;
            }

            print("sent GPS");
            StartCoroutine(SendPlayerGPS());

            GameObject tsG = GameObject.FindGameObjectWithTag("TreasureSpawner");
            TreasureSpawner ts = null;

            if (tsG != null)
            {
                ts = tsG.GetComponent<TreasureSpawner>();
                ts.UpdateTreasureLocations();
            }
            GameObject psG = GameObject.FindGameObjectWithTag("PlayerSpawner");
            PlayerSpawner ps;
            if (psG != null)
            {
                ps = psG.GetComponent<PlayerSpawner>();
                ps.UpdatePlayerLocations();
            }
            GameObject gsG = GameObject.FindGameObjectWithTag("GoalSpawner");
            GoalSpawner gs;
            if (gsG != null)
            {
                gs = gsG.GetComponent<GoalSpawner>();
                gs.UpdateGoalLocation();
            }
            GameObject csG = GameObject.FindGameObjectWithTag("CommanderSpawner");
            CommanderSpawner cs = null;
            if (csG != null)
            {
                cs = csG.GetComponent<CommanderSpawner>();
                cs.UpdateTreasureLocations();
            }

            if (tsG != null)
            {
                if (!MinigameStarter.generated)
                {
                    MinigameStarter.generated = true;
                    settled = true;
                    print("generated treasures");
                    ts.GenerateNewTreasures();


                }
            }
        }

    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
    }
    

}

[System.Serializable]
public class GoogleMapLocation
{
    public string address;
    public float latitude;
    public float longitude;
}

public static class ColorTypeConverter
{
    public static string ToRGBHex(Color c)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }

    public static string ToHTMLRGBHex(Color c)
    {
        return string.Format("0x{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }
}
