using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class TreasureSpawner : MonoBehaviour {


    public bool autoRefresh = true;
    public int refreshDelay = 20;

    private string[] nav;
    public List<GameObject> treasureList;
    List<TreasureList> fetchedList;

    // Use this for initialization
    void Start () {

        treasureList = new List<GameObject>();
        fetchedList = new List<TreasureList>();

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



        for (int i = treasureList.Count; i-- > 0;)
        {
            if (!fetchedList.Exists(x => x.lat == treasureList[i].gameObject.transform.position.x && x.lng == treasureList[i].gameObject.transform.position.z))
            {
                //removing old objects
                Destroy(treasureList[i].gameObject);
                treasureList.RemoveAt(i);

            }
            else
            {
                //removing copies
                fetchedList.Remove(fetchedList.Find(x => x.lat == treasureList[i].gameObject.transform.position.x && x.lng == treasureList[i].gameObject.transform.position.z));
            }
        }


        foreach (TreasureList v in fetchedList)
        {
            //adding the new
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Meshes/TreasureChest.fbx", typeof(GameObject));
            GameObject newPlayer = (GameObject)Instantiate(prefab, new Vector3(v.lat - GPSController.latitude, 0.0f, v.lng - GPSController.longitude), Quaternion.Euler(new Vector3(0, Random.value * 360, 0)));
            newPlayer.transform.parent = gameObject.transform;
            newPlayer.transform.localScale = new Vector3(6, 6, 6);
            treasureList.Add(newPlayer);

        }







        yield return null;
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

            fetchedList.Add(new TreasureList(id, lat, lng));
        }
        print(fetchedList.Count);
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
