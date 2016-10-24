using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public Canvas MainMenu_UI;
    public Canvas RankedMatch_UI;
    public Canvas HowToPlay_UI;

    public GameObject quickMatchBtn;
    public GameObject rankedMatchBtn;
    public GameObject howToPlayBtn;
    public GameObject exitBtn;

    public enum Phase
    {
        MainMenu,
        RankedMatch,
        HowToPlay
    }
    Phase current_phase;
    
    // Use this for initialization
	void Start ()
    {
        current_phase = Phase.MainMenu;
	}
	
	// Update is called once per frame
	void Update ()
    {
	     switch(current_phase)
        {
            case Phase.MainMenu:
            {
               MainMenu_UI.enabled = true;
               RankedMatch_UI.enabled = false;
               HowToPlay_UI.enabled = false;

                }
            break;
            case Phase.RankedMatch:
            {
                MainMenu_UI.enabled = false;
                RankedMatch_UI.enabled = true;
                HowToPlay_UI.enabled = false;
            }
            break;
            case Phase.HowToPlay:
            {
                 MainMenu_UI.enabled = false;
                 RankedMatch_UI.enabled = false;
                 HowToPlay_UI.enabled = true;
            }
           break;
        }
	}

    public void RankedMatchClick()
    {
        current_phase = Phase.RankedMatch;
    }

    public void HowToPlayClick()
    {
        current_phase = Phase.HowToPlay;
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}
