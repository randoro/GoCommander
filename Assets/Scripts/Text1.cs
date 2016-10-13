﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text1 : MonoBehaviour {
    
    string[] firstChoices;

    private string[] lines;
    public static int randomLineNumber;
    private string line;
    private string file = "Assets/TextFiles/firstOptions.txt";

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
            //Debug.Log("Option 1: " + GetComponent<TextMesh>().text);
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
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
