using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator1 : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform circlePrefab;
    public Vector2 mapSize;
    
    public int shuffleSeeed;
    int circleCount;

    [Range(0,1)]
    public float gridLinePercent;

    public Tile[,] tileArray;
    public Tile startPoint;
    public Tile otherStartPoint;

    public List<Coordinate> tileCoordinates;
    Queue<Coordinate> circleCoordinates;
    List<String> map_strings;

    private void Awake()
    {
        //circleCount = Random.Range(5,15);
        //shuffleSeeed = Random.Range(0, 9999);
        //gridLinePercent = 0.3f;
        SetUpReadFromFile();
        GenerateMap();
        SetUpCamera();
    }

    private void SetUpCamera()
    {
        Camera.main.transform.position = new Vector3(mapSize.x / 2, mapSize.y / 2, Camera.main.transform.position.z);
    }

    public void SetUpReadFromFile()
    {
        map_strings = new List<String>();

        TextAsset level_file = Resources.Load("memorylevel1") as TextAsset;

        String[] linesInFile = level_file.text.Split('\n');

        for (int i = 0; i < linesInFile.Length; i++)
        {
            map_strings.Add(linesInFile[i]);
        }

        map_strings.Reverse();

        mapSize.x = map_strings[0].Length;
        mapSize.y = map_strings.Count;

        tileCoordinates = new List<Coordinate>();
        tileArray = new Tile[(int)mapSize.x, (int)mapSize.y];
    }

    private void GenerateMap()
    {
        string holderName = "Generated Map";
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = this.transform;

        for (int y = 0; y < mapSize.y; y++)
            for (int x = 0; x < mapSize.x; x++)
            {
                tileCoordinates.Add(new Coordinate(x, y));
                Tile newTile = new Tile(new Coordinate(x, y));
                tileArray[x, y] = newTile;

                Vector3 tilePos = CoordToVector(x, y);
                //Vector3 tilePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
                Transform newTileInstance = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right)) as Transform;

                newTileInstance.localScale = Vector3.one * (1 - gridLinePercent);
                newTileInstance.parent = mapHolder;

                DetermineTileStatus(x, y, mapHolder);
            }
    }
    private void DetermineTileStatus(int x, int y, Transform mapHolder)
    {
        switch (map_strings[y][x])
        {
            case 'O':
                Vector3 circlePos = CoordToVector(x, y);
                Transform newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
                newCircle.parent = mapHolder;

                tileArray[x, y].Obstacle = true;
                break;
            case 'S':
                if (startPoint == null)
                {
                    startPoint = tileArray[x, y];
                }
                else
                {
                    otherStartPoint = tileArray[x, y];
                }
                break;
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

    public class Tile
    {
        bool obstacle = false;

        Coordinate coordinate;
        ColorTile colorTile;

        public Tile(Coordinate coordinate)
        {
            this.coordinate = coordinate;
        }
        public bool Obstacle
        {
            get
            {
                return obstacle;
            }
            set
            {
                obstacle = value;
            }
        }
        public ColorTile ColorTile
        {
            get
            {
                return colorTile;
            }
            set
            {
                colorTile = value;
            }
        }
        public void TilePressed()
        {
            colorTile.ChangeColor(Color.blue);
        }
        public void GoalIsReached()
        {
            colorTile.ChangeColor(Color.green);
        }
        public void ObstacleCollision()
        {
            colorTile.ChangeColor(Color.red);
        }
        public void TileReleased()
        {
            colorTile.ChangeColor(Color.white);
        }
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
