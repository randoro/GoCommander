using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerSpawner : MonoBehaviour {


    public bool autoRefresh = true;
    public int refreshDelay = 20;

    private string[] nav;
    public GameObject prefab;
    public List<GameObject> playerList;
    public static List<Player> fetchedList;

    private GameObject map;
    private GoogleMap gMap;

    private float lastTime = 0;
    private bool fetched;


    // Use this for initialization
    void Start () {

        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();

        playerList = new List<GameObject>();
        fetchedList = new List<Player>();

        StartCoroutine(UpdatePlayers());
    }

    IEnumerator UpdatePlayers()
    {
        while (true)
        {
            StartCoroutine(GetPlayers());

            while (!fetched)
            {
                yield return null;
            }


            //fetchedList.Add(new Treasure(0, 55.590137, 12.995480));

            for (int i = playerList.Count; i-- > 0; )
            {

                PlayerHolder tempTres = playerList[i].gameObject.GetComponent<PlayerHolder>();

                if (!fetchedList.Exists(x => x.lat.Equals(tempTres.player.lat) && x.lng.Equals(tempTres.player.lng)))
                {
                    //removing old objects
                    Destroy(playerList[i].gameObject);
                    playerList.RemoveAt(i);

                }
                else
                {
                    //removing copies
                    fetchedList.Remove(
                        fetchedList.Find(x => x.lat.Equals(tempTres.player.lat) && x.lng.Equals(tempTres.player.lng)));
                }
            }
            //print(coordScaleToGameScale(180));


            foreach (Player v in fetchedList)
            {
                //adding the new
                PlayerHolder newTreasureHolder = ((GameObject)Instantiate(prefab,
                            new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                                0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f)),
                            Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 360, 0)))).GetComponent<PlayerHolder>();
                newTreasureHolder.Initialize(v.id, v.name, v.lat, v.lng);
                newTreasureHolder.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                playerList.Add(newTreasureHolder.gameObject);

            }


            fetched = false;
            yield return new WaitForSeconds(refreshDelay);
        }

    }




    private float coordScaleToGameScale(double inFloat, double total, float multi)
    {
        float returnfloat = (float)((inFloat / total) * (multi * (double)Math.Pow(2, gMap.zoom)));
        return returnfloat;
    }


    IEnumerator GetPlayers()
    {
        string playerURL = "http://gocommander.sytes.net/scripts/get_player_locations.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        form.AddField("userGroupPost", GoogleMap.groupName);

        WWW www = new WWW(playerURL, form);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            nav = result.Split(';');
        }

        fetchedList.Clear();

        for (int i = 0; i < nav.Length - 1; i++)
        {
            int id = int.Parse(GetDataValue(nav[i], "ID:"));
            string username = GetDataValue(nav[i], "Username:");
            double lat = double.Parse(GetDataValue(nav[i], "Latitude:"));
            double lng = double.Parse(GetDataValue(nav[i], "Longitude:"));
            string message = GetDataValue(nav[i], "Minimessage:");

            if(message.Equals(""))
            {
                fetchedList.Add(new Player(id, username, lat, lng));
            }
            else
            {
                fetchedList.Add(new Player(id, username, lat, lng, message));             
            }
        }
        fetched = true;
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    public void UpdatePlayerLocations()
    {
        for (int i = playerList.Count; i-- > 0; )
        {
            PlayerHolder tempTres = playerList[i].gameObject.GetComponent<PlayerHolder>();
            Player v = tempTres.player;
            playerList[i].transform.position =
                new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                    0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f));

        }
    }



    
}
