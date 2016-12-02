using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public GameObject result;
    public GameObject timer;

    private string Username;
    private string Password;
    private Text Result;
    private Text Timer;
    private float timerUp;
  

    public void LoginButton()
    {
        if (Username != "" && Password != "")
        {
            StartCoroutine(LoginUser(Username, Password));
        }
    }

    // Use this for initialization
    void Start()
    {
        Result = result.GetComponent<Text>();
        Timer = timer.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

      Username = username.GetComponent<InputField>().text;
      Password = password.GetComponent<InputField>().text;
    }

    IEnumerator LoginUser(string name, string pass)
    {
        string loginUserURL = "http://gocommander.sytes.net/scripts/login.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", name);
        form.AddField("passwordPost", pass);

        WWW www = new WWW(loginUserURL, form);

        yield return www;

        string result = www.text;

        if (result == "User not found")
        {
            Result.text = "User not Found!";
        }
        else if (result == "Wrong password")
        {
            Result.text = "Wrong Password!";
        }
        else
        {
            GoogleMap.id = Int32.Parse(result);
            GoogleMap.username = Username;
            Result.text = "You are Logged in!";
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            username.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Username";
            password.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Password";
            SceneManager.LoadScene("LobbyScene");
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
