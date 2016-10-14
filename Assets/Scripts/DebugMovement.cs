using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugMovement : MonoBehaviour {

    public GameObject lat_inputField;
    public GameObject long_inputField;

    string lat_value;
    string long_value;

    // Use this for initialization
    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        lat_value = lat_inputField.GetComponent<InputField>().text;
        long_value = long_inputField.GetComponent<InputField>().text;
    }
}
