using UnityEngine;
using System.Collections;

public class MainGame_UI_Changer : MonoBehaviour
{

    public enum ActiveUI
    {
        inGameUI,
        menuUI
    }
    public ActiveUI activeUI;

    public Canvas inGameUI;
    public Canvas menuUI;

    public GameObject menu_btn;
    public GameObject back_btn;

	// Use this for initialization
	void Start ()
    {
        activeUI = ActiveUI.inGameUI;
	}
	
	// Update is called once per frame
	void Update ()
    {
	  switch(activeUI)
        {
            case ActiveUI.inGameUI:
                {
                    inGameUI.enabled = true;
                    menuUI.enabled = false;
                }
                break;
            case ActiveUI.menuUI:
                {
                    inGameUI.enabled = false;
                    menuUI.enabled = true;

                }
                break;
        }
	}

    public void MenuBtnClick()
    {
        activeUI = ActiveUI.menuUI;
    }

    public void BackBtnClick()
    {
        activeUI = ActiveUI.inGameUI;
    }
}
