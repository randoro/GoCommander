using UnityEngine;
using System.Collections;

public class Treasure
{
    public int id;
    public double lat;
    public double lng;
    public int type;
    
    public Treasure(int id, double lat, double lng, int type)
    {
        this.id = id;
        this.lat = lat;
        this.lng = lng;
        this.type = type;
    }
}
