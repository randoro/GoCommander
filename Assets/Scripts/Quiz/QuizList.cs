using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QuizList
{

    public string city;
    public string wrong1;
    public string wrong2;
    public string wrong3;
    public string correct;

    public QuizList(string city, string wrong1, string wrong2, string wrong3, string correct)
    {
        this.city = city;
        this.wrong1 = wrong1;
        this.wrong2 = wrong2;
        this.wrong3 = wrong3;
        this.correct = correct;
    }
}
