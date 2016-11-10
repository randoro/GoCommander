using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListController : MonoBehaviour {

    private List<ListItem> listObjects;
    RectTransform contentRT;
    GameObject playerSpawnerObj;
    public List<GameObject> playerList;

    private GoogleMap gMap;
    private GPSController gps;

	void Start () {
        contentRT = this.GetComponent<RectTransform>();
        playerSpawnerObj = GameObject.FindGameObjectWithTag("PlayerSpawner");
        PlayerSpawner playerSpawner = playerSpawnerObj.GetComponent<PlayerSpawner>();
        playerList = playerSpawner.playerList;

        gMap = GameObject.FindGameObjectWithTag("Map").GetComponent<GoogleMap>();
	}
	
	void Update () {
	}

    public class ListItem
    {
        private GameObject clickItem;
        private ListController listController;
        private GoogleMap gMap;
        public ListItem(RectTransform contentRT, string username, ListController listController, GoogleMap gMap)
        {
            clickItem = new GameObject();

            RectTransform buttonRT = clickItem.AddComponent<RectTransform>();
            buttonRT.SetParent(contentRT);
            buttonRT.sizeDelta = new Vector2(200.0f, 100.0f);

            this.listController = listController;

            Button buttonBU = clickItem.AddComponent<Button>();
            buttonBU.onClick.AddListener(() => buttonClicked());

            Image buttonIM = clickItem.AddComponent<Image>();
            buttonIM.sprite = Resources.Load("UISprite", typeof(Sprite)) as Sprite;

            GUIText text = clickItem.AddComponent<GUIText>();
            text.text = username;

            this.gMap = gMap;
        }
        public void buttonClicked()
        {
            Debug.Log("Button clicked");
            float lat_value = 0;
            float long_value = 0;

            //gps.StopRefreshing();

            GoogleMap.centerLocation.address = "";
            GoogleMap.centerLocation.latitude = lat_value;
            GoogleMap.centerLocation.longitude = long_value;
            gMap.Refresh();
        }
    }
    
}