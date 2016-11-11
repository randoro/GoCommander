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

        //StartCoroutine(DownloadImage("http://img14.deviantart.net/706d/i/2014/022/2/4/grunge_texture_overlay_png_by_fictionchick-d73ass0.png"));
    }

    public void LoadBackgroundImage()
    {
        int question = Manager.randomQuestion;
        string images = Manager.allQuestionsList[question].image;       
        images = images.Substring(1);
        print(question);
        StartCoroutine(DownloadImage(images));

        //if (Manager.randomQuestion == 0)
        //{
        //    //background.sprite = Resources.Load<Sprite>("torso") as Sprite;
        //    StartCoroutine(DownloadImage("http://architectism.com/wp-content/uploads/2011/12/Turning-Torso-Malmo-5.jpg"));
        //}
        //else if (Manager.randomQuestion == 1)
        //{
        //    background.sprite = Resources.Load<Sprite>("Liseberg") as Sprite;
        //}
        //else if (Manager.randomQuestion == 2)
        //{
        //    background.sprite = Resources.Load<Sprite>("öresundsbron") as Sprite;
        //}
        //else
        //{
        //    background.sprite = Resources.Load<Sprite>("sunnyday") as Sprite;
        //}
    }

    IEnumerator DownloadImage(string _url)
    {
        //print(_url);
        WWW www = new WWW(_url);
        Texture2D texture = new Texture2D(1, 1, TextureFormat.DXT1, false);
        
        yield return www;
        www.LoadImageIntoTexture(texture);

        Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        background.sprite = image;
    }
}
