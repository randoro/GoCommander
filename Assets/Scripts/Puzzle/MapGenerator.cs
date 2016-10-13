﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform circlePrefab;
    public Vector2 mapSize;
    public int shuffleSeeed;
    int circleCount;

    [Range(0,1)]
    public float gridLinePercent;

    public List<Coordinate> tileCoordinates;
    Queue<Coordinate> circleCoordinates;

    void Start()
    {
        circleCount = Random.Range(5,15);
        shuffleSeeed = Random.Range(0, 9999);
        gridLinePercent = 0.3f;

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

        circleCoordinates = new Queue<Coordinate>(Utility.ShuffleArray(tileCoordinates.ToArray(), shuffleSeeed));

        string holderName = "Generated Map";
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
                Transform newTile = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right)) as Transform;
                newTile.localScale = Vector3.one * (1 - gridLinePercent);
                newTile.parent = mapHolder;      
            }
        
        for(int i = 0; i < circleCount; i++)
        {
            Coordinate randomizeCoord = GetRandomCoordinates();
            Vector3 circlePos = CoordToVector(randomizeCoord.x, randomizeCoord.y);
            Transform newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
            newCircle.parent = mapHolder;
        }
    }

    public Vector3 CoordToVector(int x, int y)
    {
        return new Vector3(x + 0.5f, y + 0.5f, 0);
    }

    public Coordinate GetRandomCoordinates()
    {
        Coordinate randomCoordinate = circleCoordinates.Dequeue();
        circleCoordinates.Enqueue(randomCoordinate);
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
