using UnityEngine;
using System.Collections;

public class Treasure
{
    public int id;
    public float lat;
    public float lng;
    public int type;
    
    public Treasure(int id, double lat, double lng, int type)
    {
        this.id = id;
        this.lat = (float)lat;
        this.lng = (float)lng;
        this.type = type;
    }
}
