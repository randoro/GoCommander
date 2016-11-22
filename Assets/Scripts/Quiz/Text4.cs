using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class Text4 : MonoBehaviour {
    
    private string[] fourthChoices;
    
    private List<string> lines = new List<string>();
    public static int randomLineNumber;
    private string line;
    private string file = "fourthOptions";

    public static TextMesh fourthOption;

    public Button fourthOptionBtn;

    // Use this for initialization
    void Start()
    {
        fourthChoices = new string[4];
    }

    // Update is called once per frame
    void Update()
    {
        ReadFourthOptions();

        if (Manager.randomQuestion > -1)
        {
            fourthOptionBtn.GetComponentInChildren<Text>().text = fourthChoices[Manager.randomQuestion];
        }
    }

    public void OnMouseDown()
    {
        Manager.selectedAnswer = fourthOptionBtn.name;
        Manager.choiceSelected = "y";
    }

    private void ReadFourthOptions()
    {
        for (int i = 0; i < Manager.allQuestionsList.Count; i++)
        {
            fourthChoices[i] = Manager.allQuestionsList[i].alt4;
        }
    }

}
