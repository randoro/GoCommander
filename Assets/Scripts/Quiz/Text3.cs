using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text3 : MonoBehaviour {
    
    private string[] thirdChoices;

    private List<string> lines = new List<string>();
    public static int randomLineNumber;
    private string line;
    private string file = "thirdOptions";

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
        TextAsset level_file = Resources.Load(_filePath) as TextAsset;

        string[] linesInFile = level_file.text.Split('\n');


        for (int i = 0; i < linesInFile.Length; i++)
        {
            lines.Add(linesInFile[i]);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            thirdChoices[i] = lines[i];
        }
    }
}
