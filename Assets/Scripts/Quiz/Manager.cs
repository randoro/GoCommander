﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private string[] quiz;
    private List<QuizList> listQuiz = new List<QuizList>();

    private string[] questions, answers, answersID;

    public Transform resultObj;

    int score = 0;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1;

    string file = "Questions", fileAnswers = "Answers", fileAnswersID = "AnswersID";

    private List<string> lines, answerlines, answerIDLines;
    private string line, answerline;

    private TextMesh[] options;

    // Use this for initialization
    private void Start () {

        StartCoroutine(GetQuizes());

        lines = new List<string>();
        answerlines = new List<string>();
        answerIDLines = new List<string>();

        
        questions = new string[4];
        answers = new string[4];
        answersID = new string[4];

        ReadQuestions(file);
        ReadAnswers(fileAnswers);
        ReadAnswerID(fileAnswersID);

        options[0] = Text1.firstOption;
        options[1] = Text2.secondOption;
        options[2] = Text3.thirdOption;
        options[3] = Text4.fourthOption;

        //Debug.Log("Choice Selected: " + choiceSelected);

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
            string wrong1 = GetDataValue(quiz[i], "Wrong_1:");
            string wrong2 = GetDataValue(quiz[i], "Wrong_2:");
            string wrong3 = GetDataValue(quiz[i], "Wrong_3:");
            string correct = GetDataValue(quiz[i], "Correct:");

            print(city + " " + wrong1 + " " + wrong2 + " " + wrong3 + " " + correct);

            listQuiz.Add(new QuizList(city, wrong1, wrong2, wrong3, correct));
        }
        print(listQuiz.Count);
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

    void ReadQuestions(string _filePath)
    {
        TextAsset level_file = Resources.Load(_filePath) as TextAsset;
        //Debug.Log(level_file);

        string[] linesInFile = level_file.text.Split('\n');
        

        for (int i = 0; i < linesInFile.Length; i++)
        {
            lines.Add(linesInFile[i]);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            questions[i] = lines[i];
        }
    }

    void ReadAnswers(string _filePath)
    {
        TextAsset level_file = Resources.Load(_filePath) as TextAsset;

        string[] linesInFile = level_file.text.Split('\n');

        for (int i = 0; i < linesInFile.Length; i++)
        {
            answerlines.Add(linesInFile[i]);
        }

        for (int i = 0; i < answerlines.Count; i++)
        {
            answers[i] = answerlines[i];
        }
    }

    void ReadAnswerID(string _filePath)
    {
        TextAsset level_file = Resources.Load(_filePath) as TextAsset;
        string[] linesInFile = level_file.text.Split('\n');

        for (int i = 0; i < linesInFile.Length; i++)
        {
            answerIDLines.Add(linesInFile[i]);
        }

        for (int i = 0; i < answerIDLines.Count; i++)
        {
            answersID[i] = answerIDLines[i];
        }
    }
    
    
}
