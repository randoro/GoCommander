using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text4 : MonoBehaviour {
    
    private string[] fourthChoices;
    
    private string[] lines;
    public static int randomLineNumber;
    private string line;
    private string file = "Assets/TextFiles/fourthOptions.txt";

    // Use this for initialization
    void Start()
    {
        fourthChoices = new string[4];

        ReadFourthOptions(file);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.randomQuestion > -1)
        {
            GetComponent<TextMesh>().text = fourthChoices[Manager.randomQuestion];
            //Debug.Log("Option 4: " + GetComponent<TextMesh>().text);
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
        Manager.choiceSelected = "y";
    }

    private void ReadFourthOptions(string _filePath)
    {
        lines = File.ReadAllLines(_filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            fourthChoices[i] = lines[i];
        }
    }

}
