﻿using UnityEngine;
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
    private string[] allQuestions, allAnswersID, Quiz;

    public Transform resultObj;
    GameObject scoremanager;
    public int score = 0;
    int answeredQuestions = 0;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1;

    private string line, answerline;
    private string username = GoogleMap.username;

    public static bool isGameCompleted;

    // Use this for initialization
    private void Start()
    {
        allQuestions = new string[4];
        allAnswersID = new string[4];

        StartCoroutine(GetQuizes());

        isGameCompleted = false;
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
            GetComponent<TextMesh>().text = allQuestions[randomQuestion];
        }
    }

    void QuizSystem()
    {
        if (choiceSelected == "y")
        {
            choiceSelected = "n";

            if (allAnswersID[randomQuestion].Contains(selectedAnswer))
            {

                resultObj.GetComponent<TextMesh>().text = "Correct";
                resultObj.GetComponent<TextMesh>().color = Color.green;

                StartCoroutine(delayTime());
                score += 10;
            }
            else
            {
                resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
                resultObj.GetComponent<TextMesh>().color = Color.red;

                StartCoroutine(delayTime());
            }
        }
    }

    void LoadMainScene()
    {
        if (answeredQuestions == 4)
        {
            SceneManager.LoadScene("mainScene");

            isGameCompleted = true;
        }
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
        answeredQuestions++;

        resultObj.GetComponent<TextMesh>().text = "";
    }

    void RandomizeQuestion()
    {
        randomQuestion = Random.Range(0, 3);
        LoadImage.loadImage = true;
    }
    
}