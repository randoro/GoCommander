using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Manager : MonoBehaviour {

    private string[] questions, answers, answersID;

    public Transform resultObj;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1;

    string file = "Questions", fileAnswers = "Answers", fileAnswersID = "AnswersID";

    private List<string> lines, answerlines, answerIDLines;
    private string line, answerline;

    // Use this for initialization
    private void Start () {

        lines = new List<string>();
        answerlines = new List<string>();
        answerIDLines = new List<string>();

        
        questions = new string[4];
        answers = new string[4];
        answersID = new string[4];

        ReadQuestions(file);
        ReadAnswers(fileAnswers);
        ReadAnswerID(fileAnswersID);

        //Debug.Log("Choice Selected: " + choiceSelected);
        
    }

    // Update is called once per frame
    private void Update()
    {
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

        //Debug.Log(selectedAnswer);
        //Debug.Log(choiceSelected);
        //Debug.Log(questions[0]);
        //Debug.Log(answersID[0]);

        if (choiceSelected == "y")
        {
            choiceSelected = "n";
            //Debug.Log("Answer: " + answersID[randomQuestion] + " SelectedAnswer: " + selectedAnswer);

            //Debug.Log(answersID[randomQuestion].Equals(selectedAnswer));

            if (answersID[randomQuestion].Contains(selectedAnswer))
            {
                //Debug.Log("Answer: " + answersID[randomQuestion] + " SelectedAnswer: " + selectedAnswer);
                resultObj.GetComponent<TextMesh>().text = "Correct";
                resultObj.GetComponent<TextMesh>().color = Color.green;

                //Debug.Log("Correct");
            }
            else
            {
                resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
                resultObj.GetComponent<TextMesh>().color = Color.red;
                //Debug.Log("Answer: " + answersID[randomQuestion] + " SelectedAnswer: " + selectedAnswer);
                //Debug.Log("False");
            }
            
        }
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
