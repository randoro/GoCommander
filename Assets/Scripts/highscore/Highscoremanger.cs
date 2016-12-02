using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Highscoremanger : MonoBehaviour {

	public Text hightext;
	private string result;
	public int qwizscore;
	public int puzzelscore;
	public int memoryscore;
	int totalscore;

    List<HighscoreList> highscoreList;
	public string[] scoreArray;
    public string username;
    bool loadTotal = true;


	// Use this for initialization
	void Start () {
        //username = GoogleMap.username;
        username = GoogleMap.username;
        highscoreList = new List<HighscoreList>();
        StartCoroutine(GetScore(username));
		//StopCoroutine (Getallusserscore (1));
        AllScore();
    }
	
	// Update is called once per frame
	void Update () {
		if (username == null) {
			print ("nåll");
		}
			
	}

	public void Yourscore(){

        for (int i = 0; i < highscoreList.Count; i++)
        {
            
        }
	}

	public void AllScore(){
        totalscore = 0;
        for (int i = 0; i < highscoreList.Count; i++)
        {
            totalscore += highscoreList[i].score;
        }

		hightext.text = "total " + totalscore;
	}

	public void QuizScore(){
        for (int i = 0; i < highscoreList.Count; i++)
        {
            if(highscoreList[i].game.Contains("Quiz"))
            {
                result = highscoreList[i].name + "  " + highscoreList[i].game + "  " + highscoreList[i].score.ToString();
            }
        }
        hightext.text = result;
    }

	public void PuzzleScore(){
        for (int i = 0; i < highscoreList.Count; i++)
        {
            if (highscoreList[i].game.Contains("Puzzle"))
            {
                result = highscoreList[i].name + "  " + highscoreList[i].game + "  " + highscoreList[i].score.ToString();
            }
        }
        hightext.text = result;
    }

	public void MemoryScore(){
        for (int i = 0; i < highscoreList.Count; i++)
        {
            if (highscoreList[i].game.Contains("Memory"))
            {
                result = highscoreList[i].name + "  " + highscoreList[i].game + "  " + highscoreList[i].score.ToString();
            }
        }
        hightext.text = result;
	}

	public void SprintScore(){
        for (int i = 0; i < highscoreList.Count; i++)
        {
            if (highscoreList[i].game.Contains("Sprint"))
            {
                result = highscoreList[i].name + "  " + highscoreList[i].game + "  " + highscoreList[i].score.ToString();
            }
        }
        hightext.text = result;
    }
	IEnumerator Getallusserscore(int id_test){
		
		string memoryURL = "http://gocommander.sytes.net/scripts/get_highscore.php";

		WWWForm form = new WWWForm();
		form.AddField("idPost", id_test);

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
			string game = GetDataValue(scoreArray[i], "Game:");
			int point = int.Parse(GetDataValue(scoreArray[i], "Score:"));
			string name = GetDataValue(scoreArray[i], "Username:");

			highscoreList.Add(new HighscoreList(id, name, game, point));
		}
		id_test =id_test+ 1;
	}




    IEnumerator GetScore(string in_name)
    {
        string memoryURL = "http://gocommander.sytes.net/scripts/get_highscore.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);

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
            string game = GetDataValue(scoreArray[i], "Game:");
            int point = int.Parse(GetDataValue(scoreArray[i], "Score:"));
            string name = GetDataValue(scoreArray[i], "Username:");

            highscoreList.Add(new HighscoreList(id, name, game, point));
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
