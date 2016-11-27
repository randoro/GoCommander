using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InformativeMessage : MonoBehaviour {

    public GameObject notificationWindow;
    public Text notificationText;
    
    string[] nav;

    private string[] messages;

    string minimessage;
    string gamemessage;

    private List<MessageList> ListOfMessages = new List<MessageList>();

    public static bool finished;

    private void Start()
    {
        finished = false;

        InitializeComponents();

        InvokeRepeating("StartMethod", 1.0f, 10f);
        InvokeRepeating("StartGetGameMessage", 2.0f, 10f);
    }

    void StartGetGameMessage()
    {
        print(GoogleMap.username);
        print(GoogleMap.groupName);
        StartCoroutine(TimerPauseGameMessage());
    }

    void StartMethod()
    {
        
        print("START REVOKE");
        print(GoogleMap.username);
        print(GoogleMap.groupName);
        StartCoroutine(TimerPause());
    }

    private void Update()
    {

    }

    IEnumerator TimerPause()
    {      
        StartCoroutine(GetCompletedMinigame());

        if (!minimessage.Equals(""))
        {
            notificationText.text = minimessage;
            notificationWindow.SetActive(true);

            StartCoroutine(DeleteMinimessage());
        }

        yield return new WaitForSeconds(5);
    }

    IEnumerator TimerPauseGameMessage()
    {
        StartCoroutine(GetGameMessage());

        if (!gamemessage.Equals(""))
        {
            notificationText.text = gamemessage;
            notificationWindow.SetActive(true);

            StartCoroutine(DeleteGameMessage());
        }

        yield return new WaitForSeconds(5);
    }

    private void InitializeComponents()
    {
        notificationWindow.SetActive(false);
    }

    IEnumerator GetCompletedMinigame()
    {
        string loginUserURL = "http://gocommander.sytes.net/scripts/get_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);

        WWW www = new WWW(loginUserURL, form);

        yield return www;

        string result = www.text;

        if (result.Equals(""))
        {
            print("NULL");
            notificationText.text = "";
            notificationWindow.SetActive(false);           
        }
        else
        {
            minimessage = result;
            print(minimessage);
        }
    }

    IEnumerator GetGameMessage()
    {
        string gameMessageURL = "http://gocommander.sytes.net/scripts/get_game_message.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);

        WWW www = new WWW(gameMessageURL, form);

        yield return www;

        string result = www.text;

        if (result.Equals(""))
        {
            print("NULL");
            notificationText.text = "";
            notificationWindow.SetActive(false);
        }
        else
        {
            gamemessage = result;
            print(gamemessage);
            print(result);
        }
    }

    IEnumerator DeleteMinimessage()
    {
        print("DELETE MESSAGE");
        minimessage = "";

        string loginUserURL = "http://gocommander.sytes.net/scripts/delete_minigamemessage.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);

        WWW www = new WWW(loginUserURL, form);

        yield return www;
    }

    IEnumerator DeleteGameMessage()
    {
        print("DELETE MESSAGE");
        gamemessage = "";

        string deleteMessageURL = "http://gocommander.sytes.net/scripts/delete_game_message.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);

        WWW www = new WWW(deleteMessageURL, form);

        yield return www;
    }
}
