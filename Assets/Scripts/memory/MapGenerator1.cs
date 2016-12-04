using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
//memoryclassen
public class MapGenerator1 : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform circlePrefab;
    public Vector2 mapSize;

    private String[] memoryLevels;
    private String level;
    public Text theTimeText, theTriesText;
    public int shuffleSeeed;
    int circleCount;
    //tiden man har på sig att klara spelet
    private int starttime;
    //tid som är kvar
    public float timeTaken;
    //public float timestartcolers;
	public float timeObstacle;
    public int showtime;
	public int score;
	public int lvl;//hämttar lvl från server (svårihihetsgrad)
	GameObject scoremanager;
//	List<Cerclecolerchencher> changecolor = new List<>();



  [Range(0, 1)]
    public float gridLinePercent;

    public static bool win = false, startUpdating = false;
    public static int tries = 1;

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

        SetUpReadFromFile();
        GenerateMap();
        SetUpCamera();
        timeTaken = 0;
		score = 0;
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

    public void CancelBtnClick()
    {
        SceneManager.LoadScene("mainScene");
    }

    void Update()
    {
        //if (timeObstacle < 0)
        //{

        //    timestartcolers = timestartcolers - Time.deltaTime;
        //    showtime = (int)timestartcolers;
        //}
        //if (timestartcolers < 0) {
        //    showtime = (int)timeleft;
        //}
        if (timeObstacle >= 0)
        {
            showtime = (int)timeObstacle;
            timeObstacle -= Time.deltaTime;
            theTimeText.text = "Obstacles disappears in " + showtime.ToString();
        }
        else if (timeObstacle < 0)
        {
            theTimeText.alignment = TextAnchor.UpperCenter;
            startUpdating = true;
            timeTaken += Time.deltaTime;
            showtime = (int)timeTaken;
            theTimeText.text = "time: " + showtime.ToString();
            theTriesText.text = "tries taken: " + tries.ToString();

            if (win)
            {
                GoogleMap.completedMinigames++;
                win = false;
                startUpdating = false;
				score = (750 -(int)timeTaken) - (tries*20);
                StartCoroutine(SendGroupScore(score));
                StartCoroutine(SendHighscore(score));
				print (score);
				tries = 1;
                StartCoroutine(SendCompletedMinigame());
                //StartCoroutine(delayTime());
                SceneManager.LoadScene("mainScene");
            }
        }
    }
    IEnumerator SendGroupScore(int score)
    {
        string scoreURL = "http://gocommander.sytes.net/scripts/score_send_group.php";

        WWWForm form = new WWWForm();
        form.AddField("userScorePost", score);
        form.AddField("userGroupPost", GoogleMap.groupName);

        WWW www = new WWW(scoreURL, form);

        yield return www;
    }

    IEnumerator SendHighscore(int score)
    {
        string scoreURL = "http://gocommander.sytes.net/scripts/highscore_send.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        form.AddField("userScorePost", score);
        form.AddField("userGamePost", "Memory");

        WWW www = new WWW(scoreURL, form);

        yield return www;
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(5);
    }

    IEnumerator SendCompletedMinigame()
    {
        string message = GoogleMap.username + " completed a Memory!";
        string loginUserURL = "http://gocommander.sytes.net/scripts/send_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        form.AddField("userMiniMessagePost", message);

        WWW www = new WWW(loginUserURL, form);

        yield return www;
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

                newTileInstance.localScale = new Vector3(Vector3.one.x * (1 - gridLinePercent), Vector3.one.y * (1 - gridLinePercent), 0.01f);
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
                newCircle.localScale = Vector3.one * (1 - gridLinePercent);
                newCircle.parent = mapHolder;

		//	changecolor.Add

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
            colorTile.ChangeColor(TouchController.ourBlue);
        }
        public void GoalIsReached()
        {
            colorTile.ChangeColor(TouchController.ourGreen);

            MapGenerator1.win = true;
        }
        public void ObstacleCollision()
        {
            colorTile.ChangeColor(TouchController.ourRed);
        }
        public void TileReleased()
        {
            colorTile.ChangeColor(TouchController.ourWhite);
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
