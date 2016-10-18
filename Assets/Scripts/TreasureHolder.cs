using UnityEngine;
using System.Collections;

public class TreasureHolder : MonoBehaviour
{
    public Treasure treasure;
    public bool canBeClicked;
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


    void OnTriggerEnter(Collider collision)
    {
        GameObject gm = collision.gameObject;
        if (gm.tag == "PlayerArea")
        {
            canBeClicked = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        GameObject gm = collision.gameObject;
        if (gm.tag == "PlayerArea")
        {
            canBeClicked = false;
        }
    }


}
