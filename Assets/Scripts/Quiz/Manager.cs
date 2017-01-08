using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Net;
using SimpleJSON;

public class Manager : MonoBehaviour
{
    public static List<QuizList> allQuestionsList = new List<QuizList>();
    private List<int> alreadyAnsweredQuestions = new List<int>();
    private string[] allQuestions, allAnswersID, Quiz;
    
    public int score = 0;
    int answeredQuestions = 0;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1;
    public static int randomVal;

    public static bool win = false;

    private string line, answerline;
    public static string username = GoogleMap.username;
    
    public Camera camera;

    private Color defaultBackgroundColor;

    public Text questionText;

    // Use this for initialization
    private void Start()
    {
        ClearData();

        allQuestions = new string[100];
        allAnswersID = new string[100];
        
        StartCoroutine(GetQuizes());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!win)
        {
            ReadFromServer();
            SetQuestion();
            QuizSystem();
            CheckWinCondition();
        }

        if (win)
		{
            win = false;
            CheckWinCondition();
            StartCoroutine(SendGroupScore(score));
			StartCoroutine(SendHighscore(score));
            GoogleMap.completedMinigames++;
            SceneManager.LoadScene("mainScene");
        }
    }

    IEnumerator GetQuizes()
    {
        string quizURL = "http://gocommander.sytes.net/scripts/get_quiz.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);

        WWW www = new WWW(quizURL, form);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            Quiz = result.Split(';');
        }

        for (int i = 0; i < Quiz.Length - 1; i++)
        {
            string position = GetDataValue(Quiz[i], "Position:");
            string question = GetDataValue(Quiz[i], "Question:");
            string alt1 = GetDataValue(Quiz[i], "Wrong_1:");
            string alt2 = GetDataValue(Quiz[i], "Wrong_2:");
            string alt3 = GetDataValue(Quiz[i], "Wrong_3:");
            string alt4 = GetDataValue(Quiz[i], "Correct:");
            string answer = GetDataValue(Quiz[i], "Answer:");
            string image = GetDataValue(Quiz[i], "Image");

            allQuestionsList.Add(new QuizList(position, question, alt1, alt2, alt3, alt4, answer, image, false));
        }

        ///////MOVED FROM START////////////
        defaultBackgroundColor = camera.backgroundColor;
        answeredQuestions = 0;
        alreadyAnsweredQuestions.Clear();
        //randomQuestion = Random.Range(0, allQuestionsList.Count);
        RandomizeQuestion();
    }

    void ClearData()
    {
        if(allQuestionsList.Count < 0)
        {
            allQuestionsList.Clear();
        }

        if(alreadyAnsweredQuestions.Count < 0)
        {
            alreadyAnsweredQuestions.Clear();
        }
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(1);

        if (!win)
        {
            RandomizeQuestion();
        }
        else
        {
            CheckWinCondition();
        }
    }

    void SetQuestion()
    {
        if (randomQuestion == -1)
        {
            RandomizeQuestion();
        }
        if (randomQuestion > -1)
        {
            questionText.text = allQuestions[randomQuestion];
        }
    }

    void QuizSystem()
    {
        if (choiceSelected == "y")
        {
            choiceSelected = "n";

            answeredQuestions++;
            if (allAnswersID[randomQuestion].Contains(selectedAnswer))
            {
                camera.backgroundColor = Color.green;
                //alreadyAnsweredQuestions.Add(randomQuestion);
                //answeredQuestions++;
                //allQuestionsList.RemoveAt(randomQuestion);
                //allQuestionsList[randomQuestion].isAlreadyAnswered = true;
                //LoadImage.loadImage = true;
                score += 230;

                StartCoroutine(delayTime());
            }
            else
            {
                camera.backgroundColor = Color.red;
                //alreadyAnsweredQuestions.Add(randomQuestion);
                StartCoroutine(delayTime());
            }
        }
    }

    void CheckWinCondition()
    {
        if (answeredQuestions == 4)
        {
            StartCoroutine(SendCompletedMinigame());
            InformativeMessage.finished = true;

			win = true;
        }
    }

    IEnumerator SendCompletedMinigame()
    {
        string message = GoogleMap.username + " completed a Quiz!";
        string loginUserURL = "http://gocommander.sytes.net/scripts/send_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        form.AddField("userMiniMessagePost", message);

        WWW www = new WWW(loginUserURL, form);

        yield return www;
    }
	IEnumerator SendGroupScore(int score)
	{
		string scoreURL = "http://gocommander.sytes.net/scripts/score_send_group.php";

		WWWForm form = new WWWForm();
		form.AddField("userScorePost", score);
		form.AddField("userGroupPost", GoogleMap.groupName);

		WWW www = new WWW(scoreURL, form);

		yield return www;
	}

	IEnumerator SendHighscore(int score)
	{
		string scoreURL = "http://gocommander.sytes.net/scripts/highscore_send.php";

		WWWForm form = new WWWForm();
		form.AddField("usernamePost", GoogleMap.username);
		form.AddField("userScorePost", score);
		form.AddField("userGamePost", "Quiz");

		WWW www = new WWW(scoreURL, form);

		yield return www;
	}

    void ReadFromServer()
    {
        for (int i = 0; i < allQuestionsList.Count; i++)
        {
            allQuestions[i] = allQuestionsList[i].question;
            allAnswersID[i] = allQuestionsList[i].answer;
        }
    }

    void GenerateNewQuestion()
    {
        camera.backgroundColor = defaultBackgroundColor;

        answeredQuestions++;

        RandomizeQuestion();
    }

    public void RandomizeQuestion()
    {
        camera.backgroundColor = defaultBackgroundColor;

        randomQuestion = Random.Range(0, allQuestionsList.Count);

        while (alreadyAnsweredQuestions.Contains(randomQuestion))
        {
            allQuestionsList.RemoveAt(randomQuestion);
            alreadyAnsweredQuestions.Remove(randomQuestion);

            randomQuestion = Random.Range(0, allQuestionsList.Count);
        }

        allQuestionsList[randomQuestion].isAlreadyAnswered = true;
        alreadyAnsweredQuestions.Add(randomQuestion);
        LoadImage.loadImage = true;
        //QuizSystem();
        //randomVal = randomQuestion;
    }
    
}