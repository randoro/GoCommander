using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SprintHelpMessage : MonoBehaviour {

    public GameObject help_UI;
    public Button ok_button;

    int timer = 550;

    static bool showMessage;

    //static bool stopShowingMessage;

    // Use this for initialization
    void Awake()
    {
        SprintHelpMessage.showMessage = true;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer--;

        if (timer < 1 && showMessage)
        {
            help_UI.SetActive(false);
            Destroy(help_UI);
            SprintHelpMessage.showMessage = false;
        }
    }

    public void OkClick()
    {
        help_UI.SetActive(false);
        Destroy(help_UI);
        SprintHelpMessage.showMessage = false;
    }
}
