using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TreasureList
{

    public int id;
    public float lat;
    public float lng;

    public TreasureList(int id, double lat, double lng)
    {
        this.id = id;
        this.lat = (float)lat;
        this.lng = (float)lng;
    }
}
