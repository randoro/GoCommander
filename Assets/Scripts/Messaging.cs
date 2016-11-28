using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Messaging : MonoBehaviour {

    public Text message1Text;
    public Text message2Text;
    public Text message3Text;
    public Text message4Text;

    public Text player1Name;
    public Text player2Name;
    public Text player3Name;
    public Text player4Name;

    private Text playerToMessage;
    private Text messageToSend;

    private string[] messageArray = new string[4];
    private string[] playerArray = new string[4];
    private string[] playerArrayTest = new string[4];
    private string[] teamArray = new string[10];

    // Use this for initialization
    void Start () {

        SetMessages();
        SetPlayers();

        GetTeamsFromServer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SetMessages()
    {
        messageArray[0] = "Great work!";
        messageArray[1] = "Thank you!";
        messageArray[2] = "Hello!";
        messageArray[3] = "Hurry up!";

        message1Text.text = messageArray[0];
        message2Text.text = messageArray[1];
        message3Text.text = messageArray[2];
        message4Text.text = messageArray[3];
    }

    private void SetPlayers()
    {
        playerArray[0] = "Milan";
        playerArray[1] = "Rasmus";
        playerArray[2] = "player";
        playerArray[3] = "player";

        player1Name.text = playerArray[0];
        player2Name.text = playerArray[1];
        player3Name.text = playerArray[2];
        player4Name.text = playerArray[3];
    }

    public void SetPlayerToMessage(Text _playerToMessage)
    {
        playerToMessage = _playerToMessage;

        print(playerToMessage.text);
    }

    public void SetMessageToSend(Text _messageToSend)
    {
        messageToSend = _messageToSend;

        print(messageToSend.text);
        
    }

    public void SendToServer()
    {
        StartCoroutine(SendMessage());
    }

    public IEnumerator SendMessage()
    {
        string message = messageToSend.text;
        string player = playerToMessage.text;

        print("Sent: " + message + "To player: " + player);

        string loginUserURL = "http://gocommander.sytes.net/scripts/send_game_message.php";

        WWWForm form = new WWWForm();
        form.AddField("userRecPost", player);
        form.AddField("userMessagePost", GoogleMap.username + ": " + message);

        WWW www = new WWW(loginUserURL, form);

        yield return www;

        print("Message is sent");
    }

    IEnumerator GetTeamsFromServer()
    {
        print("GETTING TEAMS FROM SERVER");

        string getTeamsURL = "http://gocommander.sytes.net/scripts/show_group.php";

        WWW www = new WWW(getTeamsURL);

        yield return www;

        string result = www.text;

        print("TEAM RESULT: " + result);

        if (result != null)
        {
            teamArray = result.Split(';');

            print("TEAMARRAY: " + teamArray);
        }

        int id = 0;
        for (int i = 0; i < teamArray.Length - 1; i++)
        {
            //string teamName = GetLobbyData(teamArray[i], "Groupname:");
            //id++;
            //teamData = new LobbyData(id, teamName);
            //teamList.Add(teamData);
        }
    }

    IEnumerator GetMembersInTeam(string selectedTeam)
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/show_group_members.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", "Killerbunnies");
        WWW www = new WWW(getMembersURL, form);

        yield return www;

        string result = www.text;

        print(result);

        if (result != null)
        {
            teamArray = result.Split(';');
        }

        //for (int i = 0; i < teamArray.Length - 1; i++)
        //{
        //    if (memberList.Count < 10)
        //    {
        //        int id = int.Parse(GetLobbyData(teamArray[i], "ID:"));
        //        string member = GetLobbyData(teamArray[i], "Groupusers:");
        //        memberData = new LobbyData(id, member);
        //        memberList.Add(memberData);
        //    }
        //}
    }
}
