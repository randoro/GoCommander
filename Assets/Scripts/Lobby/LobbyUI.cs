using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class LobbyUI : MonoBehaviour
{
    enum UI_Phase
    {
        UI_Join_Create,
        UI_Lobby,
        UI_AddFriend
    }
    UI_Phase current_UI;

    public Canvas UI_Join_Create;
    public Canvas UI_Lobby;
    public Canvas UI_AddFriend;

    List<GameObject> memberList;
    List<GameObject> teamList;

    LobbyItem lobbyItem;
    List<LobbyItem> lobbyList = new List<LobbyItem>();

    public GameObject memberListContent;
    public GameObject teamListContent;
    public GameObject teamElementPrefab;
    public GameObject memberElementPrefab;

    Text memberNameText;
    Text teamNameText;

    public Button addFriendBtn;
    public Button teamButton;
    public Button leaveLobbyBtn;
    public Button startMatchBtn;
    public Button backToLobbyBtn;
    public Button createTeamBtn;

    RectTransform memberElementTransform;
    RectTransform teamElementTransform;
    RectTransform memberScrollTransform;
    RectTransform teamScrollTransform;

    int amountofMembers = 10;
    int amountOfTeams;
    private string[] groups;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetGroupFromServer());
        current_UI = UI_Phase.UI_Join_Create;
        leaveLobbyBtn.onClick.AddListener(delegate { BackToLobbyButtonClick(); });
        startMatchBtn.onClick.AddListener(delegate { StartButtonClick(); });
        backToLobbyBtn.onClick.AddListener(delegate { LeaveLobbyClick(); });
        teamButton.onClick.AddListener(delegate { TeamBtnClick(); });
        PopulateTeamList();
    }

    void Update()
    {
        switch(current_UI)
        {
            case UI_Phase.UI_Join_Create:
                {
                    UI_Join_Create.enabled = true;
                    UI_Lobby.enabled = false;
                    UI_AddFriend.enabled = false;
                }
                break;
            case UI_Phase.UI_Lobby:
                {
                    UI_Join_Create.enabled = false;
                    UI_Lobby.enabled = true;
                    UI_AddFriend.enabled = false;
                }
                break;
            case UI_Phase.UI_AddFriend:
                {
                    UI_Join_Create.enabled = false;
                    UI_Lobby.enabled = false;
                    UI_AddFriend.enabled = true;
                }
                break;
        }
    }

    IEnumerator GetGroupFromServer()
    {
        string getGroupURL = "http://gocommander.sytes.net/scripts/show_group.php";

        WWW www = new WWW(getGroupURL);

        //yield return www;

        string result = www.text;

        if (result != null)
        {
            groups = result.Split(';');
        }

        for (int i = 0; i < groups.Length - 1; i++)
        {
            int id = int.Parse(GetDataValue(groups[i], "ID:"));
            string groupName = GetDataValue(groups[i], "Groupname:");

            lobbyItem = new LobbyItem(id, groupName);
            lobbyList.Add(lobbyItem); 
                      
        }
        amountOfTeams = lobbyList.Count;
        yield return new WaitForSeconds(5);
    }

    private int GetLobbyListSize()
    {
        amountOfTeams = lobbyList.Count;
        print(amountOfTeams);
        return amountOfTeams;
    }


    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    public void PopulateMemberList()
    {
        memberList = new List<GameObject>();

        memberElementTransform = memberElementPrefab.GetComponent<RectTransform>();
        memberScrollTransform = memberListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < amountofMembers; i++)
        {
            j++;

            GameObject newMemberElement = Instantiate(memberElementPrefab, memberScrollTransform) as GameObject;
            newMemberElement.transform.SetParent(memberScrollTransform, false);
            addFriendBtn = newMemberElement.GetComponentInChildren<Button>();
            addFriendBtn.enabled = true;
            memberNameText = newMemberElement.GetComponentInChildren<Text>();
            memberNameText.text = "Test Member";
            memberNameText.fontSize = 10;

            RectTransform rectTransform = newMemberElement.GetComponent<RectTransform>();

            float x = -memberScrollTransform.rect.width / 2 * (i % 1);
            float y = memberScrollTransform.rect.height / 2 - 30 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y + 30;
            rectTransform.offsetMax = new Vector2(x, y);

            memberList.Add(newMemberElement);
            addFriendBtn.onClick.AddListener(delegate { AddFriendButtonClick(); });
        }
    }

    private void PopulateTeamList()
    {
        teamList = new List<GameObject>();

        teamElementTransform = teamElementPrefab.GetComponent<RectTransform>();
        teamScrollTransform = teamListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < amountOfTeams; i++)
        {
            j++;

            GameObject newTeamElement = Instantiate(teamElementPrefab, teamScrollTransform) as GameObject;
            newTeamElement.transform.SetParent(teamScrollTransform, false);
            teamButton = newTeamElement.GetComponentInChildren<Button>();
            teamButton.enabled = true;
            teamNameText = teamButton.GetComponentInChildren<Text>();
            teamNameText.text = "Test Team";
            teamNameText.fontSize = 10;

            RectTransform rectTransform = newTeamElement.GetComponent<RectTransform>();

            float x = -teamScrollTransform.rect.width / 2 * (i % 1);
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y + 50;
            rectTransform.offsetMax = new Vector2(x, y);

            teamList.Add(newTeamElement);
        }
    }

    private void AddFriendButtonClick()
    {
        current_UI = UI_Phase.UI_AddFriend;
    }

    public void TeamBtnClick()
    {
        Debug.Log("Joining selected team..");
    }

    public void BackToLobbyButtonClick()
    {
        current_UI = UI_Phase.UI_Lobby;
    }

    public void LeaveLobbyClick()
    {
        current_UI = UI_Phase.UI_Join_Create;
    }

    public void StartButtonClick()
    {
        SceneManager.LoadScene("mainScene");
    }
}



