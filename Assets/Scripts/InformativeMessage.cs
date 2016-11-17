using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InformativeMessage : MonoBehaviour {

    public GameObject window;
    public Text notificationText;

    public static bool isQuizCompleted, isPuzzleCompleted, isMemoryCompleted, isSprintCompleted;
    string[] nav;

    //public static string s;

    public static bool run = false;

    private void Start()
    {
        //notificationText.text = "A minigame was completed by " + Manager.username;

        //window.GetComponent<Image>().enabled = false;

        InitializeComponents();

        Debug.Log("START START!!");
        StartCoroutine(GetCompletedMinigame());

        //ShowCompletedMinigame();
        //StartCoroutine(RemoveNotification());
    }

    private void Update()
    {
        //ShowCompletedMinigame();
        //if (run)
        //{
        //    print("START UPDATE");
        //    StartCoroutine(GetCompletedMinigame());
        //    run = false;
        //}
    }

    private void InitializeComponents()
    {
        //window.SetActive(true);
        //window.SetActive(false);

        //ShowCompletedMinigame();
        //StartCoroutine(RemoveNotification());
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
        string name = "milan";
        print("Beginning");

        //for (int i = 0; i < PlayerSpawner.fetchedList.Count; i++)
        //{
        //    print("FOR");
        //    if(PlayerSpawner.fetchedList[i].name == Manager.username)
        //    {
        //        print()
        //        if(PlayerSpawner.fetchedList[i].message != "")
        //        {
        //            window.SetActive(true);
        //            notificationText.text = Manager.username + " completed a Memory!";

        //            yield return new WaitForSeconds(10);
        //            window.SetActive(false);
        //        }
        //    }
        //}
        //window.SetActive(true);
        //window.SetActive(false);

        string loginUserURL = "http://gocommander.sytes.net/scripts/get_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", name);


        WWW www = new WWW(loginUserURL, form);

        yield return www;

        string result = www.text;

        if (result != null)
        {
            //nav = result.Split(';');
            print(result);
            window.SetActive(true);
            notificationText.text = result;

            yield return new WaitForSeconds(15);
            window.SetActive(false);
        }

        //for (int i = 0; i < nav.Length - 1; i++)
        //{
        //    int id = int.Parse(GetDataValue(nav[i], "ID:"));
        //    string username = GetDataValue(nav[i], "Username:");
        //    double lat = double.Parse(GetDataValue(nav[i], "Latitude:"));
        //    double lng = double.Parse(GetDataValue(nav[i], "Longitude:"));
        //    string message = GetDataValue(nav[i], "Minimessage:");

        //    if (message.Equals(""))
        //    {
        //        fetchedList.Add(new Player(id, username, lat, lng));
        //    }
        //    else
        //    {
        //        fetchedList.Add(new Player(id, username, lat, lng, message));
        //    }
        //}
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
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
