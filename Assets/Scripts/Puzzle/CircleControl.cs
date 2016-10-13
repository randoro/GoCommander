﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CircleControl : MonoBehaviour
{

    public enum Type
    {
        Red = 0,
        Blue = 1,
        Green = 2
    }

    Type defaultType;

    Vector3 tilePosition;
    Vector3 touchPosition;
    Vector3 moveDirection;
    Vector3 offset;
    Vector3 noScale;
    MapGenerator mapGenerator;
    GameObject circleCopy;
    
    float speedRadius = 0;
    bool isDragging = false;
    bool canMove = true;
    float maxAmountOfMoves = 2;
    public int amountofMoves;
    int x, y;

    void Start()
    {
        amountofMoves = CalcMoveAmount(1.0f - transform.localScale.x);
        moveDirection = new Vector3(0, 0, 0);
        noScale = new Vector3(0, 0, 0);
        tilePosition = transform.position;
        mapGenerator = FindObjectOfType<MapGenerator>();

        System.Random random = new System.Random();
        defaultType = ((Type)(random.Next(0, 4)));

        x = (int)transform.position.x;
        y = (int)transform.position.y;
    }
    int CalcMoveAmount(float scaleDifference)
    {
        int scaleToInt = 5;
        return (int)(maxAmountOfMoves - (scaleDifference * scaleToInt));
    }

    void Update()
    {
        //FirstTouch
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosition = Input.GetTouch(0).position;
                Vector3 realPosition = Input.GetTouch(0).position;
                realPosition.z = 10f;
                realPosition = Camera.main.ScreenToWorldPoint(realPosition);

                if (!isDragging && 
                    WithinCircleRadius(realPosition, transform.position, GetComponent<CircleCollider2D>().radius) &&
                    amountofMoves > 0)
                {
                    circleCopy = this.gameObject;
                    circleCopy.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
                    Instantiate(circleCopy, transform.position, Quaternion.identity);

                    offset = realPosition - transform.position;
                    amountofMoves -= 1;
                    isDragging = true;

                    //if ()
                    //{
                        
                    //}
                    //else
                    //{
                    //    canMove = false;
                    //}
                }
            }
            //Release
            else if (isDragging && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isDragging = false;

                int new_x = (int)transform.position.x;
                int new_y = (int)transform.position.y;

                if (WithinMapBounds(new_x, new_y) && CheckNeighbourTiles(new_x, new_y))
                {
                    tilePosition = mapGenerator.CoordToVector(x, y);
                }
                else
                {
                    Destroy(circleCopy);
                    amountofMoves += 1;
                    transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                }

                speedRadius = Vector3.Distance(transform.position, tilePosition);
                moveDirection = tilePosition - transform.position;
            }
            //Drag
            else if (isDragging)
            {
                touchPosition = Input.GetTouch(0).position;
                Vector3 realPos = Input.GetTouch(0).position;
                realPos.z = 10f;
                realPos = Camera.main.ScreenToWorldPoint(realPos);

                transform.position = realPos - offset;
            }

            if (!isDragging && !WithinCircleRadius(tilePosition, transform.position, 0.01f))
            {
                SnapToGrid();
            }
    }

    private void SnapToGrid()
    {
        float distance_right_now = Vector3.Distance(circleCopy.transform.position, tilePosition);
        transform.position += moveDirection * (distance_right_now / speedRadius) * 0.3f;
    }

    private bool CheckNeighbourTiles(int new_x, int new_y)
    {
        if (new_x == x + 1 && new_y == y || new_x == x - 1 && new_y == y)
        {
            x = new_x;
            return true;
        }
        if (new_y == y + 1 && new_x == x || new_y == y - 1 && new_x == x)
        {
            y = new_y;
            return true;
        }
        return false;
    }
    private bool WithinMapBounds(int x, int y)
    {
        if (x < mapGenerator.mapSize.x && x > -1 && y < mapGenerator.mapSize.y && y > -1)
        {
            return true;
        }
        return false;
    }
    private bool WithinCircleRadius(Vector3 realPos, Vector3 pos, float radius)
    {
        if (Vector3.Distance(realPos, pos) < radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}