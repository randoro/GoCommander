﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListController : MonoBehaviour {

    private List<ListItem> listObjects = new List<ListItem>();
    private RectTransform contentRT;
    private List<GameObject> playerList;
    private List<GameObject> listGameObjects = new List<GameObject>();
    private GameObject playerSpawnerObj;
    private PlayerSpawner playerSpawner;

    private GoogleMap gMap;
    private GPSController gps;
    public int refreshDelay = 20;

	void Start () {
        contentRT = this.GetComponent<RectTransform>();
        playerSpawnerObj = GameObject.FindGameObjectWithTag("PlayerSpawner");

        playerSpawner = playerSpawnerObj.GetComponent<PlayerSpawner>();
        playerList = playerSpawner.playerList;

        gMap = GameObject.FindGameObjectWithTag("Map").GetComponent<GoogleMap>();
        CreateListObjects();

        StartCoroutine(UpdatePlayers());
	}
    private void CreateListObjects()
    {
        PlayerHolder currPH;

        for (int i = 0; i < playerList.Count; i++)
        {
            currPH = playerList[i].GetComponent<PlayerHolder>();
            listObjects.Add(new ListItem(contentRT, currPH, gMap, new Vector2(100, 100 * -i), listGameObjects));
            Debug.Log(currPH.name);
        }
    }
    private void DestroyGameObjects()
    {
        for (int i = 0; i < listGameObjects.Count; i++)
        {
            Destroy(listGameObjects[i]);
        }
    }
    IEnumerator UpdatePlayers()
    {
        while (true)
        {
            listObjects.Clear();
            DestroyGameObjects();
            CreateListObjects();
            yield return new WaitForSeconds(refreshDelay);
        }
    }
	
    //void Update () {

    //    //playerList = playerSpawner.playerList;
    //    //if (playerList.Count == 0)
    //    //{
    //    //    Debug.Log("Shitballs");
    //    //}

    //    if (playerList != oldPlayerList)
    //    {
    //        Debug.Log("Hey it's happening!");
    //        listObjects.Clear();
    //        CreateListObjects();
    //    }
    //}

    public class ListItem
    {
        private GameObject clickItem;
        private ListController listController;
        private GoogleMap gMap;
        private PlayerHolder player;
        public ListItem(RectTransform contentRT, PlayerHolder player, GoogleMap gMap, Vector2 pos, List<GameObject> listGameObjects)
        {
            clickItem = new GameObject();
            listGameObjects.Add(clickItem);

            RectTransform buttonRT = clickItem.AddComponent<RectTransform>();
            buttonRT.SetParent(contentRT);
            buttonRT.sizeDelta = new Vector2(500.0f, 50.0f);
            buttonRT.position = pos;

            Button buttonBU = clickItem.AddComponent<Button>();
            buttonBU.onClick.AddListener(() => ButtonClicked());
            buttonBU.colors = ColorBlock.defaultColorBlock;

            Text text = clickItem.AddComponent<Text>();
            text.text = player.name;
            Debug.Log(player.name);
            text.color = Color.black;
            text.fontSize = 20;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            //Image buttonIM = clickItem.AddComponent<Image>();
            //buttonIM.sprite = Resources.Load("UISprite", typeof(Sprite)) as Sprite;

            this.gMap = gMap;
            this.player = player;
        }
        public void ButtonClicked()
        {
            Debug.Log("Button clicked");

            //gps.StopRefreshing();

            GoogleMap.centerLocation.address = "";
            GoogleMap.centerLocation.latitude = (float)player.lat;
            Debug.Log(player.lat);
            GoogleMap.centerLocation.longitude = (float)player.lng;
            Debug.Log(player.lng);
            gMap.Refresh();
        }
    }
}