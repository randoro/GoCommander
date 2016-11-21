using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InformativeMessage : MonoBehaviour {

    public GameObject notificationWindow;
    public Text notificationText, ButtonTextOne, ButtonTextTwo, ButtonTextThree, ButtonTextFour;

    public static bool isQuizCompleted, isPuzzleCompleted, isMemoryCompleted, isSprintCompleted;
    string[] nav;

    private string[] messages;

    string minimessage;

    private List<MessageList> ListOfMessages = new List<MessageList>();

    //public static string s;

    public static bool run = false;
    float timer = 5f;
    float messagetimer = 10f;

    private void Start()
    {
        //notificationText.text = "A minigame was completed by " + Manager.username;

        //window.GetComponent<Image>().enabled = false;

        InitializeComponents();

        Debug.Log("START START!!");
        //StartCoroutine(GetCompletedMinigame());

        messages = new string[4];

        messages[0] = "Great work!";
        messages[1] = "Thanks";
        messages[2] = "MessageToChoose3";
        messages[3] = "MessageToChoose4";

        SetButtonText();

        //ShowCompletedMinigame();
        //StartCoroutine(RemoveNotification());
    }

    private void Update()
    {
        messagetimer -= Time.deltaTime;

        if(messagetimer < 0)
        {
            StartCoroutine(GetCompletedMinigame());
            //messagetimer = 15f;
        }

        if (isQuizCompleted || isMemoryCompleted || isPuzzleCompleted || isSprintCompleted)
        {
            timer -= Time.deltaTime;
            //StartCoroutine(GetCompletedMinigame());
            notificationWindow.SetActive(true);
            notificationText.text = GoogleMap.username + " " + minimessage;
            
            StartCoroutine(RemoveNotification());
            if (timer < 0)
            {
                StartCoroutine(DeleteMinimessage());               
            }
        }
        //    else if (isMemoryCompleted)
        //    {
        //        window.SetActive(true);
        //        notificationText.text = Manager.username + " completed a Memory!";
        //    }
        //    else if (isPuzzleCompleted)
        //    {
        //        window.SetActive(true);
        //        notificationText.text = Manager.username + " completed a Puzzle!";
        //    }
        //    else if (isSprintCompleted)
        //    {
        //        window.SetActive(true);
        //        notificationText.text = Manager.username + " completed a Sprint!";
        //    }
    }

    private void InitializeComponents()
    {
        notificationWindow.SetActive(false);
    }

    //IEnumerator GetMessages()
    //{
    //    string messagesURL = "";

    //    WWWForm form = new WWWForm();
    //    //form.AddField("", );

    //    WWW www = new WWW(messagesURL, form);

    //    yield return www;

    //    string result = www.text;

    //    if(result != null)
    //    {
    //        messages = result.Split(';');

    //    }

    //    for (int i = 0; i < messages.Length - 1; i++)
    //    {
    //        string message = GetMessageValue(messages[i], "Message:");

    //        ListOfMessages.Add(new MessageList(message));
    //    }


    //} 

    //string GetMessageValue(string data, string index)
    //{
    //    string value = data.Substring(data.IndexOf(index) + index.Length);
    //    if (value.Contains("|"))
    //        value = value.Remove(value.IndexOf("|"));
    //    return value;
    //}

    public void SetButtonText()
    {
        ButtonTextOne.text = messages[0];
        ButtonTextTwo.text = messages[1];
        ButtonTextThree.text = messages[2];
        ButtonTextFour.text = messages[3];
    }

    //public static void ShowCompletedMinigame()
    //{
    //    if (isQuizCompleted)
    //    {
    //        window.SetActive(true);
    //        notificationText.text = Manager.username + " completed a Quiz!";

    //        s = Manager.username + " completed a Quiz!";

    //        StartCoroutine(SendCompletedMinigame(s));
    //    }
    //    else if (isMemoryCompleted)
    //    {
    //        window.SetActive(true);
    //        notificationText.text = Manager.username + " completed a Memory!";
    //    }
    //    else if (isPuzzleCompleted)
    //    {
    //        window.SetActive(true);
    //        notificationText.text = Manager.username + " completed a Puzzle!";
    //    }
    //    else if (isSprintCompleted)
    //    {
    //        window.SetActive(true);
    //        notificationText.text = Manager.username + " completed a Sprint!";
    //    }
    //}

    IEnumerator GetCompletedMinigame()
    {
        //string name = "milan";
        print("Beginning");

        string loginUserURL = "http://gocommander.sytes.net/scripts/get_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", Manager.username);

        WWW www = new WWW(loginUserURL, form);

        yield return www;

        string result = www.text;

        if (result.Equals(""))
        {
            messagetimer = 10f;            
        }
        else
        {
            minimessage = www.text;
            isQuizCompleted = true;
        }
    }

    IEnumerator DeleteMinimessage()
    {
        //string name = "milan";
        print("Beginning");

        string loginUserURL = "http://gocommander.sytes.net/scripts/delete_minigamemessage.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);

        WWW www = new WWW(loginUserURL, form);

        yield return www;
        timer = 5f;
        isQuizCompleted = false;
    }

    IEnumerator RemoveNotification()
    {
        if (isQuizCompleted)
        {
            yield return new WaitForSeconds(10);
            notificationWindow.SetActive(false);
            //isQuizCompleted = false;
        }
        else if (isMemoryCompleted)
        {
            yield return new WaitForSeconds(10);
            notificationWindow.SetActive(false);
            isMemoryCompleted = false;
        }
        else if (isPuzzleCompleted)
        {
            yield return new WaitForSeconds(10);
            notificationWindow.SetActive(false);
            isPuzzleCompleted = false;
        }
        else if (isSprintCompleted)
        {
            yield return new WaitForSeconds(10);
            notificationWindow.SetActive(false);
            isSprintCompleted = false;
        }
    }
}
