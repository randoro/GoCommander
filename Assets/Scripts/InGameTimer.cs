﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour {
    
    public static float timeLeft = 1200;
    public Text timeLeftText;
    private Text testText;

	// Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        timeLeft = PlayerPrefs.GetFloat("Player Time");
    }

	// Update is called once per frame
	void Update () {

        timeLeft -= Time.fixedDeltaTime;
        print(timeLeft);

        timeLeftText.text = ((int)timeLeft/60).ToString() + " minutes";

        if (timeLeft <= 0)
        {
            print("GAME IS OVER!!!!");

            LeaveTeam();

            timeLeft = 1200.0f;

            SceneManager.LoadScene("LobbyScene");
        }

	}

    void OnDestroy()
    {
        PlayerPrefs.SetFloat("Player Time", timeLeft);
    }

    void OnApplicationQuit()
    {
        timeLeft = 1200.0f;
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
