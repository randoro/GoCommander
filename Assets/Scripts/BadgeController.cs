using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BadgeController : MonoBehaviour {

    float timerValue;
    public int refreshDelay = 5;
    public int refreshDelayCommander = 2;
    public static bool interested;
    public static bool update;
    public static bool moveBadge;
    bool turnItOff;
    Vector3 badgePosition;
    Vector3 moveDirection;
    Vector3 startPosition;
    Vector3 endPosition;
    public float notLocalEndX;
    float speedRadius = 0;

    public BadgeInfoListener badgeInfoListener;

	// Use this for initialization
	void Start () {
        moveBadge = true;
        turnItOff = false;
        startPosition = transform.position;
        endPosition = new Vector3(notLocalEndX, transform.position.y, transform.position.z);
        badgePosition = endPosition;
        moveDirection = badgePosition - startPosition;
        speedRadius = Vector3.Distance(transform.position, badgePosition);

        SetStartValues();
	}
    void SetStartValues()
    {
        timerValue = 10;
        interested = false;
    }
	// Update is called once per frame
	void Update () {
        if (moveBadge)
        {
            if (!WithinDistance(badgePosition, transform.position, 0.01f))
            {
                SnapToGrid();
            }
            else
            {
                moveBadge = false;
            }
        }
        else
        {
            timerValue -= Time.deltaTime;
            if (timerValue <= 0)
            {

                moveBadge = true;
                badgePosition = startPosition;

                if (interested)
                {
                    //StartCoroutine(SendCommanderRequest());
                }
                else
                {
                    //StartCoroutine(SendNonInterest());
                }
                SetStartValues();
                enabled = false;
                //gameObject.transform.localScale = new Vector3(0, 0, 0);
            }
            else if (interested)
            {
                moveBadge = true;
                badgePosition = startPosition;
                //StartCoroutine(SendCommanderRequest());
                SetStartValues();
                enabled = false;
                //gameObject.transform.localScale = new Vector3(0, 0, 0);
            }
        }
	}
    private void SnapToGrid()
    {
        float distance_right_now = Vector3.Distance(transform.position, badgePosition);
        transform.position += moveDirection * (distance_right_now / speedRadius) * 0.1f;
    }
    private bool WithinDistance(Vector3 realPos, Vector3 pos, float radius)
    {
        if (Vector3.Distance(realPos, pos) < radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //IEnumerator SendCommanderRequest()
    //{
    //    string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";
    //    WWWForm form = new WWWForm();
    //    form.AddField("usernamePost", GoogleMap.username);
    //    form.AddField("userVotePost", "CANDIDATE");
    //    WWW www = new WWW(votersURL, form);
    //    yield return www;
    //    StartCoroutine(WaitForCommanderDecision());
    //}

    // Note(Calle): This should be the commander thing that does the commander thing
    //IEnumerator BecomeCommander()
    //{
    //    string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";
    //    WWWForm form = new WWWForm();
    //    form.AddField("usernamePost", GoogleMap.username);
    //    form.AddField("userVotePost", "COMMANDER");
    //    form.AddField("userGroupPost", GoogleMap.groupName);
    //    WWW www = new WWW(votersURL, form);
    //    yield return www;
    //}
    IEnumerator SendNonInterest()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        form.AddField("userVotePost", "NOT");
        WWW www = new WWW(votersURL, form);
        yield return www;
        badgeInfoListener.StartCoroutine(badgeInfoListener.listener);
    }
    //IEnumerator WaitForCommanderDecision()
    //{
    //    while (true)
    //    {
    //        string votersURL = "http://gocommander.sytes.net/scripts/commander_check.php";
    //        WWWForm form = new WWWForm();
    //        form.AddField("usernamePost", GoogleMap.username);
    //        WWW www = new WWW(votersURL, form);
    //        yield return www;
    //        string result = www.text;
    //        print(result);

    //        if (result.Contains("COMMANDER"))
    //        {
    //            GoogleMap.lastCommander = true;
    //            StopCoroutine(WaitForCommanderDecision());
    //            badgeInfoListener.StartCoroutine(badgeInfoListener.listener);
    //            SceneManager.LoadScene("CommanderScene");
    //        }
    //        else if(result.Contains("STOP"))
    //        {
    //            StopCoroutine(WaitForCommanderDecision());
    //            StartCoroutine(badgeInfoListener.listener);
    //        }
    //        yield return new WaitForSeconds(refreshDelayCommander);
    //    }
    //}
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}