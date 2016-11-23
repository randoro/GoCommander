using UnityEngine;
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
    public int refreshDelay = 10;

	void Start () {
        contentRT = GetComponent<RectTransform>();

        playerSpawnerObj = GameObject.FindGameObjectWithTag("PlayerSpawner");
        playerSpawner = playerSpawnerObj.GetComponent<PlayerSpawner>();
        playerList = playerSpawner.playerList;

        gMap = GameObject.FindGameObjectWithTag("Map").GetComponent<GoogleMap>();

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Orbit>().enabled = false;

        StartCoroutine(UpdatePlayers());
	}
    private void CreateListObjects()
    {
        PlayerHolder currPH;

        for (int i = 0; i < playerList.Count; i++)
        {
            currPH = playerList[i].GetComponent<PlayerHolder>();
            listObjects.Add(new ListItem(contentRT, currPH, gMap, new Vector2(0, contentRT.position.y + 100 * i), listGameObjects, this));
            Debug.Log(currPH.Name);
        }
    }
    //public void RestartTreasureSpawner()
    //{
    //    CommanderSpawner commanderSpawner = GameObject.FindGameObjectWithTag("CommanderSpawner").GetComponent<CommanderSpawner>();
    //    commanderSpawner.fetched = false;
    //    IEnumerator co = commanderSpawner.UpdateTreasures();
    //    StopCoroutine(co);
    //    StartCoroutine(co);
    //}
    IEnumerator UpdatePlayers()
    {
        while (true)
        {
            while (playerList.Count == 0)
            {
                yield return null;
            }

            listObjects.Clear();
            for (int i = 0; i < listGameObjects.Count; i++)
            {
                Destroy(listGameObjects[i]);
            }
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
        private GameObject clickItem, treasureCaller;
        private ListController listController;
        private GoogleMap gMap;
        private PlayerHolder player;
        public ListItem(RectTransform contentRT, PlayerHolder player, GoogleMap gMap, Vector2 pos, List<GameObject> listGameObjects, ListController treasureCaller)
        {
            clickItem = new GameObject();
            listGameObjects.Add(clickItem);

            RectTransform buttonRT = clickItem.AddComponent<RectTransform>();
            buttonRT.SetParent(contentRT);
            Vector2 canvasScale = new Vector2(contentRT.lossyScale.x, contentRT.lossyScale.y);

            buttonRT.sizeDelta = Vector2.Scale(new Vector2(500.0f, 75.0f), canvasScale);
            buttonRT.position = Vector2.Scale(pos, canvasScale);
            buttonRT.pivot = contentRT.pivot;

            contentRT.sizeDelta -= new Vector2(0, buttonRT.sizeDelta.y);

            Button buttonBU = clickItem.AddComponent<Button>();
            buttonBU.onClick.AddListener(() => ButtonClicked());
            buttonBU.colors = ColorBlock.defaultColorBlock;

            Text text = clickItem.AddComponent<Text>();
            text.text = player.Name;
            text.color = Color.black;
            text.fontSize = 20 * (int)canvasScale.x * 2;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            //Image buttonIM = clickItem.AddComponent<Image>();
            //buttonIM.sprite = Resources.Load("UISprite", typeof(Sprite)) as Sprite;

            this.gMap = gMap;
            this.player = player;
            listController = treasureCaller;
        }
        public void ButtonClicked()
        {
            Debug.Log("Button clicked");

            //gps.StopRefreshing();

            //listController.RestartTreasureSpawner();

            GoogleMap.centerLocation.address = "";
            GoogleMap.centerLocation.latitude = (float)player.lat;
            Debug.Log(player.lat);
            GoogleMap.centerLocation.longitude = (float)player.lng;
            Debug.Log(player.lng);

            gMap.Refresh();

            GameObject.FindGameObjectWithTag("MemberCanvas").SetActive(false);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Orbit>().enabled = true;
        }
    }
}