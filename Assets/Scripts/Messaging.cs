using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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

    public Button player1Button;
    public Button player2Button;
    public Button player3Button;
    public Button player4Button;

    private string[] messageArray = new string[4];
    private string[] playerArray = new string[4];
    private string[] playerArrayTest = new string[4];
    private string[] teamArray = new string[10];

    LobbyData memberData;
    
    List<LobbyData> memberList = new List<LobbyData>();

    private int amountOfPlayers = 0;

    // Use this for initialization
    void Start () {
        
        StartCoroutine(GetMembersInTeam());

        SetMessages();
    }
	
	// Update is called once per frame
	void Update () {

        CheckAmountOfPlayers();

        SetPlayers();

        EnablePlayerButtons(player1Button, player1Name);
        EnablePlayerButtons(player2Button, player2Name);
        EnablePlayerButtons(player3Button, player3Name);
        EnablePlayerButtons(player4Button, player4Name);
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

    public void SetPlayers()
    {
        for (int i = 0; i < amountOfPlayers; i++)
        {
            if (memberList[i].name.Equals(GoogleMap.username))
            {
                memberList.Remove(memberList[i]);
            }

            playerArray[i] = memberList[i].name;
        }

        player1Name.text = playerArray[0];
        player2Name.text = playerArray[1];
        player3Name.text = playerArray[2];
        player4Name.text = playerArray[3];
    }

    void CheckAmountOfPlayers()
    {
        amountOfPlayers = memberList.Count;
    }

    public void EnablePlayerButtons(Button _b , Text _t)
    {
        if(_t.text.Equals(""))
        {
            _b.interactable = false;
        }
        _b.interactable = true;
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
        player1Name.text = playerArray[0];
        player2Name.text = playerArray[1];
        player3Name.text = playerArray[2];
        player4Name.text = playerArray[3];

        StartCoroutine(SendMessage());
    }

    string GetLobbyData(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
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

    IEnumerator GetMembersInTeam()
    {
        string getMembersURL = "http://gocommander.sytes.net/scripts/show_group_members.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
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
            if (memberList.Count < 10)
            {
                int id = int.Parse(GetLobbyData(teamArray[i], "ID:"));
                string member = GetLobbyData(teamArray[i], "Groupusers:");
                memberData = new LobbyData(id, member);
                memberList.Add(memberData);
            }
        }
    }
}
