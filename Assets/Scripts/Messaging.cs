using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Messaging : MonoBehaviour {

    public Text socialMessage1Text;
    public Text socialMessage2Text;
    public Text socialMessage3Text;
    public Text socialMessage4Text;

    public Text tacticalMessage1Text;
    public Text tacticalMessage2Text;
    public Text tacticalMessage3Text;
    public Text tacticalMessage4Text;

    public Text helpfulMessage1Text;
    public Text helpfulMessage2Text;
    public Text helpfulMessage3Text;
    public Text helpfulMessage4Text;

    public Text additionalMessage1Text;
    public Text additionalMessage2Text;
    public Text additionalMessage3Text;
    public Text additionalMessage4Text;

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

    private string[] messageArray = new string[16];
    private string[] playerArray = new string[10];
    private string[] playerArrayTest = new string[10];
    private string[] teamArray = new string[10];

    LobbyData memberData;

    Message_UI ui;
       
    List<LobbyData> memberList = new List<LobbyData>();

    private int amountOfPlayers = 0;

    // Use this for initialization
    void Start () {

        ui = new Message_UI();

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

    //public void CheckMemberButtonClick()
    //{
    //    if(player1Name.text != " ")
    //    {
    //        AddMemberButtonListeners(player1Button, player1Name.text);
    //    }
    //    if (player2Name.text != " ")
    //    {
    //        AddMemberButtonListeners(player2Button, player2Name.text);
    //    }
    //    if (player3Name.text != " ")
    //    {
    //        AddMemberButtonListeners(player3Button, player3Name.text);
    //    }
    //    if (player4Name.text != " ")
    //    {
    //        AddMemberButtonListeners(player4Button, player4Name.text);
    //    }


    //}

    //public void AddMemberButtonListeners(Button button, string ID)
    //{
    //    button.onClick.AddListener(delegate { ui.ToCategories(ID); });
    //}

    private void SetMessages()
    {
        //Social
        messageArray[0] = "Hello!";
        messageArray[1] = "Yes";
        messageArray[2] = "No";
        messageArray[3] = "Have a nice day!";

        //Tactical
        messageArray[4] = "I need guidance!";
        messageArray[5] = "Take the Commander Badge!";
        messageArray[6] = "Do you need help?";
        messageArray[7] = "Hurry up!";


        //Helpful
        messageArray[8] = "Go North!";
        messageArray[9] = "Go East!";
        messageArray[10] = "Go West!";
        messageArray[11] = "Go South!";

        //Additonal
        messageArray[12] = "I am the best!";
        messageArray[13] = "You are the best!";
        messageArray[14] = "Well played!";
        messageArray[15] = "Good game!";

        

        socialMessage1Text.text = messageArray[0];
        socialMessage2Text.text = messageArray[1];
        socialMessage3Text.text = messageArray[2];
        socialMessage4Text.text = messageArray[3];

        tacticalMessage1Text.text = messageArray[4];
        tacticalMessage2Text.text = messageArray[5];
        tacticalMessage3Text.text = messageArray[6];
        tacticalMessage4Text.text = messageArray[7];

        helpfulMessage1Text.text = messageArray[8];
        helpfulMessage2Text.text = messageArray[9];
        helpfulMessage3Text.text = messageArray[10];
        helpfulMessage4Text.text = messageArray[11];

        additionalMessage1Text.text = messageArray[12];
        additionalMessage2Text.text = messageArray[13];
        additionalMessage3Text.text = messageArray[14];
        additionalMessage4Text.text = messageArray[15];

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
        
        if(_b.interactable)
        {
            _b.onClick.AddListener(delegate { ui.ToCategories(_t.text); }); 
        }
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
