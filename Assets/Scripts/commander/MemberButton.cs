using UnityEngine;
using System.Collections;

public class MemberButton : MonoBehaviour {

    private Canvas memberListCanvas;
    private GameObject canvasGO;

	// Use this for initialization
	void Start () {
        canvasGO = GameObject.FindGameObjectWithTag("MemberCanvas");
	}

    public void ButtonClicked()
    {
        //memberListCanvas = canvasGO.GetComponent<Canvas>();
        if (canvasGO.activeInHierarchy)
        {
            canvasGO.SetActive(false);
        }
        else
        {
            canvasGO.SetActive(true);
        }
    }
}
