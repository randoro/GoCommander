using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscoremanger : MonoBehaviour {

	public Text hightext;
	public string result="";
	public int qwizscore;
	public int puzzelscore;
	public int memoryscore;
	int totalscore;
	public int switchiro = 0;
	public string[] scoreArray;
    public string username;

	GameObject scoremanager;
	//HighScoreHolder


	// Use this for initialization
	void Start () {
        username = GoogleMap.username;
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

		hightext.text = result;
	}

	public void LoadScores(){
		memoryscore = scoremanager.GetComponent<ScoreHolder> ().Getmemoryscore();
		puzzelscore= scoremanager.GetComponent<ScoreHolder> ().Getpuzzelscore();
		qwizscore = scoremanager.GetComponent<ScoreHolder> ().Getqwizscore();
	}

	public void Yourscore(){
		result = "dinscore \n qwizscore= "+ qwizscore + "\n puzzelscore= "+ puzzelscore+ "\n memoryscore= "+ memoryscore ;
	}
	public void Evreonesscore(){
		totalscore = memoryscore + puzzelscore + qwizscore;
		//lodescore från server
		result = "total " + totalscore;

	}
	public void Qscore(){
       /// result = "q";
        StartCoroutine(GetHighscore("Quiz"));
    }

	public void Pscore(){
       // result = "s";
        StartCoroutine(GetHighscore("Puzzle"));
    }

	public void Mscore(){
		StartCoroutine (GetHighscore ("Memory"));

		}

	public void Sscore(){
        //result = "s";
        StartCoroutine(GetHighscore("Sprint"));
    }

	IEnumerator GetHighscore(string in_game)
	{
		string memoryURL = "http://gocommander.sytes.net/scripts/get_highscore.php";
		
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("userGamePost", in_game);

        WWW www = new WWW(memoryURL, form);
		yield return www;
		string result = www.text;

		if (result != null)
		{
			scoreArray = result.Split(';');
		}

		for (int i = 0; i < scoreArray.Length - 1; i++)
		{

			int id = int.Parse(GetDataValue(scoreArray[i], "ID:"));
			string game = GetDataValue (scoreArray [i], "Game:");
			int point = int.Parse(GetDataValue(scoreArray[i],"Score:"));
			string name = GetDataValue (scoreArray [i], "Username:");
			this.result = name + "  " + game+ "  "+ point.ToString();

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
