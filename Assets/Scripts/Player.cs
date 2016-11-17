using UnityEngine;
using System.Collections;

public class Player  {

	public int id;
    public string name;
    public double lat;
    public double lng;
    public string message;

    public Player(int id, string name, double lat, double lng)
    {
        this.id = id;
        this.name = name;
        this.lat = lat;
        this.lng = lng;
    }

    public Player(int id, string name, double lat, double lng, string message)
    {
        this.id = id;
        this.name = name;
        this.lat = lat;
        this.lng = lng;
        this.message = message;
    }
}
