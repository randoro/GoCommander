using UnityEngine;
using System.Collections;

public class TreasureHolder : MonoBehaviour
{
    public Treasure treasure;
    
	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        
    }

    public void Initialize(int id, double lat, double lng)
    {
        treasure = new Treasure(id, lat, lng);
    }
}
