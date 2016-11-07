using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class GoalSpawner : MonoBehaviour
{

    public bool goalSpawned;
    private GameObject map;
    private GoogleMap gMap;
    public GameObject prefab;

    private GoalHolder goalHolder;



    // Use this for initialization
    void Start () {
        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();

    }
	
	// Update is called once per frame
	void Update () {

	    if (!goalSpawned)
	    {
	        if (GoogleMap.centerLocation.latitude != 0 && GoogleMap.centerLocation.longitude != 0)
	        {
	            
	            goalSpawned = true;

                //double xGoal = randomizeAround(GoogleMap.centerLocation.longitude, 0.0030D);
                //double yGoal = randomizeAround(GoogleMap.centerLocation.latitude, 0.0030D);
                double xGoal = (GoogleMap.centerLocation.longitude + 0.0030D);
                double yGoal = (GoogleMap.centerLocation.latitude + 0.0030D);

                print("Create goal at: " + yGoal + " ," + xGoal);

                goalHolder = ((GameObject)Instantiate(prefab,
                            new Vector3(coordScaleToGameScale(xGoal - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                                0.0f, coordScaleToGameScale(yGoal - GoogleMap.centerLocation.latitude, 90.0f, 9.0f)),
                            Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 360, 0)))).GetComponent<GoalHolder>();
                goalHolder.Initialize(0, yGoal, xGoal);
                goalHolder.gameObject.transform.localScale = new Vector3(4f, 4f, 4f);
            }
	    }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                print("clicked");
                if (hit.transform.tag == "Goal")
                {
                    print("clicked on goal");
                    GameObject treasure = hit.transform.root.gameObject;
                    GoalHolder th = treasure.GetComponent<GoalHolder>();
                    if (th.canBeClicked)
                    {
                        print("clicked on goal inside ring");
                        Treasure t = th.treasure;
                        int id = t.id;
                        //int type = t.type;
                        if (id == 0)
                        {
                            print("game won");
                            Application.LoadLevel("mainScene");
                        }
                    }
                }
            }
        }

    }

    private double randomizeAround(double middle, double radius)
    {
        double zeroToOne = Random.value;
        double adder = -radius + (2*radius*zeroToOne);

        return middle + adder;
    }

    private float coordScaleToGameScale(double inFloat, double total, double multi)
    {
        double returnfloat = (inFloat / total) * (multi * (double)Math.Pow(2, gMap.zoom));
        return (float)returnfloat;
    }

    public void UpdateGoalLocation()
    {
            
            Treasure v = goalHolder.treasure;
        goalHolder.transform.position =
                new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                    0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f));
        
    }
}
