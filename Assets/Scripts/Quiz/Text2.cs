using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text2 : MonoBehaviour {

    private string[] secondChoices;

    private List<string> lines = new List<string>();
    private string line;
    private string file = "secondOptions";

    public static TextMesh secondOption;

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
            secondOption = GetComponent<TextMesh>();
            secondOption.text = secondChoices[Manager.randomQuestion];
            //Debug.Log("Option 2: " + GetComponent<TextMesh>().text);
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
        Manager.choiceSelected = "y";
    }

    void ReadSecondOptions(string _filePath)
    {
        TextAsset level_file = Resources.Load(_filePath) as TextAsset;

        string[] linesInFile = level_file.text.Split('\n');


        for (int i = 0; i < linesInFile.Length; i++)
        {
            lines.Add(linesInFile[i]);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            secondChoices[i] = lines[i];

            //Debug.Log(secondChoices[0]);
        }
    }
}
