using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MapGenerator1 : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform circlePrefab;
    public Vector2 mapSize;

    private String[] memoryLevels;
    private String level;
    public Text Thetext;
    public int shuffleSeeed;
    int circleCount;
    //tiden man har på sig att klara spelet
   private int starttime;
    //tid som är kvar
    private float timeleft;
    public int showtime;

  [Range(0, 1)]
    public float gridLinePercent;

    public static bool win = false;

    public Tile[,] tileArray;
    public Tile startPoint;
    public Tile otherStartPoint;

    public List<Coordinate> tileCoordinates;
    Queue<Coordinate> circleCoordinates;
    List<String> map_strings;

    public static bool finished = false;

    IEnumerator Start()
    {
        // !!! Denna metod skall läsas så tidigt som möjligt !!!
        yield return StartCoroutine(GetMemory());

        //circleCount = Random.Range(5,15);
        //shuffleSeeed = Random.Range(0, 9999);
        //gridLinePercent = 0.3f;
        SetUpReadFromFile();
        GenerateMap();
        SetUpCamera();
        starttime = 45;
        timeleft = starttime;
}

    IEnumerator GetMemory()
    {
        string memoryURL = "http://gocommander.sytes.net/scripts/memorylevel.php";

        WWW www = new WWW(memoryURL);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            memoryLevels = result.Split(';');
        }

        for (int i = 0; i < memoryLevels.Length - 1; i++)
        {
            int id = int.Parse(GetDataValue(memoryLevels[i], "ID:"));
            level = GetDataValue(memoryLevels[i], "Level:");
            //print(level);
            //listPuzzle.Add(new PuzzleList(id, level));
        }
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    void Update()
    {
     
        if (win == true)
        {
            StartCoroutine(delayTime());

            SceneManager.LoadScene("mainScene");
        }
        if (timeleft < -1)

        {
            Debug.Log("lose");
            StartCoroutine(delayTime());

            SceneManager.LoadScene("mainScene");
        }
        timeleft = timeleft - Time.deltaTime;

        // Debug.Log(timeleft);
        showtime = (int)timeleft;
        Thetext.text = showtime.ToString("");
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(5);
    }

    private void SetUpCamera()
    {
        Camera.main.transform.position = new Vector3(mapSize.x / 2, mapSize.y / 2, Camera.main.transform.position.z);
        finished = true;
    }

    public void SetUpReadFromFile()
    {
        map_strings = new List<String>();

        level = level.Replace(",", "\n");

        print(level);

        //TextAsset level_file = Resources.Load("memorylevel1") as TextAsset;

        String[] linesInFile = level.Split('\n');

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

            MapGenerator1.win = true;

        }
        public void ObstacleCollision()
        {
            colorTile.ChangeColor(Color.red);
        }
        public void TileReleased()
        {
            colorTile.ChangeColor(Color.white);
        }

        //IEnumerator delayTime()
        //{
        //    yield return new WaitForSeconds(5);
        //}
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
