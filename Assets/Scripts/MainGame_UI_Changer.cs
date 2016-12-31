using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGame_UI_Changer : MonoBehaviour
{
    private string[] messageList = new string[4];

    public enum ActiveUI
    {
        inGameUI,
        menuUI,
        helpUI,
        messagePlayerListUI,
        messageCategoryListUI,
        socialMessageOptionsUI,
        tacticalMessageOptionsUI,
        helpfulMessageOptionsUI,
        additionalMessageOptionsUI,
        treasureOrbsUI,
    }
    public static ActiveUI activeUI;

    public Canvas inGameUI;
    public Canvas menuUI;
    public Canvas helpUI;
    public Canvas messagePlayerListUI;
    public Canvas messageCategoryListUI;
    public Canvas socialMessageOptionsUI;
    public Canvas tacticalMessageOptionsUI;
    public Canvas helpfulMessagesOptionsUI;
    public Canvas additionalMessageOptionsUI;
    public Canvas treasureOrbsUI;

    public GameObject menu_btn;
    public GameObject back_to_game_btn;
    public GameObject back_to_menu_btn;
    public GameObject help_btn;
    public GameObject exit_btn;
    public GameObject commander_badge_btn;
    public Button scoreBtn;

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
            //NO MENUS ACTIVE, ONLY ACTUAL GAME AND SCORE/TIME PANEL
            case ActiveUI.inGameUI:
                {
                    //ResumeGame();

                    inGameUI.enabled = true;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;

                }
                break;
            case ActiveUI.menuUI:
                {
                    //PauseGame();

                    inGameUI.enabled = false;
                    menuUI.enabled = true;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;

                }
                break;
            case ActiveUI.helpUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = true;

                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;
                }
                break;
            case ActiveUI.messagePlayerListUI:
                {

                    //PauseGame();

                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = true;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;

                }
                break;
            case ActiveUI.messageCategoryListUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    messageCategoryListUI.enabled = true;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = false;
                }
                break;
            case ActiveUI.socialMessageOptionsUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = true;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;

                }
                break;
            case ActiveUI.tacticalMessageOptionsUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = true;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;

                }
                break;
            case ActiveUI.helpfulMessageOptionsUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = true;
                    additionalMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;

                }
                break;
            case ActiveUI.additionalMessageOptionsUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = false;
                    tacticalMessageOptionsUI.enabled = false;
                    helpfulMessagesOptionsUI.enabled = false;
                    additionalMessageOptionsUI.enabled = true;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = false;
                    messageCategoryListUI.enabled = false;

                }
                break;
            case ActiveUI.treasureOrbsUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = false;
                    helpUI.enabled = false;

                    socialMessageOptionsUI.enabled = false;
                    messagePlayerListUI.enabled = false;
                    treasureOrbsUI.enabled = true;
                    messageCategoryListUI.enabled = false;

                }
                break;
        }
	}

    ////////////////////////
    //PAUSE GAME TEST
    ///////////////////////

    void PauseGame()
    {
        //Orbit orbit = FindObjectOfType<Orbit>();
        //orbit.enabled = false;
    }

    void ResumeGame()
    {
        //Orbit orbit = FindObjectOfType<Orbit>();
        //orbit.enabled = true;
    }

    ///////////////////

    IEnumerator LeaveTeam()
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/leave_group.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;
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
        StartCoroutine(LeaveTeam());
        GameObject.FindGameObjectWithTag("TreasureSpawner").GetComponent<TreasureSpawner>().StartRemoveUserTreasures();
        SceneManager.LoadScene("login");
    }

    public void CommanderBadgeBtnClick()
    {
        BadgeController.interested = true;
    }

    public void ScoreBtnClick()
    {
        SceneManager.LoadScene("highscoreScene");
    }
}
