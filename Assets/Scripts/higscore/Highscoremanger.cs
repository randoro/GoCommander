using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscoremanger : MonoBehaviour {

	public Text hightext;
	public string test;
	public int qwizscore;
	public int puzzelscore;
	public int memoryscore;
	int totalscore;
	public bool shiftscore = false;

	GameObject scoremanager;
	//HighScoreHolder


	// Use this for initialization
	void Start () {
		scoremanager = GameObject.Find ("HighScoreHolder").gameObject;
		LoadScores ();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (shiftscore == false) {
			Yourscore ();
		}
		if (shiftscore == true) {
			
			Evreonesscore ();
		}

		hightext.text = test;


	}
	public void LoadScores(){
		memoryscore = scoremanager.GetComponent<ScoreHolder> ().Getmemoryscore();
		puzzelscore= scoremanager.GetComponent<ScoreHolder> ().Getpuzzelscore();
		qwizscore = scoremanager.GetComponent<ScoreHolder> ().Getqwizscore();

	}
	public void Yourscore(){
		test = "dinscore \n qwizscore= "+ qwizscore + "\n puzzelscore= "+ puzzelscore+ "\n memoryscore= "+ memoryscore ;
	

	}
	public void Evreonesscore(){
		totalscore = memoryscore + puzzelscore + qwizscore;
		//lodescore från server
		test="user"+totalscore;
	//http://gocommander.sytes.net/scripts/highscore.php

	}


}
