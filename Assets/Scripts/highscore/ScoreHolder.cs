using UnityEngine;
using System.Collections;

public class ScoreHolder : MonoBehaviour {

    public int quizscore;
	public int puzzlescore;
	public int memoryscore;
 	public int total_score;

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
		return quizscore;
	}
	public int Getpuzzelscore()
	{
		return puzzlescore;
	}
	public int Getmemoryscore()
	{
		return memoryscore;
	}

}
