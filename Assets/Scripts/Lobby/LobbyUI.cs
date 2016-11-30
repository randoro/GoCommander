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
    List<LobbyData> fetchedMemberList = new List<LobbyData>();
    List<GameObject> memberList = new List<GameObject>();

    public GameObject memberListContent;
    public GameObject teamListContent;
    public GameObject teamElementPrefab;
    public GameObject memberElementPrefab;
    public GameObject teamInputField;

    GameObject newMemberElement;

    public Text userInfo;
    public Text teamInfo;
    public Text memberCountInfo;
    public Text updatingText;

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

    int maxTeamMembers = 5;

    bool canPopulate = true; 

    // Use this for initialization
    void Start()
    {
        InGameTimer.timeLeft = 1200.0f;
        PlayerPrefs.SetFloat("Player Time", InGameTimer.timeLeft);

        current_UI = UI_Phase.UI_Join_Create;
        startMatchBtn.onClick.AddListener(delegate { StartButtonClick(); });
        InvokeRepeating("StartLoop", 0.2f, 5);       
    }

    private void StartLoop()
    {
        if (current_UI == UI_Phase.UI_Join_Create)
        {
            StartCoroutine(GetTeamsFromServer());
        }
        if(selectedTeam != null && current_UI == UI_Phase.UI_Lobby)
        {
            updatingText.text = "searching for players...";
            StartCoroutine(GetMembersInTeam(selectedTeam));
        }
    }


    void Update()
    {
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

        fetchedMemberList.Clear();

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
        fetchedMemberList.Clear();

        //if (addFriendButtons != null && memberNameTexts != null)
        //{
        //    for (int i = 0; i < maxTeamMembers; i++)
        //    {
        //        Destroy(newMemberElement);
        //        Destroy(newMemberElement.GetComponent<Button>());
        //        Destroy(newMemberElement.GetComponentInChildren<Button>());
        //        Destroy(addFriendButtons[i]);
        //        Destroy(memberNameTexts[i]);
        //        Destroy(addFriendButtons[i].GetComponent<Button>());
        //        Destroy(memberNameTexts[i].GetComponent<Text>());
        //        Array.Clear(memberNameTexts, i, maxTeamMembers);
        //        Array.Clear(addFriendButtons, i, maxTeamMembers);
        //        addFriendButtons = null;
        //        memberNameTexts = null;
        //    }
        //}

        //addFriendButtons = new Button[maxTeamMembers];
        //memberNameTexts = new Text[maxTeamMembers];

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
                if (fetchedMemberList.Count < maxTeamMembers + 1)
                {
                  int id = int.Parse(GetLobbyData(teamArray[i], "ID:"));
                  string member = GetLobbyData(teamArray[i], "Groupusers:");
                  memberData = new LobbyData(id, member);
                  fetchedMemberList.Add(memberData);
                }
            }
        CheckMemberListChanged();
    }

    IEnumerator CreateNewTeam(string newTeamName)
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/create_group.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", newTeamName);
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;

        TeamButtonClick(newTeamName);
    }

    IEnumerator LeaveTeam()
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/leave_group.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;

        CheckMemberListChanged();
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

            float x = 0;
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y;
            rectTransform.offsetMax = new Vector2(x, y);

            AddTeamButtonListeners(teamJoinButtons[i], teamNameTexts[i].text);
        }
    }

    public void PopulateMemberList(string selectedTeam)
    {
        if (addFriendButtons != null && memberNameTexts != null)
        {
            for (int i = 0; i < maxTeamMembers; i++)
            {
                Destroy(newMemberElement);
                Destroy(newMemberElement.GetComponent<Button>());
                Destroy(newMemberElement.GetComponentInChildren<Button>());
                Destroy(addFriendButtons[i]);
                Destroy(memberNameTexts[i]);
                Destroy(addFriendButtons[i].GetComponent<Button>());
                Destroy(memberNameTexts[i].GetComponent<Text>());
                Array.Clear(memberNameTexts, i, maxTeamMembers);
                Array.Clear(addFriendButtons, i, maxTeamMembers);
                addFriendButtons = null;
                memberNameTexts = null;
            }
        }

        addFriendButtons = new Button[maxTeamMembers];
        memberNameTexts = new Text[maxTeamMembers];

        memberElementTransform = memberElementPrefab.GetComponent<RectTransform>();
        memberScrollTransform = memberListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < fetchedMemberList.Count; i++)
        {
            j++;

            newMemberElement = Instantiate(memberElementPrefab, memberScrollTransform) as GameObject;
            newMemberElement.transform.SetParent(memberScrollTransform, false);
            addFriendButtons[i] = newMemberElement.GetComponentInChildren<Button>();
            addFriendButtons[i].enabled = true;
            memberNameTexts[i] = addFriendButtons[i].GetComponentInChildren<Text>();
            memberNameTexts[i].text = fetchedMemberList[i].name;
            memberNameTexts[i].fontSize = 12;

            memberList.Add(newMemberElement);

            RectTransform rectTransform = newMemberElement.GetComponent<RectTransform>();

            float x = 0;
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y;
            rectTransform.offsetMax = new Vector2(x, y);

            AddFriendButtonListeners(addFriendButtons[i], memberNameTexts[i].text);

            updatingText.text = " ";
        }
        
        Debug.Log("Updating list");
        memberCountInfo.text = "" + fetchedMemberList.Count.ToString() + "/" + maxTeamMembers.ToString();
    }

    private void CheckMemberListChanged()
    {
        if(memberList.Count != fetchedMemberList.Count)
        {
            PopulateMemberList(selectedTeam);
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
        GoogleMap.groupName = selectedTeam;
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
        //if (memberList.Count > 2)
        //{
            GoogleMap.groupName = selectedTeam;
            SceneManager.LoadScene("mainScene");
        //}
    }

    public void LogOutButtonClick()
    {
        SceneManager.LoadScene("login");
    }
}



