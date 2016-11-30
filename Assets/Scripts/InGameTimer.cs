using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour {

    private static InGameTimer inGameTimer;

    public static float timeLeft;
    public Text timeLeftText;
    private Text testText;

	// Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        if(inGameTimer == null)
        {
            DontDestroyOnLoad(gameObject);
            inGameTimer = this;
        }
        else if(inGameTimer != this)
        {
            Destroy(gameObject);
        }

        timeLeft = 1200.0f;
    }

	// Update is called once per frame
	void Update () {

        timeLeft -= Time.fixedDeltaTime;
        print(timeLeft);

        //timeLeftText.text = ((int)timeLeft/60).ToString() + " minutes";

        if(timeLeft <= 0)
        {
            print("GAME IS OVER!!!!");

            timeLeft = 1200.0f;

            LeaveTeam();

            SceneManager.LoadScene("LobbyScene");
        }

	}

    void OnLevelWasLoaded()
    {
        timeLeftText = GameObject.Find("Time_text").GetComponent<Text>();
        if(testText != null)
        {
            timeLeftText = testText;
            timeLeftText.text = timeLeft.ToString();
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
