using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    enum UI_Phase
    {
        UI_Main,
        UI_AddFriend
    }
    UI_Phase current_UI;

    public Canvas UI_Main;
    public Canvas UI_AddFriend;

    public GameObject scrollviewContent;
    public GameObject listElementPrefab;

    Text contentText;
    public Button addFriendBtn;
    public Button backBtn;

    RectTransform listElementTransform;
    RectTransform scrollviewTransform;

    int amountofMembers = 10;

    // Use this for initialization
    void Start()
    {
        current_UI = UI_Phase.UI_Main;
        PopulateList();
    }

    void Update()
    {
        switch(current_UI)
        {
            case UI_Phase.UI_Main:
                {
                    UI_Main.enabled = true;
                    UI_AddFriend.enabled = false;
                }
                break;
            case UI_Phase.UI_AddFriend:
                {
                    UI_Main.enabled = false;
                    UI_AddFriend.enabled = true;
                }
                break;
        }
    }

    public void PopulateList()
    {
        listElementTransform = listElementPrefab.GetComponent<RectTransform>();
        scrollviewTransform = scrollviewContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < amountofMembers; i++)
        {
            j++;

            GameObject newListElement = Instantiate(listElementPrefab, scrollviewTransform) as GameObject;
            newListElement.transform.SetParent(scrollviewTransform, false);
            addFriendBtn = newListElement.GetComponentInChildren<Button>();
            addFriendBtn.enabled = true;
            contentText = newListElement.GetComponentInChildren<Text>();
            contentText.text = "Test Member";
            contentText.fontSize = 10;

            RectTransform rectTransform = newListElement.GetComponent<RectTransform>();

            float x = -scrollviewTransform.rect.width / 2 * (i % 1);
            float y = scrollviewTransform.rect.height / 2 - 30 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y + 30;
            rectTransform.offsetMax = new Vector2(x, y);

            addFriendBtn.onClick.AddListener(delegate { AddFriendButtonClick(); });
        }
    }

    private void AddFriendButtonClick()
    {
        current_UI = UI_Phase.UI_AddFriend;
    }

    private void BackBtnClick()
    {
        current_UI = UI_Phase.UI_Main;
    }
}



