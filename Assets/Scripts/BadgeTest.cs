﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeTest : MonoBehaviour {

    public Button commanderBadgeButton;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
    public void OnClick()
    {
        commanderBadgeButton.enabled = false;
    }

}