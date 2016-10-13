﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Manager : MonoBehaviour {

    private string[] questions, answers, answersID;
    

    public Transform resultObj;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1;

    string file = "Assets/Questions.txt", fileAnswers = "Assets/Answers.txt", fileAnswersID = "Assets/AnswersID.txt";

    private string[] lines, answerlines, answerIDLines;
    public static int randomLineNumber, nrOfQuestions;
    private string line, answerline;

    // Use this for initialization
    private void Start () {

        nrOfQuestions = 4;

        questions = new string[4];
        answers = new string[4];
        answersID = new string[4];

        ReadQuestions(file);
        ReadAnswers(fileAnswers);
        ReadAnswerID(fileAnswersID);

        Debug.Log("Choice Selected: " + choiceSelected);
    }

    // Update is called once per frame
    private void Update()
    {
        
        Debug.Log("Choice Selected: " + choiceSelected);

        if (randomQuestion == -1)
        {
            randomQuestion = Random.Range(0, 3);
        }
        if (randomQuestion > -1)
        {
            GetComponent<TextMesh>().text = questions[randomQuestion];

            //Debug.Log("RandomQuestionInt = " + randomQuestion);
            //Debug.Log("RandomLineNumberInt = " + randomLineNumber);
            //Debug.Log("NrOfQuestions = " + nrOfQuestions);
            //Debug.Log("Answer: " + answersID[randomQuestion] + " SelectedAnswer: " + selectedAnswer);
        }

        if (choiceSelected == "y")
        {
            choiceSelected = "n";

            if (answersID[randomQuestion] == selectedAnswer)
            {
                resultObj.GetComponent<TextMesh>().text = "Correct";
                resultObj.GetComponent<TextMesh>().color = Color.green;
            }
            else if (answersID[randomQuestion] != selectedAnswer)
            {
                resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
                resultObj.GetComponent<TextMesh>().color = Color.red;
            }
            
        }
    }

    void ReadQuestions(string _filePath)
    {
            lines = File.ReadAllLines(_filePath);

            //questions[0] = lines[0];
            //questions[1] = lines[1];
            //questions[2] = lines[2];
            //questions[3] = lines[3];

            for (int i = 0; i < lines.Length; i++)
            {
                questions[i] = lines[i];
            }
    }

    void ReadAnswers(string _filePath)
    {
        answerlines = File.ReadAllLines(_filePath);

        for (int i = 0; i < answerlines.Length; i++)
        {
            answers[i] = answerlines[i];
        }
    }

    void ReadAnswerID(string _filePath)
    {
        answerIDLines = File.ReadAllLines(_filePath);

        for (int i = 0; i < answerIDLines.Length; i++)
        {
            answersID[i] = answerIDLines[i];
        }
    }
    
}
