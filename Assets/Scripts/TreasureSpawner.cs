using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

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

    private const double radius = 0.003;

    public static bool testIfRadiusCheckWorks = false;

    // Use this for initialization
    void Start ()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();

        GameObject playerArea = GameObject.FindGameObjectWithTag("PlayerArea");
        playerArea.transform.localScale = new Vector3(coordScaleToGameScale(radius * 2, 90, 6.5f), 0, coordScaleToGameScale(radius * 2, 90, 6.5f)); 

        treasureList = new List<GameObject>();
        fetchedList = new List<Treasure>();

        StartCoroutine(UpdateTreasures());
    }

    void Update()
    {
        //print(GoogleMap.groupName);
    }

    IEnumerator UpdateTreasures()
    {
        while (true)
        {
            StartCoroutine(GetTreasuresAndGroupScore());

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
                TreasureHolder newTreasureHolder = ((GameObject)Instantiate(prefab,
                            new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                                0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f)),
                            Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 360, 0)))).GetComponent<TreasureHolder>();
                newTreasureHolder.Initialize(v.id, v.lat, v.lng, v.type, v.visible);
                newTreasureHolder.gameObject.transform.localScale = new Vector3(4f, 4f, 4f);
                treasureList.Add(newTreasureHolder.gameObject);
                //StartCoroutine(SetAsVisible(v.id));

            }
            fetched = false;
            yield return new WaitForSeconds(refreshDelay);
        }    
    }
    //IEnumerator SetAsVisible(int id)
    //{
    //    string IDsendURL = "http://gocommander.sytes.net/scripts/visible_treasure_locations.php";

    //    WWWForm form = new WWWForm();
    //    form.AddField("treasureidPost", id);
    //    WWW www = new WWW(IDsendURL, form);

    //    yield return www;
    //}
    private float coordScaleToGameScale(double inFloat, double total, float multi)
    {
        float returnfloat = (float)((inFloat / total) * (multi * (double)Math.Pow(2, gMap.zoom)));
        return returnfloat;
    }

    public void GenerateNewTreasures()
    {
        StartCoroutine(GenerateTreasures());
    }
    IEnumerator GetTreasuresAndGroupScore()
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
            int visible = int.Parse(GetDataValue(nav[i], "Visible:"));

            if (WithinRadiusLatLng(lng, lat, GoogleMap.centerLocation.longitude, GoogleMap.centerLocation.latitude) || visible == 1)
            {
                int id = int.Parse(GetDataValue(nav[i], "ID:"));
                int type = int.Parse(GetDataValue(nav[i], "Type:"));

                fetchedList.Add(new Treasure(id, lat, lng, type, visible));
            }
            //if (!testIfRadiusCheckWorks)
            //{
                
            //    if (WithinRadiusLatLng(lng, lat, GoogleMap.centerLocation.longitude, GoogleMap.centerLocation.latitude))
            //    {
            //        int id = int.Parse(GetDataValue(nav[i], "ID:"));
            //        int type = int.Parse(GetDataValue(nav[i], "Type:"));

            //        fetchedList.Add(new Treasure(id, lat, lng, type, visible));
            //    }
            //}
            //else
            //{
            //    int id = int.Parse(GetDataValue(nav[i], "ID:"));
            //    int type = int.Parse(GetDataValue(nav[i], "Type:"));

            //    fetchedList.Add(new Treasure(id, lat, lng, type, visible));
            //}
            
        }
        fetched = true;

        // Note(Calle): So this is the score part, right here, starts here
        string scoreURL = "http://gocommander.sytes.net/scripts/score_get_group.php";

        form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        www = new WWW(scoreURL, form);
        yield return www;
        result = www.text;

        GoogleMap.groupScore = int.Parse(result);
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
        for (int i = treasureList.Count; i-- > 0;)
        {
            TreasureHolder tempTres = treasureList[i].gameObject.GetComponent<TreasureHolder>();
            Treasure v = tempTres.treasure;
            treasureList[i].transform.position =
                new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                    0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f));
        }
    }


    public void RemoveTreasure(int id, int type)
    {
        for (int i = treasureList.Count; i-- > 0;)
        {
            TreasureHolder tempTres = treasureList[i].gameObject.GetComponent<TreasureHolder>();
            Treasure v = tempTres.treasure;
            int itsId = v.id;
            if (itsId == id)
            {   
                StartCoroutine(RemoveChosenTreasure(id, type, tempTres));
            }
        }
    }

    IEnumerator RemoveChosenTreasure(int id, int type, TreasureHolder treasure_holder)
    {
        Destroy(treasure_holder.gameObject);

        string removeChosenTreasure = "http://gocommander.sytes.net/scripts/remove_chosen_treasure.php";

        WWWForm form = new WWWForm();
        form.AddField("treasurePost", id); // Här ska det vara treasure id

        WWW www = new WWW(removeChosenTreasure, form);

        yield return www;

        switch (type)
        {
            case 0:
                print("loading new scene");
                SceneManager.LoadScene("MinigameMemory");
                break;
            case 1:
                print("loading new scene");
                SceneManager.LoadScene("MinigamePuzzle");
                break;
            case 2:
                print("loading new scene");
                SceneManager.LoadScene("MinigameQuiz");
                break;
            case 3:
                print("loading new scene");
                SceneManager.LoadScene("MinigameSprint");
                break;
            default:
                print("loading new scene");
                SceneManager.LoadScene("MinigameMemory");
                break;
        }
    }

    public void StartRemoveUserTreasures()
    {
        StartCoroutine(RemoveUserTreasures());
    }

    public IEnumerator RemoveUserTreasures()
    {
        string removeChosenTreasure = "http://gocommander.sytes.net/scripts/remove_all_treasure.php";

        WWWForm form = new WWWForm();
        form.AddField("useridPost", GoogleMap.id); // AnvändarID

        WWW www = new WWW(removeChosenTreasure, form);

        yield return www;
    }


    bool WithinRadiusLatLng(double treasureLng, double treasureLat, double playerLng, double playerLat)
    {
        Vector2 playerLngLat = new Vector2((float)playerLng / 2, (float)playerLat);
        Vector2 treasureLngLat = new Vector2((float)treasureLng / 2, (float)treasureLat);

        if (Vector2.Distance(playerLngLat, treasureLngLat) <= radius - 0.002)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
