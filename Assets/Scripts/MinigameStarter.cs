using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MinigameStarter : MonoBehaviour {

    public static bool generated;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	
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
                            int id = t.id;

                            GameObject tsG = GameObject.FindGameObjectWithTag("TreasureSpawner");
                            TreasureSpawner ts = null;

                            if (tsG != null)
                            {
                                ts = tsG.GetComponent<TreasureSpawner>();
                                print("DESTROY");
                                generated = false;
                                ts.RemoveTreasure(id, type);
                            }

                            //switch (type)
                            //{
                            //    case 0:
                            //        print("loading new scene");
                            //        SceneManager.LoadScene("MinigameMemory");
                            //        break;
                            //    case 1:
                            //        print("loading new scene");
                            //        SceneManager.LoadScene("MinigamePuzzle");
                            //        break;
                            //    case 2:
                            //        print("loading new scene");
                            //        SceneManager.LoadScene("MinigameQuiz");
                            //        break;
                            //    case 3:
                            //        print("loading new scene");
                            //        SceneManager.LoadScene("MinigameSprint");
                            //        break;
                            //    default:
                            //        print("loading new scene");
                            //        SceneManager.LoadScene("MinigameMemory");
                            //        break;
                            //}
                        }
                    }
                }
            }
        }
        else if (s.name == "CommanderScene")
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

                        treasure.GetComponent<ParticleSystem>().Play();
                        CommandsLeft.commandsLeft--;

                        Treasure t = th.treasure;
                        int type = t.type;
                        int id = t.id;

                        StartCoroutine(MakeTheTreasureVisible(id));

                        GameObject csG = GameObject.FindGameObjectWithTag("CommanderSpawner");
                        CommanderSpawner cs = null;

                        if (csG != null)
                        {
                            cs = csG.GetComponent<CommanderSpawner>();
                            generated = false;
                            cs.RemoveTreasure(id);
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
    IEnumerator MakeTheTreasureVisible(int id)
    {
        string IDsendURL = "http://gocommander.sytes.net/scripts/visible_treasure_locations.php";
        WWWForm form = new WWWForm();

        form.AddField("treasureidPost", id);
        WWW www = new WWW(IDsendURL, form);

        yield return www;
    }
}
