using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text1 : MonoBehaviour {
    
    string[] firstChoices;

    private List<string> lines = new List<string>();
    public static int randomLineNumber;
    private string line;
    private string file = "firstOptions";

    public static TextMesh firstOption;
    
    

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
            GetComponent<TextMesh>().text = firstChoices[Manager.randomQuestion];
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
        Manager.choiceSelected = "y";
    }

    private void ReadFirstOptions()
    {
        for (int i = 0; i < Manager.allQuestionsList.Count; i++)
        {
            firstChoices[i] = Manager.allQuestionsList[i].alt1;
        }
        //if (Manager.isInSkane)
        //{
        //    for (int i = 0; i < Manager.skaneListQuiz.Count; i++)
        //    {
        //        firstChoices[i] = Manager.skaneListQuiz[i].alt1;
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < Manager.nationalListQuiz.Count; i++)
        //    {
        //        firstChoices[i] = Manager.nationalListQuiz[i].alt1;
        //    }
        //}
    }
}
