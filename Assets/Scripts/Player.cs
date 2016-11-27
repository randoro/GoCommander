using UnityEngine;
using System.Collections;

public class Player  {

	public int id;
    public string name;
    public double lat;
    public double lng;
    public string message;
    public string vote;
    public int counter;
    public string groupName;

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
    public Player(string vote, int counter, int id, string name, string groupName)
    {
        this.vote = vote;
        this.counter = counter;
        this.id = id;
        this.name = name;
        this.groupName = groupName;
    }
}
