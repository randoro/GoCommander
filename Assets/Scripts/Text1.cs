using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text1 : MonoBehaviour {

    //List<string> firstChoice = new List<string>() { "3", "Yes", "1", "4" };

    string[] firstChoices;

    private string[] lines;
    public static int randomLineNumber;
    private string line;
    private string file = "Assets/firstOptions.txt";

    // Use this for initialization
    void Start()
    {
        firstChoices = new string[4];

        ReadFirstOptions(file);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.randomQuestion > -1)
        {
            GetComponent<TextMesh>().text = firstChoices[Manager.randomQuestion];
            Debug.Log("Option 1: " + GetComponent<TextMesh>().text);
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name; //Blir fel eftersom att objektets namn blir det rätta svaret, när det egentligen ska vara objektets textinnehåll som ska vara svaret.
        Manager.choiceSelected = "y";
    }

    private void ReadFirstOptions(string _filePath)
    {
        lines = File.ReadAllLines(_filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            firstChoices[i] = lines[i];
        }
    }
}
