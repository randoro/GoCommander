using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CommanderSpawner : MonoBehaviour
{

    public bool autoRefresh = true;
    public int refreshDelay = 5;
    public GameObject prefab;

    private string[] nav;
    public List<GameObject> treasureList;
    public List<Treasure> fetchedList;
    private GameObject map;
    private GoogleMap gMap;

    private float lastTime = 0;
    public bool fetched;

    private const double radius = 0.003;

    // Use this for initialization
    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();

        treasureList = new List<GameObject>();
        fetchedList = new List<Treasure>();

        StartCoroutine(UpdateTreasures());
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

            for (int i = treasureList.Count; i-- > 0; )
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
                TreasureHolder newTreasureHolder = ((GameObject)Instantiate(prefab,
                            new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                                0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f)),
                            Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 360, 0)))).GetComponent<TreasureHolder>();
                newTreasureHolder.Initialize(v.id, v.lat, v.lng, v.type, v.visible);
                newTreasureHolder.gameObject.transform.localScale = new Vector3(4f, 4f, 4f);
                treasureList.Add(newTreasureHolder.gameObject);
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

    public void GenerateNewTreasures()
    {
        StartCoroutine(GenerateTreasures());
    }

    IEnumerator GetTreasures()
    {
        string treasureURL = "http://gocommander.sytes.net/scripts/treasure_locations.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        WWW www = new WWW(treasureURL, form);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            nav = result.Split(';');
        }

        fetchedList.Clear();

        for (int i = 0; i < nav.Length - 1; i++)
        {
            double lat = double.Parse(GetDataValue(nav[i], "Latitude:"));
            double lng = double.Parse(GetDataValue(nav[i], "Longitude:"));

            if (OutsideRadiusLatLng(lng, lat, GoogleMap.centerLocation.longitude, GoogleMap.centerLocation.latitude))
            {
                int visible = int.Parse(GetDataValue(nav[i], "Visible:"));
                int id = int.Parse(GetDataValue(nav[i], "ID:"));
                int type = int.Parse(GetDataValue(nav[i], "Type:"));

                fetchedList.Add(new Treasure(id, lat, lng, type, visible));
            }
        }
        fetched = true;
    }

    IEnumerator GenerateTreasures()
    {
        string treasureURL = "http://gocommander.sytes.net/scripts/treasure_spawn.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        form.AddField("userLatPost", GoogleMap.centerLocation.latitude.ToString());
        form.AddField("userLongPost", GoogleMap.centerLocation.longitude.ToString());

        WWW www = new WWW(treasureURL, form);
        yield return www;
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
        for (int i = treasureList.Count; i-- > 0; )
        {
            TreasureHolder tempTres = treasureList[i].gameObject.GetComponent<TreasureHolder>();
            Treasure v = tempTres.treasure;
            treasureList[i].transform.position =
                new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                    0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f));
        }
    }


    public void RemoveTreasure(int id)
    {
        for (int i = treasureList.Count; i-- > 0; )
        {
            TreasureHolder tempTres = treasureList[i].gameObject.GetComponent<TreasureHolder>();
            Treasure v = tempTres.treasure;
            int itsId = v.id;
            if (itsId == id)
            {
                Destroy(tempTres.gameObject);
                StartCoroutine(RemoveChosenTreasure(id));
            }

        }
    }

    IEnumerator RemoveChosenTreasure(int id)
    {
        string removeChosenTreasure = "http://gocommander.sytes.net/scripts/remove_chosen_treasure.php"; // EJ AKTIV JUST NU

        WWWForm form = new WWWForm();
        form.AddField("treasurePost", id); // Här ska det vara treasure id

        WWW www = new WWW(removeChosenTreasure, form);

        yield return www;
    }
    bool OutsideRadiusLatLng(double treasureLng, double treasureLat, double playerLng, double playerLat)
    {
        Vector2 playerLngLat = new Vector2((float)playerLng / 2, (float)playerLat);
        Vector2 treasureLngLat = new Vector2((float)treasureLng / 2, (float)treasureLat);

        if (Vector2.Distance(playerLngLat, treasureLngLat) > radius - 0.002)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
