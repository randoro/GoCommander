using System;
using UnityEngine;
using System.Collections;

public class GoogleMap : MonoBehaviour {

    public enum MapType
    {
        RoadMap,
        Satellite,
        Terrain,
        Hybrid
    }
    
    public GoogleMapLocation centerLocation;
    public int zoom = 16;
    public MapType mapType;
    public int size = 640;
    public bool doubleResolution = true;
    public Color landscapeColor = Color.green;
    public Color roadColor = Color.white;
    public Color waterColor = Color.blue;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        StopCoroutine(_Refresh());
        StartCoroutine(_Refresh());
    }

    IEnumerator _Refresh()
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
        qs += "&style=feature:road|element:geometry|visibility:on|color:"+ColorTypeConverter.ToHTMLRGBHex(roadColor);
        qs += "&style=feature:landscape|element:geometry.fill|visibility:on|color:"+ColorTypeConverter.ToHTMLRGBHex(landscapeColor);
        qs += "&style=feature:water|element:geometry.fill|visibility:on|color:"+ColorTypeConverter.ToHTMLRGBHex(waterColor);
        
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
