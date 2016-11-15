using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InformativeMessage : MonoBehaviour {

    public GameObject window;
    public Text notificationText;

    public static bool isQuizCompleted, isPuzzleCompleted, isMemoryCompleted, isSprintCompleted;

    private void Start()
    {
        //notificationText.text = "A minigame was completed by " + Manager.username;

        InitializeComponents();

        ShowCompletedMinigame();
        StartCoroutine(RemoveNotification());
    }

    private void Update()
    {
        ShowCompletedMinigame();
    }

    private void InitializeComponents()
    {
        window.SetActive(false);
        ShowCompletedMinigame();
        StartCoroutine(RemoveNotification());
    }

    public void ShowCompletedMinigame()
    {
        if(isQuizCompleted)
        {
            window.SetActive(true);
            notificationText.text = Manager.username + " completed a Quiz!";
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
