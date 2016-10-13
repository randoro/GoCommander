using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text2 : MonoBehaviour {

    //List<string> secondChoice = new List<string>() { "8", "No", "2", "Fourth" };

    string[] secondChoices;

    private string[] lines;
    public static int randomLineNumber;
    private string line;
    private string file = "Assets/secondOptions.txt";

    // Use this for initialization
    void Start()
    {
        secondChoices = new string[4];

        ReadSecondOptions(file);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.randomQuestion > -1)
        {
            GetComponent<TextMesh>().text = secondChoices[Manager.randomQuestion];
            Debug.Log("Option 2: " + GetComponent<TextMesh>().text);
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
        Manager.choiceSelected = "y";
    }

    void ReadSecondOptions(string _filePath)
    {
        lines = File.ReadAllLines(_filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            secondChoices[i] = lines[i];
        }
    }
}
