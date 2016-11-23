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
        helpUI,
        messagePlayerListUI,
        messageOptionsUI,
        ActiveUI
    }
    public static ActiveUI activeUI;

    public Canvas inGameUI;
    public Canvas menuUI;
    public Canvas helpUI;
    public Canvas messagePlayerListUI;
    public Canvas messageOptionsUI;

    public GameObject menu_btn;
    public GameObject back_to_game_btn;
    public GameObject back_to_menu_btn;
    public GameObject help_btn;
    public GameObject exit_btn;
    public GameObject commander_badge_btn;

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

                    messageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                }
                break;
            case ActiveUI.menuUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = true;
                    helpUI.enabled = false;

                    messageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;

                }
                break;
            case ActiveUI.helpUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = true;

                    messageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                }
                break;
            case ActiveUI.messagePlayerListUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    messageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = true;
                }
                break;
            case ActiveUI.messageOptionsUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    messageOptionsUI.enabled = true;
                    messagePlayerListUI.enabled = false;
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
}
