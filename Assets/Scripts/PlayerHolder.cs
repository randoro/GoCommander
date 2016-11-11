using UnityEngine;
using System.Collections;

public class PlayerHolder : MonoBehaviour {

    public Player player;
    public int id;
    public string Name;
    public double lat;
    public double lng;
    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

    }

    public void Initialize(int id, string name, double lat, double lng)
    {
        player = new Player(id, name, lat, lng);
        this.id = id;
        this.Name = name;
        this.lat = lat;
        this.lng = lng;
    }


}
