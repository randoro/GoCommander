﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;

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

    StreamReader stream_reader;
    String level_file;
    List<String> map_strings;


    void Start()
    {
        //circleCount = Random.Range(5,15);
        //shuffleSeeed = Random.Range(0, 9999);
        gridLinePercent = 0.3f;

        int random_level = UnityEngine.Random.Range(1, 5);

        //if (random_level == 1)
        //{

        level_file = "Assets/Scripts/Puzzle/Level_TextFiles/Level1.txt";
        map_strings = new List<String>();
        SetUpReadFromFile(level_file);
       
        //}
    }

    public void SetUpReadFromFile(String level_file)
    {
        stream_reader = new StreamReader(@level_file);
        while (!stream_reader.EndOfStream)
        {
            map_strings.Add(stream_reader.ReadLine());
        }
        stream_reader.Close();
        map_strings.Reverse();

        //Regex.Split(level_file," ");

        //int x = int.Parse(level_file.Split(' ')[0].ToString());
        //int y = int.Parse(level_file.Split(' ')[1].ToString());

        mapSize.x = 5;
        mapSize.y = 9;

        tileCoordinates = new List<Coordinate>();
        tileArray = new Tile[(int)mapSize.x, (int)mapSize.y];

        string holderName = "Generated Map";
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = this.transform;

        GenerateMap(mapHolder);
       
        //if (transform.FindChild(holderName))
        //{
            //DestroyImmediate(transform.FindChild(holderName).gameObject);
        //}

        //circleCoordinates = new Queue<Coordinate>(Utility.ShuffleArray(tileCoordinates.ToArray(), shuffleSeeed));       
    }

    private void GenerateMap(Transform mapHolder)
    {
        for (int y = 0; y < map_strings.Count; y++)
            for (int x = 0; x < map_strings[y].Length; x++)
            {
                tileCoordinates.Add(new Coordinate(x, y));
                Tile newTile = new Tile(new Coordinate(x, y), tileArray, usedTiles);
                newTile.CurrentColor = Tile.CircleColor.Empty;
                tileArray[x,y] = newTile;

                Vector3 tilePos = CoordToVector(x,y);
                //Vector3 tilePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
                Transform newTileInstance = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right)) as Transform;

                newTileInstance.localScale = Vector3.one * (1 - gridLinePercent);
                newTileInstance.parent = mapHolder;

                switch(map_strings[y][x])
                {
                    case 'R':
                        {
                            //Coordinate randomizeCoord = GetRandomCoordinates();
                            Vector3 circlePos = CoordToVector(x, y);
                            //Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
                            Transform newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
                            newCircle.parent = mapHolder;

                            // Every tile with a circle has a gameobject
                            tileArray[x, y].CurrentColor = Tile.CircleColor.Red;
                        }
                        break;
                    case 'G':
                        {
                            //Coordinate randomizeCoord = GetRandomCoordinates();
                            Vector3 circlePos = CoordToVector(x, y);
                            //Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
                            Transform newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
                            newCircle.parent = mapHolder;

                            // Every tile with a circle has a gameobject
                            tileArray[x, y].CurrentColor = Tile.CircleColor.Green;
                        }
                        break;
                    case 'B':
                        {
                            //Coordinate randomizeCoord = GetRandomCoordinates();
                            Vector3 circlePos = CoordToVector(x, y);
                            //Vector3 circlePos = new Vector3(transform.position.x + transform.localScale.x * j, transform.position.y + transform.localScale.y * i, 0);
                            Transform newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
                            newCircle.parent = mapHolder;

                            // Every tile with a circle has a gameobject
                            tileArray[x, y].CurrentColor = Tile.CircleColor.Blue;
                        }
                        break;
                }
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
        public void UpdateColor(CircleColor newColor, CircleControl activeCircle)
        {
            // If the tile is empty it has to be binded with the dragged circle - otherwise the dragged circle should be deleted
            if (circle == null)
            {
                circle = activeCircle;
                CurrentColor = newColor;
            }
            else
            {
                ChangeNeighbourChain(newColor, null);
                CurrentColor = newColor;

                GameObject.Destroy(activeCircle.gameObject);
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
