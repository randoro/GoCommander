using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MinigameStarter : MonoBehaviour {

    public static bool generated;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Scene s = SceneManager.GetActiveScene();

        if (s.name == "mainScene")
        {

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
                            int type = t.type;
                            //int type = t.type;

                            //id = Random.Range(0, 3);

                            switch (type)
                            {
                                case 0:
                                    print("loading new scene");
                                    SceneManager.LoadScene("MinigameMemory");
                                    break;
                                case 1:
                                    print("loading new scene");
                                    SceneManager.LoadScene("MinigamePuzzle");
                                    break;
                                case 2:
                                    print("loading new scene");
                                    SceneManager.LoadScene("MinigameQuiz");
                                    break;
                                case 3:
                                    print("loading new scene");
                                    SceneManager.LoadScene("MinigameSprint");
                                    break;
                                default:
                                    print("loading new scene");
                                    SceneManager.LoadScene("MinigameMemory");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        if (s.name == "login")
        {
            if (generated)
            {
                generated = false;
            }
        }
    }
}
