using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureSpawn : MonoBehaviour {

    private string[] nav;
    public List<Treasure> listTreasure = new List<Treasure>();

    // Use this for initialization
    void Start () {
        
        //StartCoroutine(GetTreasures());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //IEnumerator GetTreasures()
    //{   
    //    string treasureURL = "https://ddwap.mah.se/AC3992/treasureSpawn.php";

    //    WWW www = new WWW(treasureURL);
    //    yield return www;
    //    string result = www.text;

    //    if (result != null)
    //    {
    //        nav = result.Split(';');
    //    }

    //    for (int i = 0; i < nav.Length - 1; i++)
    //    {
    //        int id = int.Parse(GetDataValue(nav[i], "ID:"));
    //        double lat = double.Parse(GetDataValue(nav[i], "Latitude:"));
    //        double lng = double.Parse(GetDataValue(nav[i], "Longitude:"));

    //        listTreasure.Add(new TreasureList(id, lat, lng));
    //    }
    //    print(listTreasure.Count);
    //}

    //string GetDataValue(string data, string index)
    //{
    //    string value = data.Substring(data.IndexOf(index) + index.Length);
    //    if(value.Contains("|"))
    //        value = value.Remove(value.IndexOf("|"));
    //    return value;
    //}

}
