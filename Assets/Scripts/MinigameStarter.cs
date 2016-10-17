using UnityEngine;
using System.Collections;

public class MinigameStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            print("found0");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                print("found1");

                if (hit.transform.tag == "Treasure")
                {
                    print("found2");
                    GameObject treasure = hit.transform.root.gameObject;
                    print("found3");
                    TreasureHolder th = treasure.GetComponent<TreasureHolder>();
                    print("found4");
                    Treasure t = th.treasure;
                    print("found5");
                    int id = t.id;

                    switch (id)
                    {
                        case 0:
                            print("loading new scene");
                            //Application.LoadLevel("MinigameMemory");
                            break;
                        case 1:
                            print("loading new scene");
                            Application.LoadLevel("MinigamePuzzle");
                            break;
                        case 2:
                            print("loading new scene");
                            Application.LoadLevel("MinigameQuiz");
                            break;
                        default:
                            print("loading new scene");
                            Application.LoadLevel("MinigameMemory");
                            break;
                    }
                }
                else
                {

                }
            }
        }

    }
}
