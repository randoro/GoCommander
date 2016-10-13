using UnityEngine;
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
    public int amountofMoves = 5;
    int x, y;

    void Start()
    {
        moveDirection = new Vector3(0, 0, 0);
        noScale = new Vector3(0, 0, 0);
        tilePosition = transform.position;
        mapGenerator = FindObjectOfType<MapGenerator>();

        System.Random random = new System.Random();
        defaultType = ((Type)(random.Next(0, 4)));

        x = (int)transform.position.x;
        y = (int)transform.position.y;
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

                if (canMove && !isDragging && WithinCircleRadius(realPosition,
                    transform.position, GetComponent<CircleCollider2D>().radius))
                {
                    circleCopy = this.gameObject;
                    Instantiate(circleCopy, transform.position, Quaternion.identity);

                    if (amountofMoves > 0)
                    {
                        circleCopy.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                        offset = realPosition - transform.position;
                        amountofMoves -= 1;
                        isDragging = true;
                    }
                    else
                    {
                        canMove = false;
                    }
                }
            }
            //Release
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isDragging = false;

                int new_x = (int)transform.position.x;
                int new_y = (int)transform.position.y;

                if (WithinMapBounds(new_x, new_y) && CheckNeighbourTiles(new_x, new_y))
                {
                    tilePosition = mapGenerator.CoordToVector(x, y);
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
            if (canMove && !isDragging && !WithinCircleRadius(tilePosition, transform.position, 0.01f))
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