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

    public Tile[,] tileArray;
    public List<Coordinate> tileCoordinates;
    Queue<Coordinate> circleCoordinates;
    List<Tile> usedTiles = new List<Tile>();

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
        tileArray = new Tile[(int)mapSize.x, (int)mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {

                tileCoordinates.Add(new Coordinate(x, y));
                Tile newTile = new Tile(new Coordinate(x, y), tileArray, usedTiles);
                newTile.CurrentColor = Tile.CircleColor.Empty;
                tileArray[x, y] = newTile;
            }
        }

        circleCoordinates = new Queue<Coordinate>(Utility.ShuffleArray(tileCoordinates.ToArray(), shuffleSeeed));

        string holderName = "Generated Map";

        if(transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = this.transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePos = CoordToVector(x, y);
                Transform newTileInstance = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right)) as Transform;
                newTileInstance.localScale = Vector3.one * (1 - gridLinePercent);
                newTileInstance.parent = mapHolder;
            }
        }
        
        for(int i = 0; i < circleCount; i++)
        {
            Coordinate randomizeCoord = GetRandomCoordinates();
            Vector3 circlePos = CoordToVector(randomizeCoord.x, randomizeCoord.y);
            Transform newCircle = Instantiate(circlePrefab, circlePos, Quaternion.identity) as Transform;
            newCircle.parent = mapHolder;

            // Every tile with a circle has a gameobject
            tileArray[randomizeCoord.x, randomizeCoord.y].CurrentColor = (Tile.CircleColor)Random.Range(1, 4);
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
