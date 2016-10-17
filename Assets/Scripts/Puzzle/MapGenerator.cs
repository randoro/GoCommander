using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Text;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform circlePrefab;
    public Vector2 mapSize;
    
    public int shuffleSeeed;
    //int circleCount;

    [Range(0,1)]
    public float gridLinePercent;

    public Tile[,] tileArray;
    public List<Coordinate> tileCoordinates;
    Queue<Coordinate> circleCoordinates;
    List<Tile> usedTiles = new List<Tile>();

    //StreamReader stream_reader;
    List<String> map_strings;

    void Start()
    {
        //circleCount = Random.Range(5,15);
        //shuffleSeeed = Random.Range(0, 9999);
        gridLinePercent = 0.3f;

        int random_level = UnityEngine.Random.Range(1, 5);

        //if (random_level == 1)
        //{

              //level_file_path = level_file.text;
              //level_file_text = "Assets/Resources/Level1.txt";

        //}

        SetUpReadFromFile("puzzlelevel1");
        GenerateMap();
        SetUpCamera();
    }
    private void SetUpCamera()
    {
        Camera.main.transform.position = new Vector3(mapSize.x / 2, mapSize.y / 2, Camera.main.transform.position.z);
    }

    public void SetUpReadFromFile(String level_to_load)
    {
        map_strings = new List<String>();

        TextAsset level_file = Resources.Load(level_to_load) as TextAsset;

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
                Tile newTile = new Tile(new Coordinate(x, y), tileArray, usedTiles);
                tileArray[x, y] = newTile;

                Vector3 tilePos = CoordToVector(x, y);
                //Vector3 tilePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
                Transform newTileInstance = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right)) as Transform;

                newTileInstance.localScale = Vector3.one * (1 - gridLinePercent);
                newTileInstance.parent = mapHolder;

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


    public class Tile
    {
        public enum CircleColor
        {
            //Empty,
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

                DidWeWin();
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

        private bool DidWeWin()
        {
            Tile firstColoredTile = this;
            bool breakAllLoops = false;

            for (int i = 0; i < tileFamily.GetLength(0); i++)
            {
                for (int j = 0; j < tileFamily.GetLength(1); j++)
                {
                    if (tileFamily[i, j].currentColor != null)
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
                    if (tileFamily[i, j].currentColor != null && tileFamily[i, j].currentColor != firstColoredTile.currentColor)
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
