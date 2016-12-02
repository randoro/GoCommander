using UnityEngine;
using System.Collections;

public class MemberButton : MonoBehaviour {

    private Canvas memberListCanvas;
    private GameObject canvasGO;
    public GameObject helpUI;
    public GameObject okBtn;

    int helpTimer = 1000;

	// Use this for initialization
	void Start ()
    {
        canvasGO = GameObject.FindGameObjectWithTag("MemberCanvas");
        canvasGO.SetActive(false);
    }

    public void ButtonClicked()
    {
        //memberListCanvas = canvasGO.GetComponent<Canvas>();
        if (canvasGO.activeInHierarchy)
        {
            canvasGO.SetActive(false);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Orbit>().enabled = true;
        }
        else
        {
            canvasGO.SetActive(true);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Orbit>().enabled = false;
        }
    }

    public void OKBtnClicked()
    {
        helpUI.SetActive(false);
    }

    void Update()
    {
        helpTimer--;

        if (helpTimer < 1)
        {
            helpUI.SetActive(false);
        }
        
    }
}