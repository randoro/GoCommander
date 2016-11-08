using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {

    public GameObject UIBase;
    public GameObject scrollviewContent;
    public GameObject listElementPrefab;

    Text contentText;
    Button addFriendBtn;
    RectTransform listElementTransform;
    RectTransform scrollviewTransform;

    int amountofMembers = 10;

    // Use this for initialization
    void Start()
    {
        PopulateList();
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

            float x = -scrollviewTransform.rect.width / 2 + 30 * (i % 1);
            float y = scrollviewTransform.rect.height / 2 - 30 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x + 30;
            y = rectTransform.offsetMin.y + 30;
            rectTransform.offsetMax = new Vector2(x, y);
        }
    }

}

