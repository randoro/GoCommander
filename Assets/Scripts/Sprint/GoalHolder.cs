using UnityEngine;
using System.Collections;

public class GoalHolder : MonoBehaviour {

    // Use this for initialization
    //int id;
    //int type;
    public Treasure treasure;
    public bool canBeClicked;
    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

    }

    public void Initialize(int id, double lat, double lng)
    {
        //this.id = id;
        //id = Random.Range(0, 3);
        //type = Random.Range(0, 3);
        treasure = new Treasure(id, lat, lng, 0, 0);
        SetChestColor();
    }

    private void SetChestColor()
    {
        if (treasure != null)
        {
            GameObject p10 = transform.Find("pCube10").gameObject;
            if (p10 != null)
            {
                Material[] mats = p10.GetComponent<MeshRenderer>().materials;
                p10.GetComponent<MeshRenderer>().material = mats[treasure.type];
                GetComponent<MeshRenderer>().material = mats[treasure.type];
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
