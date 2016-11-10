using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Net;
using SimpleJSON;

public class Manager : MonoBehaviour
{

    public static List<QuizList> skaneListQuiz = new List<QuizList>();
    public static List<QuizList> nationalListQuiz = new List<QuizList>();

    public static List<QuizList> allQuestionsList = new List<QuizList>();

    private string[] skaneQuestions, skaneAnswersID, Quiz;
    private string[] nationalQuestions, nationalAnswersID, nationalQuiz;

    public Transform resultObj;
    GameObject scoremanager;
    public int score = 0;
    int answeredQuestions = 0;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1;

    private string line, answerline;

    public static bool isInSkane;

    // Use this for initialization
    private void Start()
    {

        StartCoroutine(GetQuizes());

        skaneQuestions = new string[4];
        skaneAnswersID = new string[4];
        nationalQuestions = new string[4];
        nationalAnswersID = new string[4];

        //scoremanager = GameObject.Find ("HighScoreHolder").gameObject;

        isInSkane = false;
        JsonLocation();


    }

    // Update is called once per frame
    private void Update()
    {
        CheckPlayerPosition();
        ReadFromServer();
        SetQuestion();
        QuizSystem();
        LoadMainScene();

    }

    IEnumerator GetQuizes()
    {
        string quizURL = "http://gocommander.sytes.net/scripts/get_quiz.php";

        WWW www = new WWW(quizURL);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            Quiz = result.Split(';');
        }

        for (int i = 0; i < Quiz.Length - 1; i++)
        {
            string city = GetDataValue(Quiz[i], "City:");
            string question = GetDataValue(Quiz[i], "Question:");
            string alt1 = GetDataValue(Quiz[i], "Wrong_1:");
            string alt2 = GetDataValue(Quiz[i], "Wrong_2:");
            string alt3 = GetDataValue(Quiz[i], "Wrong_3:");
            string alt4 = GetDataValue(Quiz[i], "Correct:");
            string answer = GetDataValue(Quiz[i], "Answer:");

            allQuestionsList.Add(new QuizList(city, question, alt1, alt2, alt3, alt4, answer));

            if (city == "Skåne")
            {
                skaneListQuiz.Add(new QuizList(city, question, alt1, alt2, alt3, alt4, answer));
            }
            else
            {
                nationalListQuiz.Add(new QuizList(city, question, alt1, alt2, alt3, alt4, answer));
            }
        }
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(1);
        GenerateNewQuestion();
    }

    void SetQuestion()
    {
        if (randomQuestion == -1)
        {
            RandomizeQuestion();
        }
        if (randomQuestion > -1)
        {
            if (isInSkane)
            {
                GetComponent<TextMesh>().text = skaneQuestions[randomQuestion];
            }
            else
            {
                GetComponent<TextMesh>().text = nationalQuestions[randomQuestion];
            }
        }
    }

    void QuizSystem()
    {
        if (choiceSelected == "y")
        {
            choiceSelected = "n";

            if (isInSkane)
            {
                if (skaneAnswersID[randomQuestion].Contains(selectedAnswer))
                {

                    resultObj.GetComponent<TextMesh>().text = "Correct";
                    resultObj.GetComponent<TextMesh>().color = Color.green;

                    StartCoroutine(delayTime());
                    score += 10;

                    //print(answeredQuestions);
                    //print(score);
                    //print(randomQuestion);
                }
                else
                {
                    resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
                    resultObj.GetComponent<TextMesh>().color = Color.red;

                    StartCoroutine(delayTime());
                }
            }
            else
            {
                if (nationalAnswersID[randomQuestion].Contains(selectedAnswer))
                {

                    resultObj.GetComponent<TextMesh>().text = "Correct";
                    resultObj.GetComponent<TextMesh>().color = Color.green;

                    StartCoroutine(delayTime());
                    score += 10;

                    //print(answeredQuestions);
                    //print(score);
                    //print(randomQuestion);
                }
                else
                {
                    resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
                    resultObj.GetComponent<TextMesh>().color = Color.red;

                    StartCoroutine(delayTime());
                }
            }
        }
    }

    void LoadMainScene()
    {
        if (answeredQuestions == 4)
        {
            SceneManager.LoadScene("mainScene");
        }
    }

    void ReadFromServer()
    {
        for (int i = 0; i < skaneListQuiz.Count; i++)
        {
            skaneQuestions[i] = skaneListQuiz[i].question;
            skaneAnswersID[i] = skaneListQuiz[i].answer;
        }

        for (int i = 0; i < nationalListQuiz.Count; i++)
        {
            nationalQuestions[i] = nationalListQuiz[i].question;
            nationalAnswersID[i] = nationalListQuiz[i].answer;
        }
    }

    void GenerateNewQuestion()
    {
        randomQuestion = -1;
        answeredQuestions++;

        resultObj.GetComponent<TextMesh>().text = "";
    }

    void RandomizeQuestion()
    {
        randomQuestion = Random.Range(0, 2);
    }

    void CheckPlayerPosition()
    {
        //print(GoogleMap.centerLocation.latitude.ToString() + " " + GoogleMap.centerLocation.longitude.ToString());

        if (GoogleMap.centerLocation.latitude < 56 && GoogleMap.centerLocation.longitude < 14)
        {
            //isInSkane = true;
        }
    }

    void JsonLocation()
    {
        using (WebClient wc = new WebClient())
        {
            string country = "";
            string city = "";

            double lat = GoogleMap.centerLocation.latitude;
            double lng = GoogleMap.centerLocation.longitude;

            //string json = wc.DownloadString("http://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224%2C-73.961452&sensor=true");
            string json = wc.DownloadString("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat.ToString() + "%2C" + lng.ToString() + "&sensor=true");

            SimpleJSON.JSONNode item = JSON.Parse(json);
            var N = JSON.Parse(json);
            var address_components = N["results"][0]["address_components"];
            int count = N["results"][0]["address_components"].Count;

            for (int i = 0; i < count; i++)
            {
                var longName = address_components[i]["long_name"];
                var type = address_components[i]["types"][0];

                if (type.Value == "country")
                {
                    country = longName.Value;
                    print("country: "+longName);
                }

                if (type.Value == "locality")
                {
                    city = longName.Value;
                    print("city: " + longName);
                }

            }


            //RootObject item = JsonUtility.FromJson<RootObject>(json);
            /*
            print(versionString);
            List<Result> rList = item.results;
            print(rList.Count);
            List<AddressComponent> aList = rList[0].address_components;

            foreach (AddressComponent c in aList)
            {
                if (c.types[0] == "country")
                {
                    country = c.long_name;
                }

                if (c.types[0] == "administrative_area_level_1")
                {
                    city = c.long_name;
                }

            }

            print(country + " " + city);
            */

        }
    }
}

    public class AddressComponent
{
    public string long_name { get; set; }
    public string short_name { get; set; }
    public List<string> types { get; set; }
}

public class Location
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Northeast
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Southwest
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Viewport
{
    public Northeast northeast { get; set; }
    public Southwest southwest { get; set; }
}

public class Geometry
{
    public Location location { get; set; }
    public string location_type { get; set; }
    public Viewport viewport { get; set; }
}

public class Result
{
    public List<AddressComponent> address_components { get; set; }
    public string formatted_address { get; set; }
    public Geometry geometry { get; set; }
    public string place_id { get; set; }
    public List<string> types { get; set; }
}

public class RootObject
{
    public List<Result> results { get; set; }
    public string status { get; set; }
}
