using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BadgeController : MonoBehaviour {

    float timerValue;
    public int refreshDelay = 5;
    public int refreshDelayCommander = 2;
    public static bool interested;
    public static bool update;

    public BadgeInfoListener badgeInfoListener;

	// Use this for initialization
	void Start () {
        timerValue = 10;
        interested = false;
	}
	
	// Update is called once per frame
	void Update () {
        timerValue -= Time.deltaTime;
        if (timerValue <= 0){
            if (interested)
            {
                StartCoroutine(SendCommanderRequest());
                SceneManager.LoadScene("CommanderScene");
            }
            else
            {
                StartCoroutine(SendNonInterest());
            }
            Start();
            gameObject.SetActive(false);
            StartCoroutine(badgeInfoListener.Listen());
        }
	}
    IEnumerator SendCommanderRequest()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";
        WWWForm form = new WWWForm();
        form.AddField("userNamePost", GoogleMap.username);
        form.AddField("userVotePost", "CANDIDATE");
        WWW www = new WWW(votersURL, form);
        yield return www;
        StartCoroutine(WaitForCommanderDecision());
    }
    IEnumerator SendNonInterest()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";
        WWWForm form = new WWWForm();
        form.AddField("userNamePost", GoogleMap.username);
        form.AddField("userVotePost", "NOT");
        WWW www = new WWW(votersURL, form);
        yield return www;
    }
    IEnumerator WaitForCommanderDecision()
    {
        while (true)
        {
            string votersURL = "http://gocommander.sytes.net/scripts/commander_check.php";
            WWWForm form = new WWWForm();
            form.AddField("userNamePost", GoogleMap.username);
            WWW www = new WWW(votersURL, form);
            yield return www;
            string result = www.text;

            if (result.Contains("COMMANDER"))
            {
                GoogleMap.lastCommander = true;
                StopCoroutine(WaitForCommanderDecision());
            }
            yield return new WaitForSeconds(refreshDelayCommander);
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