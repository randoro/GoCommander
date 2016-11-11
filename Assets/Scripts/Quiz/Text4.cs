using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text4 : MonoBehaviour {
    
    private string[] fourthChoices;
    
    private List<string> lines = new List<string>();
    public static int randomLineNumber;
    private string line;
    private string file = "fourthOptions";

    public static TextMesh fourthOption;

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
            GetComponent<TextMesh>().text = fourthChoices[Manager.randomQuestion];
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
        Manager.choiceSelected = "y";
    }

    private void ReadFourthOptions()
    {
        for (int i = 0; i < Manager.allQuestionsList.Count; i++)
        {
            fourthChoices[i] = Manager.allQuestionsList[i].alt4;
        }
        
        //if(Manager.isInSkane)
        //{
        //    for (int i = 0; i < Manager.skaneListQuiz.Count; i++)
        //    {
        //        fourthChoices[i] = Manager.skaneListQuiz[i].alt4;
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < Manager.nationalListQuiz.Count; i++)
        //    {
        //        fourthChoices[i] = Manager.nationalListQuiz[i].alt4;
        //    }
        //}
    }

}
