using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MinigameHelpMessages : MonoBehaviour {

    public GameObject help_UI;
    public Button ok_button;

    int timer = 550;

    //static bool stopShowingMessage;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer--;

        if(timer < 1)
        {
            help_UI.SetActive(false);
            Destroy(help_UI);
        }
	}

    public void OkClick()
    {
        help_UI.SetActive(false);
        Destroy(help_UI);
    }
}
