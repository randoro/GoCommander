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

    private string line, answerline;
    public static string username = GoogleMap.username;
    
    public Camera camera;

    private Color defaultBackgroundColor;

    public Text questionText;

    // Use this for initialization
    private void Start()
    {
        //StartCoroutine(SendCompletedMinigame());

        allQuestions = new string[4];
        allAnswersID = new string[4];

        StartCoroutine(GetQuizes());

        defaultBackgroundColor = camera.backgroundColor;

        //camera.backgroundColor = Color.green;
        
    }

    // Update is called once per frame
    private void Update()
    {
        ReadFromServer();
        SetQuestion();
        QuizSystem();
        LoadMainScene();
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

            allQuestionsList.Add(new QuizList(position, question, alt1, alt2, alt3, alt4, answer, image));
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
        GenerateNewQuestion();
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

            if (allAnswersID[randomQuestion].Contains(selectedAnswer))
            {
                camera.backgroundColor = Color.green;

                StartCoroutine(delayTime());
                score += 10;
            }
            else
            {
                camera.backgroundColor = Color.red;

                StartCoroutine(delayTime());
            }
        }
    }

    void LoadMainScene()
    {
        if (answeredQuestions == 4)
        {
            //InformativeMessage.isQuizCompleted = true;

            //InformativeMessage.ShowCompletedMinigame();
            StartCoroutine(SendCompletedMinigame());
            InformativeMessage.finished = true;

            SceneManager.LoadScene("mainScene");
        }
    }

    IEnumerator SendCompletedMinigame()
    {
        string message = "completed minigame";
        string loginUserURL = "http://gocommander.sytes.net/scripts/send_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", "Killerbunnies");
        form.AddField("userMiniMessagePost", message);

        WWW www = new WWW(loginUserURL, form);

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
        randomQuestion = -1;
        camera.backgroundColor = defaultBackgroundColor;

        answeredQuestions++;

        //resultObj.GetComponent<TextMesh>().text = "";
    }

    void RandomizeQuestion()
    {
        randomQuestion = Random.Range(0, 3);
        alreadyAnsweredQuestions.Add(randomQuestion);
        print(alreadyAnsweredQuestions);
        print(randomQuestion);

        for (int i = 0; i < alreadyAnsweredQuestions.Count; i++)
        {
            if(randomQuestion == alreadyAnsweredQuestions[i])
            {
                print("Question already answered");

                randomQuestion = Random.Range(0, 3);
            }
        }
        print("new question generated");
        LoadImage.loadImage = true;
    }
    
}