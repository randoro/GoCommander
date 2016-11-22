using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainGame_UI_Changer : MonoBehaviour
{
    private string[] messageList = new string[4];

    public enum ActiveUI
    {
        inGameUI,
        menuUI,
        helpUI
    }
    public ActiveUI activeUI;

    public Canvas inGameUI;
    public Canvas menuUI;
    public Canvas helpUI;

    public GameObject menu_btn;
    public GameObject back_to_game_btn;
    public GameObject back_to_menu_btn;
    public GameObject help_btn;
    public GameObject exit_btn;
    public GameObject commander_badge_btn;
    public GameObject message_btn;

    // Use this for initialization
    void Start ()
    {
        activeUI = ActiveUI.inGameUI;
	}
	
	// Update is called once per frame
	void Update ()
    {
	  switch(activeUI)
        {
            case ActiveUI.inGameUI:
                {
                    inGameUI.enabled = true;
                    menuUI.enabled = false;
                    helpUI.enabled = false;
                }
                break;
            case ActiveUI.menuUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = true;
                    helpUI.enabled = false;

                }
                break;
            case ActiveUI.helpUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = true;
                }
                break;
        }
	}

    public void MenuBtnClick()
    {
        activeUI = ActiveUI.menuUI;
    }

    public void HelpBtnClick()
    {
        activeUI = ActiveUI.helpUI;
    }

    public void BackToGameBtnClick()
    {
        activeUI = ActiveUI.inGameUI;
    }

    public void BackToMenuBtnClick()
    {
        activeUI = ActiveUI.menuUI;
    }

    public void ExitbtnClick()
    {
        SceneManager.LoadScene("login");
    }

    public void CommanderBadgeBtnClick()
    {
        SceneManager.LoadScene("CommanderScene");
    }

    private void SetMessages()
    {
        messageList[0] = "Good work!";
        messageList[1] = "I need a power up";
        messageList[2] = "Message 3";
        messageList[3] = "Message 4";
    }
}
