using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Image = UnityEngine.UI.Image;

public class LoadImage : MonoBehaviour {

    public Image background;
    public static bool loadImage;

    // Use this for initialization
    void Start () {
        loadImage = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (loadImage)
        {
            LoadBackgroundImage();
            loadImage = false;
        }
    }

    public void LoadBackgroundImage()
    {
        int question = Manager.randomQuestion;
        string images = Manager.allQuestionsList[question].image;       
        images = images.Substring(1);
        //print(question);
        StartCoroutine(DownloadImage(images));
    }

    IEnumerator DownloadImage(string _url)
    {
        WWW www = new WWW(_url);
        Texture2D texture = new Texture2D(1, 1, TextureFormat.DXT1, false);
        
        yield return www;
        www.LoadImageIntoTexture(texture);

        Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        background.sprite = image;
    }
}
