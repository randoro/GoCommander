using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscoremanger : MonoBehaviour {

	public Text hightext;
	public string test;
	public int qwizscore;
	public int puzzelscore;
	public int memoryscore;

	// Use this for initialization
	void Start () {
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
		test = "namn efternamn  "+ qwizscore + " nr1 ";
		hightext.text = test;
		qwizscore =qwizscore+1;
	}
	public void LoadScores(){
		
	}

}
