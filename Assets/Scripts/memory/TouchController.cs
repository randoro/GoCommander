﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchController : MonoBehaviour {
    MapGenerator1 mapGenerator1;
    //Vector2 tileCoordinatePosition;
    int x, y, finger_x, finger_y;

    bool isDragging, itHasBegun = false;
    Vector3 touchPosition;
    Vector3 realPos;
    Vector3 oldRealPos;
    MapGenerator1.Tile[,] tileArray;
    List<MapGenerator1.Tile> pressedTiles;
    MapGenerator1.Tile startPoint1, startPoint2;

	// Use this for initialization
	void Start ()
    {
        mapGenerator1 = FindObjectOfType<MapGenerator1>();
        //yield return mapGenerator1.done;
        //tileArray = mapGenerator1.tileArray;
        //pressedTiles = new List<MapGenerator1.Tile>();

        //startPoint1 = mapGenerator1.startPoint;
        //startPoint2 = mapGenerator1.otherStartPoint;

        //SetStartPointColor();

        oldRealPos = new Vector3(0, 0, 0);
	}

    private void SetStartPointColor()
    {
        startPoint1.ColorTile.ChangeColor(Color.gray);
        startPoint2.ColorTile.ChangeColor(Color.gray);
    }

	// Update is called once per frame
	void Update () 
    {
        if(MapGenerator1.finished)
        {
            print(MapGenerator1.finished.ToString());
            tileArray = mapGenerator1.tileArray;
            pressedTiles = new List<MapGenerator1.Tile>();

            startPoint1 = mapGenerator1.startPoint;
            startPoint2 = mapGenerator1.otherStartPoint;

            SetStartPointColor();

            MapGenerator1.finished = false;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            realPos = Input.GetTouch(0).position;
            realPos.z = 10f;
            realPos = Camera.main.ScreenToWorldPoint(realPos);

            if (WithinMapBounds((int)realPos.x, (int)realPos.y) && tileArray[(int)realPos.x, (int)realPos.y] == startPoint1 || 
                WithinMapBounds((int)realPos.x, (int)realPos.y) && tileArray[(int)realPos.x, (int)realPos.y] == startPoint2)
            {
                isDragging = true;
                pressedTiles.Add(tileArray[(int)realPos.x, (int)realPos.y]);
                oldRealPos = realPos;
            }
        }
        //Release
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isDragging = false;

            for (int i = 0; i < pressedTiles.Count; i++)
            {
                pressedTiles[i].TileReleased();
            }
            pressedTiles.Clear();
            SetStartPointColor();
        }
        //Drag
        else if (isDragging)
        {
            touchPosition = Input.GetTouch(0).position;
            realPos = Input.GetTouch(0).position;
            realPos.z = 10f;
            realPos = Camera.main.ScreenToWorldPoint(realPos);

            if (WithinMapBounds((int)realPos.x, (int)realPos.y) && 
                new Vector2((int)realPos.x, (int)realPos.y) != new Vector2((int)oldRealPos.x, (int)oldRealPos.y))
            {
                if (NeighbourTilesCheck() && 
                    !pressedTiles.Contains(tileArray[(int)realPos.x, (int)realPos.y]) && 
                    tileArray[(int)oldRealPos.x, (int)oldRealPos.y] == pressedTiles[pressedTiles.Count - 1])
                {
                    pressedTiles.Add(tileArray[(int)realPos.x, (int)realPos.y]);

                    if (tileArray[(int)realPos.x, (int)realPos.y].Obstacle == true)
                    {
                        Failure();

                        isDragging = false;
                    }
                    else if (tileArray[(int)realPos.x, (int)realPos.y] == startPoint1 || tileArray[(int)realPos.x, (int)realPos.y] == startPoint2)
                    {
                        Victory();

                        isDragging = false;
                    }
                    else
                    {
                        tileArray[(int)realPos.x, (int)realPos.y].TilePressed();
                    }
                } 
            }
            oldRealPos = realPos;
        }
	}
    private void Failure()
    {
        for (int i = 0; i < pressedTiles.Count; i++)
        {
            pressedTiles[i].ObstacleCollision();
        }
    }
    private void Victory()
    {
        for (int i = 0; i < pressedTiles.Count; i++)
        {
            pressedTiles[i].GoalIsReached();
        }
    }
    private bool NeighbourTilesCheck()
    {
        if ((int)realPos.x == (int)oldRealPos.x + 1 && (int)realPos.y == (int)oldRealPos.y || 
            (int)realPos.x == (int)oldRealPos.x - 1 && (int)realPos.y == (int)oldRealPos.y)
        {
            return true;
        }
        else if ((int)realPos.y == (int)oldRealPos.y + 1 && (int)realPos.x == (int)oldRealPos.x ||
                 (int)realPos.y == (int)oldRealPos.y - 1 && (int)realPos.x == (int)oldRealPos.x)
        {
            return true;
        }
        return false;
    }
    private bool WithinMapBounds(int x, int y)
    {
        if (x < mapGenerator1.mapSize.x && x > -1 && y < mapGenerator1.mapSize.y && y > -1)
        {
            return true;
        }
        return false;
    }
}
