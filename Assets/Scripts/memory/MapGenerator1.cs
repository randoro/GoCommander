using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator1 : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;

    [Range(0,1)]
    public float outLinePercent;

    List<Coordinate> tileCoordinates;
    Queue<Coordinate> shuffledCoordinates;

    public int shuffleSeeed;

    //Random.Range (1,10000)
    void Start()
    {
       shuffleSeeed = Random.Range(1, 10000);

        GenerateMap();
    }

    public void GenerateMap()
    {
        tileCoordinates = new List<Coordinate>();

        for(int x = 0; x < mapSize.x; x++)
            for(int y = 0; y < mapSize.y; y++)
            {
                tileCoordinates.Add(new Coordinate(x, y));
            }

        shuffledCoordinates = new Queue<Coordinate>(Utility1.ShuffleArray(tileCoordinates.ToArray(), shuffleSeeed));

        string holderName = "Generated Map1";

        if(transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = this.transform;

        for(int x = 0; x < mapSize.x; x++)
            for(int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePos = CoordToVector(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outLinePercent);
                newTile.parent = mapHolder;      
            }

        int obstacleCount = 10;
        for(int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomizeCoord = GetRandomCoordinates();
            Vector3 obstaclePos = CoordToVector(randomizeCoord.x, randomizeCoord.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
        }
    }

    public Vector3 CoordToVector(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    public Coordinate GetRandomCoordinates()
    {
        Coordinate randomCoordinate = shuffledCoordinates.Dequeue();
        shuffledCoordinates.Enqueue(randomCoordinate);
        return randomCoordinate;
    }

    public struct Coordinate
    {
       public int x;
       public int y;
       
    public Coordinate(int x, int y)
    {
            this.x = x;
            this.y = y;
    }
    }
}
