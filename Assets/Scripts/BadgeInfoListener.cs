using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeInfoListener : MonoBehaviour {
    public int refreshDelay = 5;
    public NextCommanderController nextCommanderController;
    public GameObject commanderBadgeButton;
    string[] polls;
    public IEnumerator listener;
	// Use this for initialization
	public void Start () {
        listener = Listen();
        StartCoroutine(listener);
	}
	
	// Update is called once per frame
	public IEnumerator Listen() {
        while (true)
        {
            StartCoroutine(CheckPending());
            yield return new WaitForSeconds(refreshDelay);
        }
	}
    IEnumerator CheckPending()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_check.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(votersURL, form);
        yield return www;
        string result = www.text;
        print(result);

        result = "PENDING";

        if (result.Contains("PENDING"))
        {
            if (GoogleMap.lastCommander)
            {
                GoogleMap.lastCommander = false;
                StartCoroutine(nextCommanderController.startVoting);
                //commanderBadgeButton.SetActive(true);
                commanderBadgeButton.GetComponent<BadgeController>().enabled = true;
                commanderBadgeButton.GetComponent<Button>().interactable = false;
                StopCoroutine(listener);
                StopCoroutine(CheckPending());
            }
            else
            {
                //commanderBadgeButton.SetActive(true);
                commanderBadgeButton.GetComponent<BadgeController>().enabled = true;
                commanderBadgeButton.GetComponent<Button>().interactable = false;
                StopCoroutine(listener);
                StopCoroutine(CheckPending());
            }
        }
    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}