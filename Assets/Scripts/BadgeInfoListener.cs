using UnityEngine;
using System.Collections;

public class BadgeInfoListener : MonoBehaviour {
    public int refreshDelay = 5;
    public NextCommanderController nextCommanderController;
    public GameObject commanderBadgeButton;
    string[] polls;

	// Use this for initialization
	public void Start () {
        StartCoroutine(Listen());
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
        form.AddField("userNamePost", GoogleMap.username);
        WWW www = new WWW(votersURL, form);
        yield return www;
        string result = www.text;

        if (result.Contains("PENDING"))
        {
            if (GoogleMap.lastCommander)
            {
                GoogleMap.lastCommander = false;
                StartCoroutine(nextCommanderController.StartVoting());
                commanderBadgeButton.SetActive(true);
                StopCoroutine(Listen());
            }
            else
            {
                commanderBadgeButton.SetActive(true);
                StopCoroutine(Listen());
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