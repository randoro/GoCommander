using UnityEngine;
using System.Collections;

public class ScoreHolder : MonoBehaviour {
	 int qwizscore;
	 int puzzelscore;
	 int memoryscore;
 	public int score;
	//HighScoreHolder
	// Use this for initialization
	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}
	void Start () {
		


	
	}
	
	// Update is called once per frame
	void Update () {
		
	
	} 

	public int Getqwizscore()
	{
		return qwizscore;
	}
	public int Getpuzzelscore()
	{
		return puzzelscore;
	}
	public int Getmemoryscore()
	{
		return memoryscore;
	}
	public void setpuzzelscore(int s )
	{
		s = puzzelscore;
	}

	public void setmemoryscore(int p )
	{
		p = memoryscore;
	}
}
