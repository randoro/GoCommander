using UnityEngine;
using System.Collections;

public class MinigameStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            print("found0");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.tag == "Treasure")
                {
                    GameObject treasure = hit.transform.root.gameObject;
                    TreasureHolder th = treasure.GetComponent<TreasureHolder>();
                    if (th.canBeClicked)
                    {
                        Treasure t = th.treasure;
                        int id = t.id;

                        id = Random.Range(0, 3);

                        switch (id)
                        {
                            case 0:
                                print("loading new scene");
                                Application.LoadLevel("MinigameMemory");
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
                }
            }
        }


        
        

    }
}
