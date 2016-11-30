using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float timeLeft = 1200.0f;

    public Text timeLeftText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        timeLeft -= Time.fixedDeltaTime;
        print(timeLeft);

        timeLeftText.text = ((int)timeLeft/60).ToString();

        if(timeLeft <= 0)
        {
            print("GAME IS OVER!!!!");

            timeLeft = 1200.0f;

            LeaveTeam();

            SceneManager.LoadScene("LobbyScene");
        }

	}

    IEnumerator LeaveTeam()
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/leave_group.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;
    }
}
