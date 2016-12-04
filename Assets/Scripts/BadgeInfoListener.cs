using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeInfoListener : MonoBehaviour {
    public int refreshDelay = 5;
    public NextCommanderController nextCommanderController;
    public GameObject commanderBadgeButton;
    Vector3 originalScale;
    string[] polls;
    public IEnumerator listener;
	// Use this for initialization
	public void Start () {
        //listener = Listen();
        //StartCoroutine(listener);
        //originalScale = commanderBadgeButton.transform.localScale;
        //commanderBadgeButton.transform.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
    void Update()
    {
        if (GoogleMap.completedMinigames == 3)
        {
            GoogleMap.completedMinigames = 0;
            commanderBadgeButton.GetComponent<BadgeController>().enabled = true;
            enabled = false;
        }
    }
    //public IEnumerator Listen() {
    //    while (true)
    //    {
    //        StartCoroutine(CheckPending());
    //        yield return new WaitForSeconds(refreshDelay);
    //    }
    //}
    IEnumerator CheckPending()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_check.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(votersURL, form);
        yield return www;
        string result = www.text;
        //print(result);

        //if (result.Contains("PENDING"))
        //{
        //    //if (GoogleMap.lastCommander)
        //    //{
        //    //    GoogleMap.lastCommander = false;
        //    //    StartCoroutine(nextCommanderController.startVoting);
        //    //    commanderBadgeButton.transform.localScale = originalScale;
        //    //    commanderBadgeButton.GetComponent<BadgeController>().enabled = true;
        //    //    StopCoroutine(listener);
        //    //    StopCoroutine(CheckPending());
        //    //}
        //    //else
        //    //{
        //    //    //commanderBadgeButton.SetActive(true);
        //    //    commanderBadgeButton.GetComponent<BadgeController>().enabled = true;
        //    //    commanderBadgeButton.transform.localScale = originalScale;
        //    //    StopCoroutine(listener);
        //    //    StopCoroutine(CheckPending());
        //    //}
        //    commanderBadgeButton.GetComponent<BadgeController>().enabled = true;
        //    //commanderBadgeButton.transform.localScale = originalScale;
        //    StopCoroutine(listener);
        //    StopCoroutine(CheckPending());
        //}
    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}