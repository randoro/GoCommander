using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {

    public GameObject UIBase;

    public GameObject startBtn;
    public GameObject leaveBtn;
    public GameObject memberList;
    public GameObject listElement;

    int amountOfMembers = 10;
    
    // Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void StartBtnClick()
    {
        SceneManager.LoadScene("mainScene");
    }

    public void LeaveBtnClick()
    {
        SceneManager.LoadScene("login");
    }
}
