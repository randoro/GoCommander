using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscoremanger : MonoBehaviour {

	public Text hightext;
	public string test;
	public int qwizscore;
	public int puzzelscore;
	public int memoryscore;
	MapGenerator1 memory;
	MapGenerator puzzel;
	Manager qwiz;


	// Use this for initialization
	void Start () {
		
		//LoadScores ();
	
	}
	
	// Update is called once per frame
	void Update () {
		test = "namn efternamn \n qwizscore= "+ qwizscore + "\n puzzelscore= "+ puzzelscore+ "\n memoryscore= "+ memoryscore ;
		hightext.text = test;

	}
	public void LoadScores(){
	memoryscore = memory.score;
	puzzelscore = puzzel.score;
		qwizscore = qwiz.score;
			



	}

}
