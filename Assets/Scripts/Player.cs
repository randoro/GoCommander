﻿using UnityEngine;
using System.Collections;

public class Player  {

	public int id;
    public string name;
    public float lat;
    public float lng;

    public Player(int id, string name, double lat, double lng)
    {
        this.id = id;
        this.name = name;
        this.lat = (float)lat;
        this.lng = (float)lng;
    }
}