using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour {

    public GameObject username;
    public GameObject password;
    public GameObject result;
    public GameObject timer;

    private string Username;
    private string Password;
    private Text Result;
    private Text Timer;
    private float timerUp; 

    // Use this for initialization
    void Start () {
        Result = result.GetComponent<Text>();
        Timer = timer.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;        
    }

    public void RegisterButton()
    {
        if (Username != "" && Password != "")
        {
            StartTimer();
            StartCoroutine(CreateUser(Username, Password));
            StopTimer();
        }
    }

    IEnumerator CreateUser(string name, string pass)
    {
        string createUserURL = "http://gocommander.sytes.net/scripts/register.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", name);
        form.AddField("passwordPost", pass);

        WWW www = new WWW(createUserURL, form);

        yield return www;

        string result = www.text;

        if(result == "User Added")
        {
            Result.text = "You are Added!";
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            username.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Username";
            password.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Password";
        }
        else
        {
            Result.text = "Username already Exists";
        }
    }

    private void StartTimer()
    {
        print("Timer Start!");
        timerUp = 0.0f;
        timerUp += Time.deltaTime;
        Timer.text = "Timer: " + timerUp.ToString("F3");
    }

    private void StopTimer()
    {
        print("Timer Stop!");
        float finalTime = timerUp;
        Timer.text = "Timer: " + finalTime.ToString("F3");
    }
}
