﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TreasureSpawner : MonoBehaviour {


    public bool autoRefresh = true;
    public int refreshDelay = 5;
    public GameObject prefab;



    private string[] nav;
    public List<GameObject> treasureList;
    List<Treasure> fetchedList;
    private GameObject map;
    private GoogleMap gMap;

    private float lastTime = 0;
    private bool fetched;

    // Use this for initialization
    void Start () {

        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();

        treasureList = new List<GameObject>();
        fetchedList = new List<Treasure>();
        
        StartCoroutine(UpdateTreasures());
    }



    void Update()
    {
        
    }

    IEnumerator UpdateTreasures()
    {
        while (true)
        {
            StartCoroutine(GetTreasures());

            while (!fetched)
            {
                yield return null;
            }
            

            //fetchedList.Add(new Treasure(0, 55.590137, 12.995480));

            for (int i = treasureList.Count; i-- > 0;)
            {

                TreasureHolder tempTres = treasureList[i].gameObject.GetComponent<TreasureHolder>();

                if (!fetchedList.Exists(x => x.lat.Equals(tempTres.treasure.lat) && x.lng.Equals(tempTres.treasure.lng)))
                {
                    //removing old objects
                    Destroy(treasureList[i].gameObject);
                    treasureList.RemoveAt(i);

                }
                else
                {
                    //removing copies
                    fetchedList.Remove(
                        fetchedList.Find(x => x.lat.Equals(tempTres.treasure.lat) && x.lng.Equals(tempTres.treasure.lng)));
                }
            }
            //print(coordScaleToGameScale(180));


            foreach (Treasure v in fetchedList)
            {
                //adding the new
                GameObject newPlayer =
                    (GameObject)
                        Instantiate(prefab,
                            new Vector3(coordScaleToGameScale(v.lng - gMap.centerLocation.longitude, 180.0f, 10.0f),
                                0.0f, coordScaleToGameScale(v.lat - gMap.centerLocation.latitude, 90.0f, 9.0f)),
                            Quaternion.Euler(new Vector3(0, Random.value*360, 0)));
                newPlayer.transform.parent = gameObject.transform;
                newPlayer.transform.localScale = new Vector3(4f, 4f, 4f);
                newPlayer.gameObject.GetComponent<TreasureHolder>().treasure = v;
                treasureList.Add(newPlayer);

            }


            fetched = false;
            yield return new WaitForSeconds(refreshDelay);
        }
        
    }





    private float coordScaleToGameScale(float inFloat, float total, float multi)
    {
        float returnfloat = (inFloat/total) * (multi * (float)Math.Pow(2, gMap.zoom));
        return returnfloat;
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

        fetchedList.Clear();

        for (int i = 0; i < nav.Length - 1; i++)
        {
            int id = int.Parse(GetDataValue(nav[i], "ID:"));
            double lat = double.Parse(GetDataValue(nav[i], "Latitude:"));
            double lng = double.Parse(GetDataValue(nav[i], "Longitude:"));

            fetchedList.Add(new Treasure(id, lat, lng));
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

    public void UpdateTreasureLocations()
    {
        for (int i = treasureList.Count; i-- > 0;)
        {
            TreasureHolder tempTres = treasureList[i].gameObject.GetComponent<TreasureHolder>();
            Treasure v = tempTres.treasure;
            treasureList[i].transform.position =
                new Vector3(coordScaleToGameScale(v.lng - gMap.centerLocation.longitude, 180.0f, 10.0f),
                    0.0f, coordScaleToGameScale(v.lat - gMap.centerLocation.latitude, 90.0f, 9.0f));

        }
    }
}