using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class Text3 : MonoBehaviour {
    
    private string[] thirdChoices;

    private List<string> lines = new List<string>();
    public static int randomLineNumber;
    private string line;
    private string file = "thirdOptions";

    public static TextMesh thirdOption;

    public Button thirdOptionBtn;
    //public Text thirdOptionText;

    // Use this for initialization
    void Start()
    {
        thirdChoices = new string[4];
    }

    // Update is called once per frame
    void Update()
    {
        ReadThirdOptions();

        if (Manager.randomQuestion > -1)
        {
            //GetComponent<TextMesh>().text = thirdChoices[Manager.randomQuestion];

            thirdOptionBtn.GetComponentInChildren<Text>().text = thirdChoices[Manager.randomQuestion];
        }
    }

    public void OnMouseDown()
    {
        Manager.selectedAnswer = thirdOptionBtn.name;
        Manager.choiceSelected = "y";
    }

    private void ReadThirdOptions()
    {
        for (int i = 0; i < Manager.allQuestionsList.Count; i++)
        {
            thirdChoices[i] = Manager.allQuestionsList[i].alt3;
        }
    }
}
