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

    List<GameObject> memberElementList;
    List<GameObject> teamElementList;

    TeamData teamData;
    List<TeamData> teamDataList = new List<TeamData>();
    List<TeamData> teamMemberList = new List<TeamData>();

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
    int amountOfTeams = 10;
    private string[] groups;

    // Use this for initialization
    void Start()
    {
        amountOfTeams = 0;
        StartCoroutine(GetGroupFromServer());
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

    IEnumerator GetGroupFromServer()
    {
        print("GET FROM SERVER");
        string getGroupURL = "http://gocommander.sytes.net/scripts/show_group.php";

        WWW www = new WWW(getGroupURL);

        yield return www;

        string result = www.text;

        if (result != null)
        {
            groups = result.Split(';');
        }
        print("BEFORE FOR LOOP");
        print(groups.Length);
        int id = 0;
        for (int i = 0; i < groups.Length - 1; i++)
        {
            print("BEFORE INT PARSE");
            //string id = GetDataValue(groups[i], "ID:");
            print("AFTER PARSE");
            string groupName = GetDataValue(groups[i], "Groupname:");
            id++;
            print(groupName);
            teamData = new TeamData(id, groupName);
            teamDataList.Add(teamData);
            print("ADD TO LIST");
            //yield return null;
                      
        }
        amountOfTeams = teamDataList.Count;
        print(amountOfTeams);
        //yield return new WaitForSeconds(10);
        PopulateTeamList();
        //yield return null;
    }

    IEnumerator GetMembersInTeam()
    {
        print("GET FROM TEAM");
        string getMembersURL = "http://gocommander.sytes.net/scripts/show_group_members.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", "Killerbunnies");
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;

        if (result != null)
        {
            groups = result.Split(';');
        }
        print("BEFORE FOR LOOP");
        print(groups.Length);

        for (int i = 0; i < groups.Length - 1; i++)
        {
            print("BEFORE INT PARSE");
            int id = int.Parse(GetDataValue(groups[i], "ID:"));
            print("AFTER PARSE");
            string groupMember = GetDataValue(groups[i], "Groupusers:");
            print(groupMember);
            //teamData = new TeamData(id, groupMember);
            teamMemberList.Add(new TeamData(id, groupMember));
            print("ADD TO LIST");
            //yield return null;

        }
        print(amountOfTeams);
        //yield return new WaitForSeconds(10);
        //yield return null;
    }


    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    private void PopulateTeamList()
    {
        print(amountOfTeams);

        teamElementList = new List<GameObject>();

        teamElementTransform = teamElementPrefab.GetComponent<RectTransform>();
        teamScrollTransform = teamListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < teamDataList.Count; i++)
        {
            j++;

            GameObject newTeamElement = Instantiate(teamElementPrefab, teamScrollTransform) as GameObject;
            newTeamElement.transform.SetParent(teamScrollTransform, false);
            teamButton = newTeamElement.GetComponentInChildren<Button>();
            teamButton.enabled = true;
            teamNameText = teamButton.GetComponentInChildren<Text>();
            teamNameText.text = teamDataList[i].name;
            teamNameText.fontSize = 10;

            RectTransform rectTransform = newTeamElement.GetComponent<RectTransform>();

            float x = -teamScrollTransform.rect.width / 2 * (i % 1);
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y + 50;
            rectTransform.offsetMax = new Vector2(x, y);

            teamElementList.Add(newTeamElement);
            teamButton.onClick.AddListener(delegate { TeamBtnClick(teamDataList[i].name); });
        }
    }

    public void PopulateMemberList(string selectedTeam)
    {
        memberElementList = new List<GameObject>();

        memberElementTransform = memberElementPrefab.GetComponent<RectTransform>();
        memberScrollTransform = memberListContent.GetComponent<RectTransform>();

        StartCoroutine(GetMembersInTeam());

        int j = 0;
        for (int i = 0; i < teamMemberList.Count; i++)
        {
            j++;

            GameObject newMemberElement = Instantiate(memberElementPrefab, memberScrollTransform) as GameObject;
            newMemberElement.transform.SetParent(memberScrollTransform, false);
            addFriendBtn = newMemberElement.GetComponentInChildren<Button>();
            addFriendBtn.enabled = true;
            memberNameText = newMemberElement.GetComponentInChildren<Text>();
            memberNameText.text = teamMemberList[i].name;
            memberNameText.fontSize = 10;

            RectTransform rectTransform = newMemberElement.GetComponent<RectTransform>();

            float x = -memberScrollTransform.rect.width / 2 * (i % 1);
            float y = memberScrollTransform.rect.height / 2 - 30 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y + 30;
            rectTransform.offsetMax = new Vector2(x, y);

            memberElementList.Add(newMemberElement);
            addFriendBtn.onClick.AddListener(delegate { AddFriendButtonClick(); });
        }
        print(memberElementList.Count);
    }

    private void AddFriendButtonClick()
    {
        current_UI = UI_Phase.UI_AddFriend;
    }

    public void TeamBtnClick(string name)
    {
        Debug.Log("Joining selected team.." + name);
        current_UI = UI_Phase.UI_Lobby;
        PopulateMemberList(name);
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



