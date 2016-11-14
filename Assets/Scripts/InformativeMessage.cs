using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InformativeMessage : MonoBehaviour {

    public GameObject window;

    private void Start()
    {
        window.SetActive(false);
    }

    private void Update()
    {
        ShowCompletedMinigame();
    }

    public void ShowCompletedMinigame()
    {
        if(Manager.isGameCompleted)
        {
            window.SetActive(true);
        }
    }

    public void HideCompletedMinigame()
    {
        window.SetActive(false);
    }
}
