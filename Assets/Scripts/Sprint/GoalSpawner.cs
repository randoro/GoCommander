using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GoalSpawner : MonoBehaviour
{

    public bool goalSpawned;
    private GameObject map;
    private GoogleMap gMap;
    public GameObject prefab;

    private GoalHolder goalHolder;
    
    public Text timeText;
    private double scoreDistance;
    private float timeStarted;

    // Use this for initialization
    void Start () {
        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();
        timeStarted = Time.fixedTime;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float time = Time.fixedTime - timeStarted;

        timeText.text = ((int)time).ToString() + " sec";

        if (!goalSpawned)
	    {
	        if (GoogleMap.centerLocation.latitude != 0 && GoogleMap.centerLocation.longitude != 0)
	        {
	            
	            goalSpawned = true;

                double xGoal = randomizeAround(GoogleMap.centerLocation.longitude, 0.0030D);
                double yGoal = randomizeAround(GoogleMap.centerLocation.latitude, 0.0030D);

	            scoreDistance = Mathf.Abs((float) (GoogleMap.centerLocation.longitude - xGoal) * (float)(GoogleMap.centerLocation.latitude - yGoal));
                //double xGoal = (GoogleMap.centerLocation.longitude + 0.0030D);
                //double yGoal = (GoogleMap.centerLocation.latitude + 0.0030D);

                print("Create goal at: " + yGoal + " ," + xGoal);

                goalHolder = ((GameObject)Instantiate(prefab,
                            new Vector3(coordScaleToGameScale(xGoal - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                                0.0f, coordScaleToGameScale(yGoal - GoogleMap.centerLocation.latitude, 90.0f, 9.0f)),
                            Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 360, 0)))).GetComponent<GoalHolder>();
                goalHolder.Initialize(0, yGoal, xGoal);
                goalHolder.gameObject.transform.localScale = new Vector3(4f, 4f, 4f);
            }
	    }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                print("clicked");
                if (hit.transform.tag == "Goal")
                {
                    print("clicked on goal");
                    GameObject treasure = hit.transform.root.gameObject;
                    GoalHolder th = treasure.GetComponent<GoalHolder>();
                    if (th.canBeClicked)
                    {
                        print("clicked on goal inside ring");
                        Treasure t = th.treasure;
                        int id = t.id;
                        if (id == 0)
                        {
                            print("game won");

                                StartCoroutine(SendCompletedMinigame());
                                int score = (int)((1000.0f / (Time.fixedTime - timeStarted)) * scoreDistance);
                                StartCoroutine(SendGroupScore(score));
                                StartCoroutine(SendHighscore(score));

                                GoogleMap.completedMinigames++;

                            Application.LoadLevel("mainScene");
                        }
                    }
                }
            }
        }

    }

    IEnumerator SendCompletedMinigame()
    {
        string message = GoogleMap.username + " completed a Sprint!";
        string loginUserURL = "http://gocommander.sytes.net/scripts/send_minimessage.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        form.AddField("userMiniMessagePost", message);

        WWW www = new WWW(loginUserURL, form);

        yield return www;
    }

    IEnumerator SendGroupScore(int score)
    {
        string scoreURL = "http://gocommander.sytes.net/scripts/score_send_group.php";

        WWWForm form = new WWWForm();
        form.AddField("userScorePost", score);
        form.AddField("userGroupPost", GoogleMap.groupName);

        WWW www = new WWW(scoreURL, form);

        yield return www;
    }

    IEnumerator SendHighscore(int score)
    {
        string scoreURL = "http://gocommander.sytes.net/scripts/highscore_send.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", GoogleMap.username);
        form.AddField("userScorePost", score);
        form.AddField("userGamePost", "Sprint");

        WWW www = new WWW(scoreURL, form);

        yield return www;
    }

    private double randomizeAround(double middle, double radius)
    {
        double zeroToOne = Random.value;
        double adder = -radius + (2*radius*zeroToOne);

        return middle + adder;
    }

    private float coordScaleToGameScale(double inFloat, double total, float multi)
    {
        float returnfloat = (float)((inFloat / total) * (multi * (double)Math.Pow(2, gMap.zoom)));
        return returnfloat;
    }

    public void UpdateGoalLocation()
    {
            
            Treasure v = goalHolder.treasure;
        goalHolder.transform.position =
                new Vector3(coordScaleToGameScale(v.lng - GoogleMap.centerLocation.longitude, 180.0f, 10.0f),
                    0.0f, coordScaleToGameScale(v.lat - GoogleMap.centerLocation.latitude, 90.0f, 9.0f));
        
    }
}
