using UnityEngine;
using System.Collections;

public class HighscoreList {

    public int id;
    public string name;
    public string game;
    public int score;

    public HighscoreList(int id, string name, string game, int score)
    {
        this.id = id;
        this.name = name;
        this.game = game;
        this.score = score;
    }
}
