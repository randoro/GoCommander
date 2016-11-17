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
    public Text userInfo;
    GameObject newMemberElement;

    Text[] teamNameTexts;
    Text[] memberNameTexts;
   
    public Button leaveLobbyBtn;
    public Button startMatchBtn;
    public Button backToLobbyBtn;
    public Button createTeamBtn;
    public Button logOutBtn;

    Button[] teamJoinButtons;
    Button[] addFriendButtons;

    RectTransform memberElementTransform;
    RectTransform teamElementTransform;
    RectTransform memberScrollTransform;
    RectTransform teamScrollTransform;

    bool populateList = false;

    private string[] teamArray;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetTeamFromServer());
        current_UI = UI_Phase.UI_Join_Create;
        startMatchBtn.onClick.AddListener(delegate { StartButtonClick(); });
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
                    UI_AddFriend.enabled = false;
                    UI_Lobby.enabled = true;
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
            teamNameTexts[i].fontSize = 12;

            RectTransform rectTransform = newTeamElement.GetComponent<RectTransform>();

            float x = -teamScrollTransform.rect.width / 2 * (i % 1);
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y + 50;
            rectTransform.offsetMax = new Vector2(x, y);

            AddTeamButtonListeners(teamJoinButtons[i], teamNameTexts[i].text);
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
    }

    public void AddTeamButtonListeners(Button button, string ID)
    {
        button.onClick.AddListener(delegate { TeamButtonClick(ID); });
    }

    public void AddFriendButtonListeners(Button button, string ID)
    {
        button.onClick.AddListener(delegate { AddFriendButtonClick(ID); });
    }

    public void TeamButtonClick(string selectedTeam)
    {
        Debug.Log("Joining selected team.." + selectedTeam);

        current_UI = UI_Phase.UI_Lobby;
        StartCoroutine(GetMembersInTeam(selectedTeam));
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
        current_UI = UI_Phase.UI_Join_Create;
        teamJoinButtons = null;
    }

    public void StartButtonClick()
    {
        SceneManager.LoadScene("mainScene");
    }

    public void LogOutButtonClick()
    {
        SceneManager.LoadScene("login");
    }
}



