﻿using UnityEngine;
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
        UI_Create,
        UI_Lobby,
        UI_AddFriend
    }
    UI_Phase current_UI;

    public Canvas UI_Join_Create;
    public Canvas UI_Create;
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
    public GameObject teamInputField;

    GameObject newMemberElement;

    public Text userInfo;
    public Text teamInfo;
    public Text memberCountInfo;

    Text[] teamNameTexts;
    Text[] memberNameTexts;
   
    public Button leaveLobbyBtn;
    public Button startMatchBtn;
    public Button backToLobbyBtn;
    public Button createTeamBtn;
    public Button logOutBtn;
    public Button createTeamMenuOptionBtn;

    Button[] teamJoinButtons;
    Button[] addFriendButtons;

    RectTransform memberElementTransform;
    RectTransform teamElementTransform;
    RectTransform memberScrollTransform;
    RectTransform teamScrollTransform;

    private string[] teamArray;
    string newTeamName;
    string selectedTeam;

    int timer;

    // Use this for initialization
    void Start()
    {
        timer = 50;
        current_UI = UI_Phase.UI_Lobby;
        startMatchBtn.onClick.AddListener(delegate { StartButtonClick(); });
        StartCoroutine(GetTeamsFromServer());
        StartCoroutine(GetMembersInTeam(selectedTeam));
        current_UI = UI_Phase.UI_Join_Create;
    }

    void Update()
    {
        timer--;

        switch (current_UI)
        {
            case UI_Phase.UI_Join_Create:
                {
                    UI_Join_Create.enabled = true;
                    UI_Create.enabled = false;
                    UI_Lobby.enabled = false;
                    UI_AddFriend.enabled = false;
                }
                break;
            case UI_Phase.UI_Create:
                {
                    UI_Join_Create.enabled = false;
                    UI_Create.enabled = true;
                    UI_Lobby.enabled = false;
                    UI_AddFriend.enabled = false;
                }
                break;
            case UI_Phase.UI_Lobby:
                {
                    UI_Join_Create.enabled = false;
                    UI_Create.enabled = false;
                    UI_AddFriend.enabled = false;
                    UI_Lobby.enabled = true;
                }
                break;
            case UI_Phase.UI_AddFriend:
                {
                    UI_Join_Create.enabled = false;
                    UI_Create.enabled = false;
                    UI_Lobby.enabled = false;
                    UI_AddFriend.enabled = true;
                }
                break;
        }
    }

    IEnumerator GetTeamsFromServer()
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

    IEnumerator JoinSelectedTeam(string selectedTeam)
    {

        this.selectedTeam = selectedTeam;

        memberList.Clear();

        string getTeamURL = "http://gocommander.sytes.net/scripts/join_group.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", selectedTeam);
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(getTeamURL, form);

        yield return www;

        StartCoroutine(GetMembersInTeam(selectedTeam));
    }

    IEnumerator GetMembersInTeam(string selectedTeam)
    {
            this.selectedTeam = selectedTeam;

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

    IEnumerator CreateNewTeam(string newTeamName)
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/create_group.php";

        userInfo.text = GoogleMap.username;

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", newTeamName);
        form.AddField("usernamePost", userInfo.text);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;

        TeamButtonClick(newTeamName);
    }

    IEnumerator LeaveTeam()
    {

        string getMembersURL = "http://gocommander.sytes.net/scripts/leave_group.php";

        userInfo.text = GoogleMap.username;

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", userInfo.text);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;
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
            teamNameTexts[i].fontSize = 12;

            RectTransform rectTransform = newTeamElement.GetComponent<RectTransform>();

            //float x = -teamScrollTransform.rect.width / 2 * (i % 1);
            //float y = teamScrollTransform.rect.height / 2 - 50 * j;
            float x = 0;
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y;
            rectTransform.offsetMax = new Vector2(x, y);

            AddTeamButtonListeners(teamJoinButtons[i], teamNameTexts[i].text);

        }
        if (timer < 1)
        {
            timer = 50;
            SceneManager.LoadScene("LobbyScene");
        }
    }

    public void PopulateMemberList(string selectedTeam)
    {
                
        addFriendButtons = new Button[memberList.Count];
        memberNameTexts = new Text[memberList.Count];

        memberElementTransform = memberElementPrefab.GetComponent<RectTransform>();
        memberScrollTransform = memberListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < memberList.Count; i++)
        {
            j++;

                newMemberElement = Instantiate(memberElementPrefab, memberScrollTransform) as GameObject;
                newMemberElement.transform.SetParent(memberScrollTransform, false);
                addFriendButtons[i] = newMemberElement.GetComponentInChildren<Button>();
                addFriendButtons[i].enabled = true;
                memberNameTexts[i] = newMemberElement.GetComponentInChildren<Text>();
                memberNameTexts[i].text = memberList[i].name;
                memberNameTexts[i].fontSize = 12;

                RectTransform rectTransform = newMemberElement.GetComponent<RectTransform>();

                float x = -memberScrollTransform.rect.width / 2 * (i % 1);
                float y = memberScrollTransform.rect.height / 2 - 30 * j;
                rectTransform.offsetMin = new Vector2(x, y);

                x = rectTransform.offsetMin.x;
                y = rectTransform.offsetMin.y + 30;
                rectTransform.offsetMax = new Vector2(x, y);

                AddFriendButtonListeners(addFriendButtons[i], memberNameTexts[i].text);         
        }
        memberCountInfo.text = "" + memberList.Count.ToString() + "/10";
    }

    public void AddTeamButtonListeners(Button button, string ID)
    {
        button.onClick.AddListener(delegate { TeamButtonClick(ID); });
    }

    public void AddFriendButtonListeners(Button button, string ID)
    {
        button.onClick.AddListener(delegate { AddFriendButtonClick(ID); });
    }

    public void CreateTeamMenuOptionClick()
    {
        current_UI = UI_Phase.UI_Create;
    }

    public void CreateTeamClick()
    {
        newTeamName = teamInputField.GetComponent<InputField>().text;
        StartCoroutine(CreateNewTeam(newTeamName));
    }

    public void TeamButtonClick(string selectedTeam)
    {
        current_UI = UI_Phase.UI_Lobby;
        teamInfo.text = selectedTeam;
        StartCoroutine(JoinSelectedTeam(selectedTeam));
    }

    private void AddFriendButtonClick(string selectedMember)
    {
        current_UI = UI_Phase.UI_AddFriend;
        userInfo.text = selectedMember;
    }

    public void BackToLobbyButtonClick()
    {
        current_UI = UI_Phase.UI_Lobby;
    }

    public void LeaveLobbyButtonClick()
    {
        LeaveTeam();
        SceneManager.LoadScene("LobbyScene");
    }

    public void StartButtonClick()
    {
        if (memberList.Count > 2)
        {
            GoogleMap.username = userInfo.text;
            GoogleMap.groupName = teamInfo.text;
            SceneManager.LoadScene("mainScene");
        }
    }

    public void LogOutButtonClick()
    {
        SceneManager.LoadScene("login");
    }
}



