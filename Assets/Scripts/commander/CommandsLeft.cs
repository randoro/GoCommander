using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommandsLeft : MonoBehaviour {

    public static int commandsLeft = 7;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = "commands left: " + commandsLeft.ToString();
        if (commandsLeft == 0)
        {
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
