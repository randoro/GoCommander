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

        //First try to change treasure chests type and color
        //SetChestType(id);
    }

    private void SetChestType(int id)
    {
        switch (id)
        {
            case 0:
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                }
                break;
            case 1:
                {
                    GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                break;
            case 2:
                {
                    GetComponent<MeshRenderer>().material.color = Color.green;
                }
                break;
        }
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
