using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TreasureList
{

    public int id;
    public double lat;
    public double lng;

    public TreasureList(int id, double lat, double lng)
    {
        this.id = id;
        this.lat = lat;
        this.lng = lng;
    }
}
