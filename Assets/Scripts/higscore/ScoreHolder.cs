using UnityEngine;
using System.Collections;

public class ScoreHolder : MonoBehaviour {
	public int qwizscore;
	public int puzzelscore;
	public int memoryscore;
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
}
