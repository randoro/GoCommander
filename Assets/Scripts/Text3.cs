using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text3 : MonoBehaviour {
    
    private string[] thirdChoices;

    private string[] lines;
    public static int randomLineNumber;
    private string line;
    private string file = "Assets/TextFiles/thirdOptions.txt";

    // Use this for initialization
    void Start()
    {
        thirdChoices = new string[4];

        ReadThirdOptions(file);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.randomQuestion > -1)
        {
            GetComponent<TextMesh>().text = thirdChoices[Manager.randomQuestion];
            //Debug.Log("Option 3: " + GetComponent<TextMesh>().text);
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
        Manager.choiceSelected = "y";
    }

    private void ReadThirdOptions(string _filePath)
    {
        lines = File.ReadAllLines(_filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            thirdChoices[i] = lines[i];
        }
    }
}
