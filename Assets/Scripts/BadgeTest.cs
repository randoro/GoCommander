using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeTest : MonoBehaviour {

    public GameObject commanderBadgeButton;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
    public void OnClick()
    {
        //if (!commanderBadgeButton.activeInHierarchy)
        //{
        //    commanderBadgeButton.SetActive(true);
        //}
        //else
        //{
        //    commanderBadgeButton.SetActive(false);
        //}
        if (!TreasureSpawner.testIfRadiusCheckWorks)
        {
            TreasureSpawner.testIfRadiusCheckWorks = true;
        }
        else if (TreasureSpawner.testIfRadiusCheckWorks)
        {
            TreasureSpawner.testIfRadiusCheckWorks = false;
        }
    }
}