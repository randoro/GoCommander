using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
	public Transform tilePrefab;
	public Transform circlePrefab;
	public Vector2 mapSize;

	private string[] puzzle;
	private string level;

	public int shuffleSeeed;
	//int circleCount;

	[Range(0,1)]
	public float gridLinePercent;

	public Tile[,] tileArray;
	public Tile tile;
	public List<Coordinate> tileCoordinates;
	Queue<Coordinate> circleCoordinates;
	List<Tile> usedTiles = new List<Tile>();
    public List<Transform> allPrefabs = new List<Transform>();

	//StreamReader stream_reader;
	List<String> map_strings;
	/// <summary>
	/// efter det här tidsvariabler
	/// </summary>
	/// <returns></returns>
	public Text Thetext;
	private int starttime;
	private float timeleft;
	public int showtime;
	public int score;
	public int lvl;//hämttar lvl från server (svårihihetsgrad)
	private float countDown;
	public static bool win = false;


	//HighScoreHolder
	IEnumerator Start()
	{
		//circleCount = Random.Range(5,15);
		//shuffleSeeed = Random.Range(0, 9999);
        //gridLinePercent = 0.3f;

		int random_level = UnityEngine.Random.Range(1, 5);

		//if (random_level == 1)
		//{

		//level_file_path = level_file.text;
		//level_file_text = "Assets/Resources/Level1.txt";

		//}
		yield return StartCoroutine(GetPuzzles());

		SetUpReadFromFile();
		GenerateMap();
		SetUpCamera();

		//tid
		starttime = 0;
		timeleft = starttime;
		score = 1000;
		countDown = 0f;
	}
	void Update()
	{
		timeleft = timeleft + Time.deltaTime;
		showtime = (int)timeleft;
		countDown += Time.deltaTime;
		Thetext.text = "time: "+ showtime.ToString();

		if (win)
		{
            win = false;
            score = 1000 - (int)countDown;
            StartCoroutine(SendCompletedMinigame());
            StartCoroutine(SendGroupScore(score));
            StartCoroutine(SendHighscore(score));
            GoogleMap.completedMinigames++;
            SceneManager.LoadScene("mainScene");
		}
	}
    IEnumerator SendCompletedMinigame()
    {
        string message = GoogleMap.username + " completed a Puzzle!";
        string loginUserURL = "http://gocommander.sytes.net/scripts/send_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        form.AddField("userMiniMessagePost", message);

        WWW www = new WWW(loginUserURL, form);

        yield return www;
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
        form.AddField("userGamePost", "Puzzle");

        WWW www = new WWW(scoreURL, form);

        yield return www;
    }

    IEnumerator GetPuzzles()
	{
		string puzzleURL = "http://gocommander.sytes.net/scripts/puzzlelevel.php";

		WWW www = new WWW(puzzleURL);
		yield return www;
		string result = www.text;

		if (result != null)
		{
			puzzle = result.Split(';');
		}

		for (int i = 0; i < puzzle.Length - 1; i++)
		{
			int id = int.Parse(GetDataValue(puzzle[i], "ID:"));
			level = GetDataValue(puzzle[i], "Level:");

			//print(level);

			//listPuzzle.Add(new PuzzleList(id, level));
		}
		level = level.Replace(',', '\n');
	}

	string GetDataValue(string data, string index)
	{
		string value = data.Substring(data.IndexOf(index) + index.Length);
		if (value.Contains("|"))
			value = value.Remove(value.IndexOf("|"));
		return value;
	}


	private void SetUpCamera()
	{
		Camera.main.transform.position = new Vector3(mapSize.x / 2, mapSize.y / 2, Camera.main.transform.position.z);
	}

	public void SetUpReadFromFile()
	{
		map_strings = new List<String>();

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
				Tile newTile = new Tile(new Coordinate(x, y), tileArray, usedTiles);
				tileArray[x, y] = newTile;

				Vector3 tilePos = CoordToVector(x, y);
				Transform newTileInstance = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right)) as Transform;

				newTileInstance.localScale = new Vector3(Vector3.one.x * (1 - gridLinePercent), Vector3.one.y * (1 - gridLinePercent), 0.01f);
				newTileInstance.parent = mapHolder;

                allPrefabs.Add(newTileInstance);

				CreateCircle(x, y, mapHolder);
			}
	}
	private void CreateCircle(int x, int y, Transform mapHolder)
	{
		switch (map_strings[y][x])
		{
		case 'R':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			Vector3 circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			Transform newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Red;
			tileArray[x, y].InitialMoveDecrease = 0;
			break;

		case 'G':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Green;
			tileArray[x, y].InitialMoveDecrease = 0;
			break;

		case 'B':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Blue;
			tileArray[x, y].InitialMoveDecrease = 0;
			break;

		case 'r':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Red;
			tileArray[x, y].InitialMoveDecrease = 1;
			break;

		case 'g':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Green;
			tileArray[x, y].InitialMoveDecrease = 1;
			break;

		case 'b':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Blue;
			tileArray[x, y].InitialMoveDecrease = 1;
			break;
		case '+':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Red;
			tileArray[x, y].InitialMoveDecrease = 2;
			break;

		case '-':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Green;
			tileArray[x, y].InitialMoveDecrease = 2;
			break;

		case '*':
			//Coordinate randomizeCoord = GetRandomCoordinates();
			circlePos = CoordToVector(x, y);
			//Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
			newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
			newCircle.parent = mapHolder;
            allPrefabs.Add(newCircle);

			// Every tile with a circle has a gameobject
			tileArray[x, y].CurrentColor = Tile.CircleColor.Blue;
			tileArray[x, y].InitialMoveDecrease = 2;
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

	IEnumerator delayTime()
	{
		yield return new WaitForSeconds(5);
	}
	public void TestWin()
	{
        //StartCoroutine(delayTime());
		SceneManager.LoadScene("mainScene");
	}
    public void Restart()
    {
        for (int i = 0; i < allPrefabs.Count; i++)
        {
            if (allPrefabs[i] != null)
            {
                GameObject.Destroy(allPrefabs[i].gameObject);
            }
        }
        allPrefabs.Clear();

        SetUpReadFromFile();
        GenerateMap();
    }
    /// <summary>
    /// Ny klass
    /// </summary>
	public class Tile
	{

		public enum CircleColor
		{
			Empty,
			Red,
			Blue,
			Green
		}
		CircleColor currentColor;

		Coordinate coordinate;
		CircleControl circle;
		Tile[,] tileFamily;
		List<Tile> usedTiles;

		int inMoveDe;

		public Tile(Coordinate coordinate, Tile[,] tileFamily, List<Tile> usedTiles)
		{
			this.coordinate = coordinate;
			this.tileFamily = tileFamily;
			this.usedTiles = usedTiles;

			currentColor = CircleColor.Empty;
		}

		public CircleColor CurrentColor
		{
			get
			{
				return currentColor;
			}
			set
			{
				currentColor = value;
				if (circle != null)
				{
					circle.DetermineColor(currentColor);
				}
			}
		}
		public CircleControl Circle
		{
			get
			{
				return circle;
			}
			set
			{
				circle = value;
			}
		}
		public int InitialMoveDecrease
		{
			get
			{
				return inMoveDe;
			}
			set
			{
				inMoveDe = value;
			}
		}
		public void UpdateTile(CircleColor newColor, CircleControl activeCircle, int newMoveDecrease)
		{
			// If the tile is empty it has to be binded with the dragged circle - otherwise the dragged circle should be deleted
			if (circle == null)
			{
				circle = activeCircle;
				CurrentColor = newColor;
				inMoveDe = newMoveDecrease;
			}
			else
			{
				ChangeNeighbourChain(newColor, null);
				CurrentColor = newColor;

				GameObject.Destroy(activeCircle.gameObject);

				if (DidWeWin())
				{
//					InformativeMessage.isPuzzleCompleted = true;
					MapGenerator.win = true;
				}
			}
		}
		public void ChangeNeighbourChain(CircleColor newColor, Tile parent)
		{
			usedTiles.Add(this); // We need to MOVE ON - we don't wanna come back here and make the whole thing explode

			int y_negative_neighbour = coordinate.y - 1;
			int y_positive_neighbour = coordinate.y + 1;

			int x_negative_neighbour = coordinate.x - 1;
			int x_positive_neighbour = coordinate.x + 1;

			// Neighbour upstairs
			if (y_positive_neighbour < tileFamily.GetLength(1))
			{
				Tile neighbour_up = tileFamily[coordinate.x, y_positive_neighbour];

				if (neighbour_up.CurrentColor == currentColor && !usedTiles.Contains(neighbour_up))
				{
					Tile newParent = this;

					tileFamily[coordinate.x, y_positive_neighbour].ChangeNeighbourChain(newColor, newParent);
					tileFamily[coordinate.x, y_positive_neighbour].CurrentColor = newColor;
				}
			}
			// Neighbour downstairs
			if (y_negative_neighbour > -1)
			{
				Tile neighbour_down = tileFamily[coordinate.x, y_negative_neighbour];

				if (neighbour_down.CurrentColor == currentColor && !usedTiles.Contains(neighbour_down))
				{
					Tile newParent = this;

					tileFamily[coordinate.x, y_negative_neighbour].ChangeNeighbourChain(newColor, newParent);
					tileFamily[coordinate.x, y_negative_neighbour].CurrentColor = newColor;
				}
			}
			// Neighbour to the right
			if (x_positive_neighbour < tileFamily.GetLength(0))
			{
				Tile neighbour_right = tileFamily[x_positive_neighbour, coordinate.y];

				if (neighbour_right.CurrentColor == currentColor && !usedTiles.Contains(neighbour_right))
				{
					Tile newParent = this;

					tileFamily[x_positive_neighbour, coordinate.y].ChangeNeighbourChain(newColor, newParent);
					tileFamily[x_positive_neighbour, coordinate.y].CurrentColor = newColor;
				}
			}
			// Neighbour to the left
			if (x_negative_neighbour > -1)
			{
				Tile neighbour_left = tileFamily[x_negative_neighbour, coordinate.y];

				if (neighbour_left.CurrentColor == currentColor && !usedTiles.Contains(neighbour_left))
				{
					Tile newParent = this;

					tileFamily[x_negative_neighbour, coordinate.y].ChangeNeighbourChain(newColor, newParent);
					tileFamily[x_negative_neighbour, coordinate.y].CurrentColor = newColor;
				}
			}

			if (parent == null) // So this is where it started ...
			{
				usedTiles.Clear();
			}
		}

		public bool DidWeWin()
		{
			Tile firstColoredTile = this;
			bool breakAllLoops = false;

			for (int i = 0; i < tileFamily.GetLength(0); i++)
			{
				for (int j = 0; j < tileFamily.GetLength(1); j++)
				{
					if (tileFamily[i, j].currentColor != CircleColor.Empty)
					{
						firstColoredTile = tileFamily[i, j];
						breakAllLoops = true;
						break;
					}
				}
				if (breakAllLoops)
				{
					break;
				}
			}

			breakAllLoops = false;

			for (int i = 0; i < tileFamily.GetLength(0); i++)
			{
				for (int j = 0; j < tileFamily.GetLength(1); j++)
				{
					if (tileFamily[i, j].currentColor != CircleColor.Empty && tileFamily[i, j].currentColor != firstColoredTile.currentColor)
					{
						breakAllLoops = true;
						break;
					}
					else if (tileFamily[i, j] == tileFamily[tileFamily.GetLength(0) - 1, tileFamily.GetLength(1) - 1])
					{
						return true;

					}
				}
				if (breakAllLoops)
				{
					break;
				}
			}
			return false;
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