using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Message_UI : MonoBehaviour
{

    private string[] messageList = new string[4];

    public Button backToGameBtn;
    public Button backToPlayerListBtn;
    public Button messagesBtn;

    // Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
       
    }

    public void MessagesBtnClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.messagePlayerListUI;
    }

    public void ToSocialMessagesClick()
    {
            MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.socialMessageOptionsUI;
    }

    public void ToTacticalMessagesClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.tacticalMessageOptionsUI;
    }

    public void ToHelpfulMessagesClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.helpfulMessageOptionsUI;
    }

    public void ToAdditionalMessagesClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.additionalMessageOptionsUI;
    }

    public void BackToGameClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.inGameUI;
    }

    public void BackToPlayerListClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.messagePlayerListUI;
    }

    public void BackToCategoriesClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.messageCategoryListUI;
    }

    public void ToMessageCategories()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.messageCategoryListUI;
    }


}
