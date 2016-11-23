using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Messaging : MonoBehaviour {

    public Text message1Text;
    public Text message2Text;
    public Text message3Text;
    public Text message4Text;

    private string[] messageList = new string[4];

    // Use this for initialization
    void Start () {

        InitializeMessages();
        SetMessages();
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
    }

    private void SetMessages()
    {
        message1Text.text = messageList[0];
        message2Text.text = messageList[1];
        message3Text.text = messageList[2];
        message4Text.text = messageList[3];
    }


    IEnumerator SendMessage(Text _stringToSend)
    {
        string messageToSend;
        messageToSend = _stringToSend.text;

        print(messageToSend);

        string mottagare = "milan";

        string loginUserURL = "http://gocommander.sytes.net/scripts/send_game_message.php";

        WWWForm form = new WWWForm();
        form.AddField("userRecPost", mottagare);
        form.AddField("usermessagePost", messageToSend);

        WWW www = new WWW(loginUserURL, form);

        yield return www;
    }
}
