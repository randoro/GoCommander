using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TreasureOrbs_UI : MonoBehaviour {

    public Button treasureOrbsBtn;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void TreasureOrbsBtnClick()
    {
        MainGame_UI_Changer.activeUI = MainGame_UI_Changer.ActiveUI.treasureOrbsUI;
    }

}
