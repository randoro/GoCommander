using UnityEngine;
using System.Collections;

public class TreasureHolder : MonoBehaviour
{
    //int id;
    public Treasure treasure;
    public bool canBeClicked;
	// Use this for initialization
	void Start ()
    {
	
	}

    void Update()
    {
        
    }

    public void Initialize(int id, double lat, double lng)
    {
        //this.id = id;
        id = Random.Range(0, 3);
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
