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

    private string[] messageList = new string[4];
    private string[] playerList = new string[4];

    // Use this for initialization
    void Start () {

        InitializeMessages();
        SetMessages();
        SetPlayers();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void InitializeMessages()
    {
        messageList[0] = "Great work";
        messageList[1] = "Thank you";
        messageList[2] = "Hello";
        messageList[3] = "Hurry up";

        playerToMessage.text = "milan";
        messageToSend.text = "TEST";

        StartCoroutine(SendMessage());
    }

    private void SetMessages()
    {
        message1Text.text = messageList[0];
        message2Text.text = messageList[1];
        message3Text.text = messageList[2];
        message4Text.text = messageList[3];
    }

    private void SetPlayers()
    {
        playerList[0] = "milan";
        playerList[1] = "rasmus";
        playerList[2] = "player";
        playerList[3] = "player";

        player1Name.text = playerList[0];
        player2Name.text = playerList[1];
        player3Name.text = playerList[2];
        player4Name.text = playerList[3];
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

        StartCoroutine(SendMessage());
    }

    public IEnumerator SendMessage()
    {
        string message = messageToSend.text;
        string player = playerToMessage.text;

        print("Sent: " + message + "To player: " + player);
        
        //string mottagare = "milan";

        string loginUserURL = "http://gocommander.sytes.net/scripts/send_game_message.php";

        WWWForm form = new WWWForm();
        form.AddField("userRecPost", player);
        form.AddField("usermessagePost", message);

        WWW www = new WWW(loginUserURL, form);

        yield return www;

        print("Message is sent");
    }
}
