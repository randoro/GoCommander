using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login_UI_Changer : MonoBehaviour {

    enum UI_Phase
    {
        UI_login,
        UI_help
    }
    static UI_Phase current_UI;

    public Canvas UI_login;
    public Canvas UI_help;

    public Button helpBtn;
    public Button exitBtn;
    public Button backBtn;

    // Use this for initialization
    void Start()
    {
        current_UI = UI_Phase.UI_login;
    }

    // Update is called once per frame
    void Update()
    {
        switch (current_UI)
        {
            case UI_Phase.UI_login:
                {
                    UI_login.enabled = true;
                    UI_help.enabled = false;
                }
                break;
            case UI_Phase.UI_help:
                {
                    UI_login.enabled = false;
                    UI_help.enabled = true;
                }
                break;
        }
    }

    public void HelpBtnClick()
    {
        current_UI = UI_Phase.UI_help;
    }

    public void ExitBtnClick()
    {
        Application.Quit();
    }

    public void BackBtnClick()
    {
        current_UI = UI_Phase.UI_login;
    }
}
