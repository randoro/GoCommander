using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QuizList
{
    public string position;
    public string question;
    public string alt1;
    public string alt2;
    public string alt3;
    public string alt4;
    public string answer;
    public string image;

    public QuizList(string position, string question, string alt1, string alt2, string alt3, string alt4, string answer, string image)
    {
        this.position = position;
        this.question = question;
        this.alt1 = alt1;
        this.alt2 = alt2;
        this.alt3 = alt3;
        this.alt4 = alt4;
        this.answer = answer;
        this.image = image;
    }
}
