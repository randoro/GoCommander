using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Image = UnityEngine.UI.Image;

public class LoadImage : MonoBehaviour {

    public Image background;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        LoadBackgroundImage();
    }

    public void LoadBackgroundImage()
    {
        if (Manager.randomQuestion == 0)
        {
            background.sprite = Resources.Load<Sprite>("torso") as Sprite;
        }
        else if (Manager.randomQuestion == 1)
        {
            background.sprite = Resources.Load<Sprite>("Liseberg") as Sprite;
        }
        else if (Manager.randomQuestion == 2)
        {
            background.sprite = Resources.Load<Sprite>("öresundsbron") as Sprite;
        }
        else
        {
            background.sprite = Resources.Load<Sprite>("sunnyday") as Sprite;
        }
    }
}
