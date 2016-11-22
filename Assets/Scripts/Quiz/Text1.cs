using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class Text1 : MonoBehaviour {
    
    string[] firstChoices;

    private List<string> lines = new List<string>();
    public static int randomLineNumber;
    private string line;
    private string file = "firstOptions";

    public static TextMesh firstOption;

    public Button firstOptionBtn;
    
    

    // Use this for initialization
    void Start()
    {
        firstChoices = new string[4];
    }

    // Update is called once per frame
    void Update()
    {
        ReadFirstOptions();

        if (Manager.randomQuestion > -1)
        {
            firstOptionBtn.GetComponentInChildren<Text>().text = firstChoices[Manager.randomQuestion];
        }
    }

    public void OnMouseDown()
    {
        Manager.selectedAnswer = firstOptionBtn.name;

        Manager.choiceSelected = "y";
    }

    private void ReadFirstOptions()
    {
        for (int i = 0; i < Manager.allQuestionsList.Count; i++)
        {
            firstChoices[i] = Manager.allQuestionsList[i].alt1;
        }
    }
}
