using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscoremanger : MonoBehaviour {

	public Text hightext;
	public string test;
	public int qwizscore;
	public int puzzelscore;
	public int memoryscore;

	GameObject scoremanager;
	//HighScoreHolder


	// Use this for initialization
	void Start () {
		scoremanager = GameObject.Find ("HighScoreHolder").gameObject;
		LoadScores ();
	
	}
	
	// Update is called once per frame
	void Update () {
		test = "namn efternamn \n qwizscore= "+ qwizscore + "\n puzzelscore= "+ puzzelscore+ "\n memoryscore= "+ memoryscore ;
		hightext.text = test;

	}
	public void LoadScores(){
		memoryscore = scoremanager.GetComponent<ScoreHolder> ().Getmemoryscore();
		puzzelscore= scoremanager.GetComponent<ScoreHolder> ().Getpuzzelscore();
		qwizscore = scoremanager.GetComponent<ScoreHolder> ().Getqwizscore();
	}

}
