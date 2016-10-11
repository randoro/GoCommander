using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PlayerSpawner : MonoBehaviour {


    public bool autoRefresh = true;
    public int refreshDelay = 20;

    public List<GameObject> playerList;

    // Use this for initialization
    void Start () {

        playerList = new List<GameObject>();

        if (autoRefresh)
        {
            StartCoroutine(RefreshLoop(refreshDelay));
        }
    }

    IEnumerator RefreshLoop(float waitTime)
    {
        while (true)
        {
            StopCoroutine(UpdatePlayers());
            StartCoroutine(UpdatePlayers());
            yield return new WaitForSeconds(waitTime);
        }

    }

    IEnumerator UpdatePlayers()
    {
        //For testing
        List<Vector2> fetchedList = new List<Vector2>();
        GameObject yourPlayer = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPos = new Vector2(yourPlayer.transform.position.x, yourPlayer.transform.position.z);
        int offset = 32;
        Vector2 mapCorner = new Vector2(playerPos.x - offset, playerPos.y - offset);
        //Five random locations
        fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));
        fetchedList.Add(new Vector2(Random.value * (offset + offset) + mapCorner.x, Random.value * (offset + offset) + mapCorner.y));


        for (int i = playerList.Count; i-- > 0;)
        {
            if (!fetchedList.Contains(new Vector2(playerList[i].gameObject.transform.position.x, playerList[i].gameObject.transform.position.z)))
            {
                //removing old objects
                Destroy(playerList[i].gameObject);
                playerList.RemoveAt(i);

            }
            else
            {
                //removing copies
                fetchedList.Remove(new Vector2(playerList[i].gameObject.transform.position.x, playerList[i].gameObject.transform.position.z));
            }
        }


        foreach (Vector2 v in fetchedList)
        {
            //adding the new
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/player.prefab", typeof(GameObject));
            GameObject newPlayer = (GameObject)Instantiate(prefab, new Vector3(v.x, 0.86f, v.y), Quaternion.identity);
            newPlayer.transform.parent = gameObject.transform;
            playerList.Add(newPlayer);

        }







        yield return null;
    }
}
