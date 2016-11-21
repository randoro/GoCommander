using UnityEngine;
using System.Collections;

public class Treasure
{
    public int id;
    public double lat;
    public double lng;
    public int type;
    public int visible;
    
    public Treasure(int id, double lat, double lng, int type, int visible)
    {
        this.id = id;
        this.lat = lat;
        this.lng = lng;
        this.type = type;
        this.visible = visible;
    }
}
