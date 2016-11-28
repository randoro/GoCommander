using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Canceler : MonoBehaviour {

    public void CallCancel()
    {
        SceneManager.LoadScene("mainScene");
    }
}
