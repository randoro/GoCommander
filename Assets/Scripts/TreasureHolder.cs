using UnityEngine;
using System.Collections;

public class TreasureHolder : MonoBehaviour
{
    //int id;
    int type;
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
        type = Random.Range(0, 3);
        treasure = new Treasure(id, lat, lng);
        SetChestCollor();
    }

    private void SetChestCollor()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;

        if (treasure != null)
        {
            if (type == 0)
            {
                //GetComponent<MeshRenderer>().material.color = Color.red;  
                Debug.Log("Setting color to type 0");
            }
            if (type == 1)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                Debug.Log("Setting color to type 1");
            }
            if (type == 2)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                Debug.Log("Setting color to type 2");
            }
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
