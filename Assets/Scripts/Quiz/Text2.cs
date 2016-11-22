using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class Text2 : MonoBehaviour {

    private string[] secondChoices;

    private List<string> lines = new List<string>();
    private string line;
    private string file = "secondOptions";

    public static TextMesh secondOption;

    public Button secondOptionBtn;

    // Use this for initialization
    void Start()
    {
        secondChoices = new string[4];
    }

    // Update is called once per frame
    void Update()
    {
        ReadSecondOptions();

        if (Manager.randomQuestion > -1)
        {
            secondOptionBtn.GetComponentInChildren<Text>().text = secondChoices[Manager.randomQuestion];
        }
    }

    public void OnMouseDown()
    {
        Manager.selectedAnswer = secondOptionBtn.name;
        Manager.choiceSelected = "y";
    }

    void ReadSecondOptions()
    {
        for (int i = 0; i < Manager.allQuestionsList.Count; i++)
        {
            secondChoices[i] = Manager.allQuestionsList[i].alt2;
        }
    }
}
