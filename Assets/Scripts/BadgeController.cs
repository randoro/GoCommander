using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BadgeController : MonoBehaviour {

    float timerValue;
    public int refreshDelay = 5;
    public static bool interested;
    public static bool update;

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
            }
            else
            {
                StartCoroutine(SendNonInterest());
            }
            Start();
            gameObject.SetActive(false);
        }
	}
    IEnumerator SendCommanderRequest()
    {
        StartCoroutine(WaitForCommanderDecision());
        yield return null;
    }
    IEnumerator SendNonInterest()
    {
        yield return null;
    }
    IEnumerator WaitForCommanderDecision()
    {
        int answer = 0;
        // nåt yield return answer bullshit från servern här gissar jag
        if (answer == 1)
        {
            SceneManager.LoadScene("CommanderScene");
        }
        yield return null;
    }
}