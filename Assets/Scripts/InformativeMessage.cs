using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InformativeMessage : MonoBehaviour {

    public static GameObject window;
    public static Text notificationText;

    public static bool isQuizCompleted, isPuzzleCompleted, isMemoryCompleted, isSprintCompleted;

    public static string s;

    private void Start()
    {
        //notificationText.text = "A minigame was completed by " + Manager.username;

        InitializeComponents();

        

        //ShowCompletedMinigame();
        //StartCoroutine(RemoveNotification());
    }

    private void Update()
    {
        //ShowCompletedMinigame();

        StartCoroutine(GetCompletedMinigame());
    }

    private void InitializeComponents()
    {
        window.SetActive(false);
        ShowCompletedMinigame();
        StartCoroutine(RemoveNotification());
    }

    public static void ShowCompletedMinigame()
    {
        if(isQuizCompleted)
        {
            window.SetActive(true);
            notificationText.text = Manager.username + " completed a Quiz!";

            s = Manager.username + " completed a Quiz!";

            //StartCoroutine(SendCompletedMinigame(s));
        }
        else if(isMemoryCompleted)
        {
            window.SetActive(true);
            notificationText.text = Manager.username + " completed a Memory!";
        }
        else if(isPuzzleCompleted)
        {
            window.SetActive(true);
            notificationText.text = Manager.username + " completed a Puzzle!";
        }
        else if(isSprintCompleted)
        {
            window.SetActive(true);
            notificationText.text = Manager.username + " completed a Sprint!";
        }
    }

    IEnumerator GetCompletedMinigame()
    {
        for (int i = 0; i < PlayerSpawner.fetchedList.Count; i++)
        {
            if(PlayerSpawner.fetchedList[i].name == Manager.username)
            {
                if(PlayerSpawner.fetchedList[i].message != "")
                {
                    window.SetActive(true);
                    notificationText.text = Manager.username + " completed a Memory!";

                    yield return new WaitForSeconds(10);
                    window.SetActive(false);
                }
            }
        }

        //string loginUserURL = "http://gocommander.sytes.net/scripts/login.php";

        //WWWForm form = new WWWForm();
        //form.AddField("usernamePost", name);


        //WWW www = new WWW(loginUserURL, form);

        //yield return www;
    }

    IEnumerator RemoveNotification()
    {
        if (isQuizCompleted)
        {
            yield return new WaitForSeconds(10);
            window.SetActive(false);
            isQuizCompleted = false;
        }
        else if (isMemoryCompleted)
        {
            yield return new WaitForSeconds(10);
            window.SetActive(false);
            isMemoryCompleted = false;
        }
        else if (isPuzzleCompleted)
        {
            yield return new WaitForSeconds(10);
            window.SetActive(false);
            isPuzzleCompleted = false;
        }
        else if (isSprintCompleted)
        {
            yield return new WaitForSeconds(10);
            window.SetActive(false);
            isSprintCompleted = false;
        }
    }
}
