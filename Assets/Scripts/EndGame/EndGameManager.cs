using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour {

    public Text personalScoreText;
    public Text groupScoreText;

	// Use this for initialization
	void Start () {

        SetFinalScore();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    private void SetFinalScore()
    {
        personalScoreText.text = Highscoremanger.totalscore.ToString();
        groupScoreText.text = GoogleMap.groupScore.ToString();
    }
}
