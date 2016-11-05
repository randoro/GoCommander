using UnityEngine;
using System.Collections;

public class GoalLooker : MonoBehaviour
{
    private GameObject goalSpawner;
    private GoalSpawner goalS;

    // Use this for initialization
    void Start () {
        goalSpawner = GameObject.FindGameObjectWithTag("GoalSpawner");
        goalS = goalSpawner.GetComponent<GoalSpawner>();

    }
	
	// Update is called once per frame
	void Update () {

	    if (goalS.goalSpawned)
	    {
	        GameObject goal = GameObject.FindGameObjectWithTag("Goal");
            gameObject.transform.LookAt(goal.transform);
        }
	
	}
}
