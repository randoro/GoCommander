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
    List<LobbyData> fetchedTeamList = new List<LobbyData>();
    List<LobbyData> teamList = new List<LobbyData>();
    List<GameObject> teamObjectList = new List<GameObject>();

    LobbyData memberData;
    List<LobbyData> fetchedMemberList = new List<LobbyData>();
    List<LobbyData> memberList = new List<LobbyData>();
    List<GameObject> memberObjectList = new List<GameObject>();

    public GameObject memberListContent;
    public GameObject teamListContent;
    public GameObject teamElementPrefab;
    public GameObject memberElementPrefab;
    public GameObject teamInputField;

    GameObject newMemberElement;
    GameObject newTeamElement;

    public Text userInfo;
    public Text teamInfo;
    public Text memberCountInfo;
    public Text teamCountInfo;
    public Text updatingMembersText;
    public Text updatingTeamsText;


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

        StartCoroutine(StartLoop());
    }

    private IEnumerator StartLoop()
    {
        while (true)
        {
            if (current_UI == UI_Phase.UI_Join_Create)
            {
                updatingTeamsText.text = "searching for teams...";
                
                yield return StartCoroutine(GetTeamsFromServer());
                if (TeamListChanged())
                {
                    PopulateTeamList();
                }
            }
            else if (selectedTeam != null && current_UI == UI_Phase.UI_Lobby)
            {
                updatingMembersText.text = "searching for players...";

                yield return StartCoroutine(GetMembersInTeam());
                if (MemberListChanged())
                {
                    PopulateMemberList();
                }
            }
            yield return new WaitForSeconds(5);
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
        fetchedTeamList.Clear();

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
            fetchedTeamList.Add(teamData);                    
        }
        //PopulateTeamList();
    }

    IEnumerator JoinSelectedTeam()
    {
        //this.selectedTeam = selectedTeam;

        //fetchedMemberList.Clear();

        string getTeamURL = "http://gocommander.sytes.net/scripts/join_group.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", selectedTeam);
        form.AddField("usernamePost", GoogleMap.username);
        WWW www = new WWW(getTeamURL, form);

        yield return www;

        //StartCoroutine(GetMembersInTeam(selectedTeam));
    }

    IEnumerator GetMembersInTeam()
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

        //this.selectedTeam = selectedTeam;

        string getMembersURL = "http://gocommander.sytes.net/scripts/show_group_members.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", selectedTeam);
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;

        print(result);

        if (result != null)
        {
            teamArray = result.Split(';');
        }

        for (int i = 0; i < teamArray.Length - 1; i++)
        {
            int id = int.Parse(GetLobbyData(teamArray[i], "ID:"));
            string member = GetLobbyData(teamArray[i], "Groupusers:");
            memberData = new LobbyData(id, member);
            fetchedMemberList.Add(memberData);
        }
        
        //CheckMemberListChanged();
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

        //StartCoroutine(GetMembersInTeam(selectedTeam));
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
        //teamJoinButtons = new Button[fetchedTeamList.Count];
        //teamNameTexts = new Text[fetchedTeamList.Count];

        teamElementTransform = teamElementPrefab.GetComponent<RectTransform>();
        teamScrollTransform = teamListContent.GetComponent<RectTransform>();

        for (int i = 0; i < teamObjectList.Count; i++) // Note (Calle): This will not run the first loop - otherwise the whole thing would explode I guess
        {
            Destroy(teamObjectList[i]);
            //addFriendButtons[i] = null;
            //memberNameTexts[i] = null;
        }

        int j = 0;
        for (int i = 0; i < fetchedTeamList.Count; i++)
        {
            j++;

            newTeamElement = Instantiate(teamElementPrefab, teamScrollTransform) as GameObject;
            newTeamElement.transform.SetParent(teamScrollTransform, false);
            //teamJoinButtons[i] = newTeamElement.GetComponentInChildren<Button>();
            Button teamJoinButton = newTeamElement.GetComponentInChildren<Button>();
            teamJoinButton.enabled = true;
            //teamNameTexts[i] = teamJoinButtons[i].GetComponentInChildren<Text>();
            Text teamNameText = teamJoinButton.GetComponentInChildren<Text>();
            teamNameText.text = fetchedTeamList[i].name;
            teamNameText.fontSize = 12;

            teamObjectList.Add(newTeamElement);

            RectTransform rectTransform = newTeamElement.GetComponent<RectTransform>();

            float x = 0;
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y;
            rectTransform.offsetMax = new Vector2(x, y);

            AddTeamButtonListeners(teamJoinButton, teamNameText.text);

            updatingTeamsText.text = "updating team list...";
        }
        teamCountInfo.text = "avaliable teams: " + fetchedTeamList.Count.ToString();
    }

    public void PopulateMemberList()
    {
        //if (addFriendButtons != null && memberNameTexts != null)
        //{
        //    for (int i = 0; i < maxTeamMembers; i++)
        //    {
        //        Destroy(memberList[i]);

        //        //Destroy(newMemberElement);
        //        //Destroy(newMemberElement.GetComponent<Button>());
        //        //Destroy(newMemberElement.GetComponentInChildren<Button>());
        //        //Destroy(addFriendButtons[i]);
        //        //Destroy(memberNameTexts[i]);
        //        //Destroy(addFriendButtons[i].GetComponent<Button>());
        //        //Destroy(memberNameTexts[i].GetComponent<Text>());
        //        //Array.Clear(memberNameTexts, i, maxTeamMembers);
        //        //Array.Clear(addFriendButtons, i, maxTeamMembers);
        //        //addFriendButtons = null;
        //        //memberNameTexts = null;
        //    }
        //}

        for (int i = 0; i < memberObjectList.Count; i++) // Note (Calle): This will not run the first loop - otherwise the whole thing would explode I guess
        {
            Destroy(memberObjectList[i]);
            //addFriendButtons[i] = null;
            //memberNameTexts[i] = null;
        }

        //addFriendButtons = new Button[memberList.Count];
        //memberNameTexts = new Text[memberList.Count];

        memberElementTransform = memberElementPrefab.GetComponent<RectTransform>();
        memberScrollTransform = memberListContent.GetComponent<RectTransform>();

        int j = 0;
        for (int i = 0; i < memberList.Count; i++)
        {
            j++;

            newMemberElement = Instantiate(memberElementPrefab, memberScrollTransform) as GameObject;
            newMemberElement.transform.SetParent(memberScrollTransform, false);
            Button addFriendButton = newMemberElement.GetComponentInChildren<Button>();
            addFriendButton.enabled = true;
            //addFriendButtons[i] = newMemberElement.GetComponentInChildren<Button>();
            //addFriendButtons[i].enabled = true;
            Text memberNameText = addFriendButton.GetComponentInChildren<Text>();
            memberNameText.text = memberList[i].name;
            memberNameText.fontSize = 12;
            //memberNameTexts[i] = addFriendButtons[i].GetComponentInChildren<Text>();
            //memberNameTexts[i].text = fetchedMemberList[i].name;
            //memberNameTexts[i].fontSize = 12;

            memberObjectList.Add(newMemberElement);

            RectTransform rectTransform = newMemberElement.GetComponent<RectTransform>();

            float x = 0;
            float y = teamScrollTransform.rect.height / 2 - 50 * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x;
            y = rectTransform.offsetMin.y;
            rectTransform.offsetMax = new Vector2(x, y);

            //AddFriendButtonListeners(addFriendButtons[i], memberNameTexts[i].text);
            AddFriendButtonListeners(addFriendButton, memberNameText.text);

            updatingMembersText.text = "searching for players...";
        }

        memberCountInfo.text = "" + fetchedMemberList.Count.ToString() + " / " 
            + maxTeamMembers.ToString()+ " members";

    }

    private bool MemberListChanged()
    {
        if (memberList.Count != fetchedMemberList.Count)
        {
            CloneToMemberList();
            return true;
        }
        else
        {
            for (int i = 0; i < memberList.Count; i++)
            {
                if(!memberList.Exists(x => x.name.Contains(fetchedMemberList[i].name)))
                {
                    CloneToMemberList();
                    return true;
                }
            }
        }
        return false;
    }

    private void CloneToMemberList()
    {
        memberList.Clear();

        List<LobbyData> fetchedMemberListClone = new List<LobbyData>();

        for (int j = 0; j < fetchedMemberList.Count; j++)
        {
            fetchedMemberListClone.Add((LobbyData)fetchedMemberList[j].Clone());
        }

        memberList = fetchedMemberListClone;
    }

    private bool TeamListChanged()
    {
        if (teamList.Count != fetchedTeamList.Count)
        {
            CloneToTeamList();
            return true;
        }
        else
        {
            for (int i = 0; i < teamList.Count; i++)
            {
                if (!teamList.Exists(x => x.name.Contains(fetchedTeamList[i].name)))
                {
                    CloneToTeamList();
                    return true;
                }
            }
        }
        return false;
    }

    private void CloneToTeamList()
    {
        teamList.Clear();

        List<LobbyData> fetchedTeamListClone = new List<LobbyData>();

        for (int j = 0; j < fetchedTeamList.Count; j++)
        {
            fetchedTeamListClone.Add((LobbyData)fetchedTeamList[j].Clone());
        }

        teamList = fetchedTeamListClone;
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
        StartCoroutine(TeamButtonOperations(selectedTeam));
    }
    IEnumerator TeamButtonOperations(string selectedTeam)
    {
        //yield return StartCoroutine(GetMembersInTeam(selectedTeam));
        if (fetchedMemberList.Count < maxTeamMembers)
        {
            updatingMembersText.text = "updating lobby...";
            current_UI = UI_Phase.UI_Lobby;
            teamInfo.text = selectedTeam;
            GoogleMap.groupName = selectedTeam;
            this.selectedTeam = selectedTeam;
            yield return StartCoroutine(JoinSelectedTeam());
            //if (MemberListChanged())
            //{
            //    PopulateMemberList(selectedTeam);
            //}
        }
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
        StartCoroutine(LeaveTeam());
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



