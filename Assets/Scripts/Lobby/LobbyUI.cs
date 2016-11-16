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

    LobbyData teamData;
    LobbyData memberData;
    List<LobbyData> teamList = new List<LobbyData>();
    List<LobbyData> memberList = new List<LobbyData>();

    public GameObject memberListContent;
    public GameObject teamListContent;
    public GameObject teamElementPrefab;
    public GameObject memberElementPrefab;

    Text memberNameText;
    Text teamNameText;
    Text[] teamNameTexts;

    public Button addFriendBtn;
    public Button teamJoinBtn;
    public Button leaveLobbyBtn;
    public Button startMatchBtn;
    public Button backToLobbyBtn;
    public Button createTeamBtn;

    Button[] teamJoinButtons;
    Button[] addFriendButtons;

    RectTransform memberElementTransform;
    RectTransform teamElementTransform;
    RectTransform memberScrollTransform;
    RectTransform teamScrollTransform;

    private string[] teamArray;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetTeamFromServer());
        current_UI = UI_Phase.UI_Join_Create;
        leaveLobbyBtn.onClick.AddListener(delegate { BackToLobbyButtonClick(); });
        startMatchBtn.onClick.AddListener(delegate { StartButtonClick(); });
        backToLobbyBtn.onClick.AddListener(delegate { LeaveLobbyClick(); });
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

    IEnumerator GetTeamFromServer()
    {
        string getTeamsURL = "http://gocommander.sytes.net/scripts/show_group.php";

        WWW www = new WWW(getTeamsURL);

        yield return www;

        string result = www.text;

        if (result != null)
        {
            teamArray = result.Split(';');
        }

        int id = 0;
        for (int i = 0; i < teamArray.Length - 1; i++)
        {

            string teamName = GetLobbyData(teamArray[i], "Groupname:");
            id++;
            teamData = new LobbyData(id, teamName);
            teamList.Add(teamData);
                      
        }
        PopulateTeamList();
    }

    IEnumerator GetMembersInTeam(string selectedTeam)
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/show_group_members.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", selectedTeam);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;

        if (result != null)
        {
            teamArray = result.Split(';');
        }

        for (int i = 0; i < teamArray.Length - 1; i++)
        {
            int id = int.Parse(GetLobbyData(teamArray[i], "ID:"));
            string member = GetLobbyData(teamArray[i], "Groupusers:");
            memberData = new LobbyData(id, member);
            memberList.Add(memberData);
        }
        PopulateMemberList(selectedTeam);
    }


    string GetLobbyData(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    private void PopulateTeamList()
    {
        teamJoinButtons = new Button[teamList.Count];
        teamNameTexts = new Text[teamList.Count];

        teamElementTransform = teamElementPrefab.GetComponent<RectTransform>();
        teamScrollTransform = teamListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < teamList.Count; i++)
        {
            j++;

            GameObject newTeamElement = Instantiate(teamElementPrefab, teamScrollTransform) as GameObject;
            newTeamElement.transform.SetParent(teamScrollTransform, false);
            teamJoinButtons[i] = newTeamElement.GetComponentInChildren<Button>();
            teamJoinButtons[i].enabled = true;
            teamNameTexts[i] = teamJoinButtons[i].GetComponentInChildren<Text>();
            teamNameTexts[i].text = teamList[i].name;
            //teamNameText.fontSize = 10;

            RectTransform rectTransform = newTeamElement.GetComponent<RectTransform>();

            float x = -teamScrollTransform.rect.width / 2 * (i % 1);
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y + 50;
            rectTransform.offsetMax = new Vector2(x, y);

            AddButtonListeners(teamJoinButtons[i], teamNameTexts[i].text);
        }
    }

    public void AddButtonListeners(Button button, string ID)
    {
        button.onClick.AddListener(delegate { TeamBtnClick(ID); });
    }

    public void PopulateMemberList(string selectedTeam)
    {

        memberElementTransform = memberElementPrefab.GetComponent<RectTransform>();
        memberScrollTransform = memberListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < memberList.Count; i++)
        {
            j++;

            GameObject newMemberElement = Instantiate(memberElementPrefab, memberScrollTransform) as GameObject;
            newMemberElement.transform.SetParent(memberScrollTransform, false);
            addFriendBtn = newMemberElement.GetComponentInChildren<Button>();
            addFriendBtn.enabled = true;
            memberNameText = newMemberElement.GetComponentInChildren<Text>();
            memberNameText.text = memberList[i].name;
            memberNameText.fontSize = 10;

            RectTransform rectTransform = newMemberElement.GetComponent<RectTransform>();

            float x = -memberScrollTransform.rect.width / 2 * (i % 1);
            float y = memberScrollTransform.rect.height / 2 - 30 * j;
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

    public void TeamBtnClick(string selectedTeam)
    {
        Debug.Log("Joining selected team.." + name);
        current_UI = UI_Phase.UI_Lobby;
        StartCoroutine(GetMembersInTeam(selectedTeam));
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



