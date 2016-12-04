using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public float timer;
    int showTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        showTime = (int)timer;
        gameObject.GetComponent<Text>().text = "time left: " + showTime.ToString();
        if (timer <= 0)
        {
            StartCoroutine(BackToMainScene());
            enabled = false;
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            print("Blabla");
            StartCoroutine(BackToMainScene());
            enabled = false;            
        }
	}
    IEnumerator BackToMainScene()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_start_vote.php";
        WWWForm form = new WWWForm();
        form.AddField("userVotePost", "PENDING");
        form.AddField("userGroupPost", GoogleMap.groupName);

        WWW www = new WWW(votersURL, form);
        yield return www;

        SceneManager.LoadScene("mainScene");
    }
}
