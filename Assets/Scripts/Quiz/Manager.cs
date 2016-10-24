﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public static List<QuizList> listQuiz = new List<QuizList>();

    private string[] questions, answers, answersID, quiz;

    public Transform resultObj;

    int score = 0;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1, nrOfQuestions;

    private string line, answerline;

    // Use this for initialization
    private void Start () {

        StartCoroutine(GetQuizes());

        questions = new string[4];
        answers = new string[4];
        answersID = new string[4];
    }

    IEnumerator GetQuizes()
    {
        string quizURL = "https://ddwap.mah.se/AC3992/getQuiz.php";

        WWW www = new WWW(quizURL);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            quiz = result.Split(';');
        }

        for (int i = 0; i < quiz.Length - 1; i++)
        {
            string city = GetDataValue(quiz[i], "City:");
            string question = GetDataValue(quiz[i], "Question:");
            string alt1 = GetDataValue(quiz[i], "Wrong_1:");
            string alt2 = GetDataValue(quiz[i], "Wrong_2:");
            string alt3 = GetDataValue(quiz[i], "Wrong_3:");
            string alt4 = GetDataValue(quiz[i], "Correct:");
            string answer = GetDataValue(quiz[i], "Answer:");

            //print(city + question + " " + wrong1 + " " + wrong2 + " " + wrong3 + " " + correct);

            listQuiz.Add(new QuizList(city, question, alt1, alt2, alt3, alt4, answer));
        }
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    // Update is called once per frame
    private void Update()
    {
        if (randomQuestion == -1)
        {
            randomQuestion = Random.Range(0, 2);
        }
        if (randomQuestion > -1)
        {
            GetComponent<TextMesh>().text = questions[randomQuestion];
        }

        ReadFromServer();

        if (choiceSelected == "y")
        {
            choiceSelected = "n";
            //Debug.Log("Answer: " + answersID[randomQuestion] + " SelectedAnswer: " + selectedAnswer);
            //Debug.Log(answersID[randomQuestion].Equals(selectedAnswer));

            if (answersID[randomQuestion].Contains(selectedAnswer))
            {
                resultObj.GetComponent<TextMesh>().text = "Correct";
                resultObj.GetComponent<TextMesh>().color = Color.green;

                StartCoroutine(delayTime());
                SceneManager.LoadScene("mainScene");

                //Debug.Log("Answer: " + answersID[randomQuestion] + " SelectedAnswer: " + selectedAnswer);
                //Debug.Log("Correct");
            }
            else
            {
                resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
                resultObj.GetComponent<TextMesh>().color = Color.red;
                //Debug.Log("Answer: " + answersID[randomQuestion] + " SelectedAnswer: " + selectedAnswer);
                //Debug.Log("False");

                StartCoroutine(delayTime());
                SceneManager.LoadScene("mainScene");
            }
            
        }
    }
    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(5000);
    }

    void ReadFromServer()
    {
        for (int i = 0; i < listQuiz.Count; i++)
        {
            questions[i] = listQuiz[i].question;
            answersID[i] = listQuiz[i].answer;
        }
    }
    
    
}
