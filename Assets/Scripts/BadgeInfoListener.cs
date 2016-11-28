using UnityEngine;
using System.Collections;

public class BadgeInfoListener : MonoBehaviour {
    public int refreshDelay = 5;
    public NextCommanderController nextCommanderController;

	// Use this for initialization
	public void Start () {
        StartCoroutine(Listen());
	}
	
	// Update is called once per frame
	IEnumerator Listen() {
        while (true)
        {
            // Här får det väl finnas nån if-sats som kollar om värdet har hittats - i så fall borde det här objektet inaktiveras för att aktiveras sen igen när
            // ett svar har skickats till servern gällande intresseanmälan för commander-rollen.
            yield return new WaitForSeconds(refreshDelay);
        }
	}

    IEnumerator CheckPending()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";

        WWW www = new WWW(votersURL);
        yield return www;
    }
}