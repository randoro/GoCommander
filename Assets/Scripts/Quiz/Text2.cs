using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Text2 : MonoBehaviour {

    private string[] secondChoices;

    private List<string> lines = new List<string>();
    private string line;
    private string file = "secondOptions";

    public static TextMesh secondOption;

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
            GetComponent<TextMesh>().text = secondChoices[Manager.randomQuestion];
        }
    }

    void OnMouseDown()
    {
        Manager.selectedAnswer = gameObject.name;
        Manager.choiceSelected = "y";
    }

    void ReadSecondOptions()
    {
        if(Manager.isInSkane)
        {
            for (int i = 0; i < Manager.skaneListQuiz.Count; i++)
            {
                secondChoices[i] = Manager.skaneListQuiz[i].alt2;
            }
        }
        else
        {
            for (int i = 0; i < Manager.nationalListQuiz.Count; i++)
            {
                secondChoices[i] = Manager.nationalListQuiz[i].alt2;
            }
        }
    }
}
