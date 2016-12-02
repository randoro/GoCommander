using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CircleControl : MonoBehaviour
{

	Vector3 tilePosition;
	Vector3 touchPosition;
	Vector3 moveDirection;
	Vector3 offset;
	MapGenerator mapGenerator;

	GameObject circleCopy;

	float speedRadius = 0;
	bool isDragging = false, destroyCopy = false;
	//bool canMove = true;
	int maxAmountOfMoves = 2;
	int amountOfMoves;
	int x, y;

    float z_depth;

    public Color ourRed;
    public Color ourGreen;
    public Color ourBlue;
	void Start()
	{
		moveDirection = new Vector3(0, 0, 0);

		tilePosition = transform.position;

		mapGenerator = FindObjectOfType<MapGenerator>();

        z_depth = GameObject.FindGameObjectWithTag("MainCamera").transform.position.z;

		x = (int)transform.position.x;
		y = (int)transform.position.y;

		transform.localScale = DeterminedScale(mapGenerator.tileArray[x, y].InitialMoveDecrease);
		//amountOfMoves = CalcMoveAmount(1.0f - transform.localScale.x);

		BindToTile();
		DetermineColor(mapGenerator.tileArray[x, y].CurrentColor);
	}
	private Vector3 DeterminedScale(int InitialMoveDecrease)
	{
		amountOfMoves = maxAmountOfMoves - InitialMoveDecrease;
		return new Vector3(1, 1, 1) - new Vector3(0.2f, 0.2f, 0) * InitialMoveDecrease;
	}
	public void DetermineColor(MapGenerator.Tile.CircleColor color)
	{
		if(color == MapGenerator.Tile.CircleColor.Red)
		{
			GetComponent<MeshRenderer>().material.color = ourRed;
		}
		else if (color == MapGenerator.Tile.CircleColor.Green)
		{
			GetComponent<MeshRenderer>().material.color = ourGreen;
		}
		else if (color == MapGenerator.Tile.CircleColor.Blue)
		{
			GetComponent<MeshRenderer>().material.color = ourBlue;
		}
	}
	private void BindToTile()
	{
		mapGenerator.tileArray[x, y].Circle = this;
	}

	//private int CalcMoveAmount(float scaleDifference)
	//{
	//    int scaleToInt = 5;
	//    return (int)(maxAmountOfMoves - (scaleDifference * scaleToInt));
	//}

	void Update()
	{
		//FirstTouch
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			touchPosition = Input.GetTouch(0).position;
			Vector3 realPosition = Input.GetTouch(0).position;
            realPosition.z = -z_depth;
			realPosition = Camera.main.ScreenToWorldPoint(realPosition);

			if (!isDragging &&
				WithinCircleRadius(realPosition, transform.position, GetComponent<CircleCollider2D>().radius) &&
				amountOfMoves > 0)
			{
				circleCopy = (GameObject)Instantiate(gameObject, transform.position, Quaternion.identity);
				AddMoves(-1);
				//mapGenerator.tileArray[x, y].InitialMoveDecrease -= 1;
				//circleCopy.transform.localScale -= new Vector3(0.2f, 0.2f, 0.0f);
				//transform.localScale = circleCopy.transform.localScale;

				//transform.localScale -= new Vector3(0.2f, 0.2f, 0.0f);
				//amountOfMoves--;

				offset = realPosition - transform.position;
				isDragging = true;
                destroyCopy = false;
			}
		}
		//Release
		else if (isDragging && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			isDragging = false;

			int new_x = (int)transform.position.x;
			int new_y = (int)transform.position.y;

			if (WithinMapBounds(new_x, new_y) && CheckNeighbourTiles(new_x, new_y) && mapGenerator.tileArray[new_x, new_y].CurrentColor != mapGenerator.tileArray[x, y].CurrentColor)
			{
				tilePosition = mapGenerator.CoordToVector(new_x, new_y);
				mapGenerator.tileArray[new_x, new_y].UpdateTile(mapGenerator.tileArray[x, y].CurrentColor, this, mapGenerator.tileArray[x, y].InitialMoveDecrease);
                mapGenerator.allPrefabs.Add(circleCopy.transform);
				//mapGenerator.tileArray[new_x, new_y].InitialMoveDecrease = ;

				x = new_x;
				y = new_y;
			}
			else
			{
                destroyCopy = true;
                //GameObject.Destroy(circleCopy);
                //AddMoves(1);
                //BindToTile();
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

            if (WithinCircleRadius(tilePosition, transform.position, 0.15f) && destroyCopy) // Note (Calle): Here the radius is bigger for the sake of making the circle bigger as soon as it clashes with the copy which is supposed to be removed
            {
                GameObject.Destroy(circleCopy);
                AddMoves(1);
                BindToTile();
                destroyCopy = false;
            }
		}
	}
	private void AddMoves(int addition)
	{
		mapGenerator.tileArray[x, y].InitialMoveDecrease -= addition;
		amountOfMoves += addition;
		transform.localScale += new Vector3(0.2f, 0.2f, 0.0f) * addition;
	}
	private void SnapToGrid()
	{
		float distance_right_now = Vector3.Distance(transform.position, tilePosition);
		transform.position += moveDirection * (distance_right_now / speedRadius) * 0.3f;
	}

	private bool CheckNeighbourTiles(int new_x, int new_y)
	{
		if (new_x == x + 1 && new_y == y || new_x == x - 1 && new_y == y)
		{
			return true;
		}
		else if (new_y == y + 1 && new_x == x || new_y == y - 1 && new_x == x)
		{
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