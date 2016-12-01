using UnityEngine;
using System.Collections;

public class CircleColorChanger : MonoBehaviour {
	MapGenerator1 mapGenerator1;
	bool onetimeuse = true;
	// Use this for initialization
	void Start () {
		
		mapGenerator1 = FindObjectOfType<MapGenerator1>();
	}
    //public void ChangeColor(Color color)
    //{
		
    //}
	
	// Update is called once per frame
	void Update () {
		if (mapGenerator1.timeObstacle < 0 && onetimeuse == true) {
			gameObject.transform.localScale = new Vector3 (0, 0, 0);
			onetimeuse = false;
		}
	}
}
