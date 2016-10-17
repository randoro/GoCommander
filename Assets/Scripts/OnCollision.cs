using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OnCollision : MonoBehaviour {

    // Use this for initialization
    void OnCollisionEnter(Collision collision)
    {
        print("Collision Detected");
        SceneManager.LoadScene("MinigameMemory");
    }
}
