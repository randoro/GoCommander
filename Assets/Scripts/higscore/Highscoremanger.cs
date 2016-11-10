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
	public int switchiro = 0;
	public string[] score;

	GameObject scoremanager;
	//HighScoreHolder


	// Use this for initialization
	void Start () {
		scoremanager = GameObject.Find ("HighScoreHolder").gameObject;
		LoadScores ();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (switchiro) {
		case 0:
			Evreonesscore ();
			break;
		case 1:
			Qscore ();
			break;
		case 2:
			Pscore (); 
			break;
		case 3:
			Mscore ();
			break;
		case 4:
			Sscore ();
			break;
		default:
			Evreonesscore ();
			break;
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
		test="total"+totalscore;
	//http://gocommander.sytes.net/scripts/highscore.php

	}
	public void Qscore(){
		test = "qwisscore";
			
		}

	public void Pscore(){
		test = "p";
		}

	public void Mscore(){
		//test = "m";
		print ("mscore");
		StartCoroutine (GetMemory ());

		}

	public void Sscore(){
		test = "e";
		}
	IEnumerator GetMemory()
	{
		string memoryURL = "http://gocommander.sytes.net/scripts/get_highscore.php";
		//WWWForm form = new WWWForm();
		//form.AddField("usernamePost", name);
		WWW www = new WWW(memoryURL);
		yield return www;
		string result = www.text;

		if (result != null)
		{
			score = result.Split(';');
		}

		for (int i = 0; i < score.Length - 1; i++)
		{

			int id = int.Parse(GetDataValue(score[i], "ID:"));
			string game = GetDataValue (score [i], "Game:");
			int point=int.Parse(GetDataValue(score[i],"Score:"));
			string name = GetDataValue (score [i], "Username:");
			test = name + "  " + game+ "  "+point.ToString();

			//print(level);
			//listPuzzle.Add(new PuzzleList(id, level));
		}
	}
	string GetDataValue(string data, string index)
	{
		string value = data.Substring(data.IndexOf(index) + index.Length);
		if (value.Contains("|"))
			value = value.Remove(value.IndexOf("|"));
		return value;
	}



}
