using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TreasureSpawner : MonoBehaviour {


    public bool autoRefresh = true;
    public int refreshDelay = 20;
    public GameObject prefab;



    private string[] nav;
    public List<GameObject> treasureList;
    List<Treasure> fetchedList;
    private GameObject map;
    private GoogleMap gMap;

    // Use this for initialization
    void Start () {

        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();

        treasureList = new List<GameObject>();
        fetchedList = new List<Treasure>();

        if (autoRefresh)
        {
            StartCoroutine(RefreshLoop(refreshDelay));
        }
    }

    IEnumerator RefreshLoop(float waitTime)
    {
        while (true)
        {
            StopCoroutine(UpdateTreasures());
            StartCoroutine(UpdateTreasures());
            yield return new WaitForSeconds(waitTime);
        }

    }

    IEnumerator UpdateTreasures()
    {
        ////For testing
        //GameObject yourPlayer = GameObject.FindGameObjectWithTag("Player");
        //Vector2 playerPos = new Vector2(yourPlayer.transform.position.x, yourPlayer.transform.position.z);
        //int offset = 32;
        //Vector2 mapCorner = new Vector2(playerPos.x - offset, playerPos.y - offset);
        ////Five random locations
        //fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        //fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        //fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        //fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        //fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));

        StartCoroutine(GetTreasures());

        print(fetchedList.Count);

        fetchedList.Add(new Treasure(0, 55.590137, 12.995480));

        for (int i = treasureList.Count; i-- > 0;)
        {

            TreasureHolder tempTres = treasureList[i].gameObject.GetComponent<TreasureHolder>();
            print("x => x.lat.Equals("+tempTres.treasure.lat+") && x.lng.Equals("+tempTres.treasure.lng+"))");

            if (!fetchedList.Exists(x => x.lat.Equals(tempTres.treasure.lat) && x.lng.Equals(tempTres.treasure.lng)))
            {
                //removing old objects
                Destroy(treasureList[i].gameObject);
                treasureList.RemoveAt(i);

            }
            else
            {
                //removing copies
                fetchedList.Remove(fetchedList.Find(x => x.lat.Equals(tempTres.treasure.lat) && x.lng.Equals(tempTres.treasure.lng)));
            }
        }
        //print(coordScaleToGameScale(180));
        

        foreach (Treasure v in fetchedList)
        {
            //adding the new

            print("v.lat "+v.lat+" latitude"+ gMap.centerLocation.latitude);
            print("v.lng " + v.lng + " longitude" + gMap.centerLocation.longitude);

            GameObject newPlayer = (GameObject)Instantiate(prefab, new Vector3(coordScaleToGameScale(v.lng - gMap.centerLocation.longitude, 180.0f, 10.0f), 0.0f, coordScaleToGameScale(v.lat - gMap.centerLocation.latitude, 90.0f, 9.0f)), Quaternion.Euler(new Vector3(0, Random.value * 360, 0)));
            newPlayer.transform.parent = gameObject.transform;
            newPlayer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newPlayer.gameObject.GetComponent<TreasureHolder>().treasure = v;
            treasureList.Add(newPlayer);

        }



        



        yield return null;
    }





    private float coordScaleToGameScale(float inFloat, float total, float multi)
    {
        float returnfloat = (inFloat/total) * (multi * (float)Math.Pow(2, gMap.zoom));
        return returnfloat;
    }


    static double NthRoot(double A, int N)
    {
        return Math.Pow(A, 1.0 / N);
    }


    IEnumerator GetTreasures()
    {
        string treasureURL = "https://ddwap.mah.se/AC3992/treasureSpawn.php";

        WWW www = new WWW(treasureURL);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            nav = result.Split(';');
        }

        for (int i = 0; i < nav.Length - 1; i++)
        {
            int id = int.Parse(GetDataValue(nav[i], "ID:"));
            double lat = double.Parse(GetDataValue(nav[i], "Latitude:"));
            double lng = double.Parse(GetDataValue(nav[i], "Longitude:"));

            fetchedList.Add(new Treasure(id, lat, lng));
        }
        //print(fetchedList.Count);
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
