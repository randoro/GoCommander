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

    //public static List<QuizList> skaneListQuiz = new List<QuizList>();
    //public static List<QuizList> nationalListQuiz = new List<QuizList>();

    public static List<QuizList> allQuestionsList = new List<QuizList>();
    private string[] allQuestions, allAnswersID, Quiz;

    //private string[] skaneQuestions, skaneAnswersID, Quiz;
    //private string[] nationalQuestions, nationalAnswersID, nationalQuiz;

    public Transform resultObj;
    GameObject scoremanager;
    public int score = 0;
    int answeredQuestions = 0;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public static int randomQuestion = -1;

    private string line, answerline;

    //public static bool isInSkane;

    private string city;
    private string country;
    private string username = GoogleMap.username;

    // Use this for initialization
    private void Start()
    {
        city = "";
        country = "";

        allQuestions = new string[4];
        allAnswersID = new string[4];

        StartCoroutine(JsonLocation());
        StartCoroutine(SendUserLocation());
        StartCoroutine(GetQuizes());

        //skaneQuestions = new string[4];
        //skaneAnswersID = new string[4];
        //nationalQuestions = new string[4];
        //nationalAnswersID = new string[4];

        //scoremanager = GameObject.Find ("HighScoreHolder").gameObject;
        //isInSkane = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //CheckPlayerPosition();
        ReadFromServer();
        SetQuestion();
        QuizSystem();
        LoadMainScene();

    }

    IEnumerator SendUserLocation()
    {
        string playerURL = "http://gocommander.sytes.net/scripts/get_city_country.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("userCityPost", city);
        form.AddField("userCountryPost", country);

        WWW www = new WWW(playerURL, form);
        yield return www;
    }

    IEnumerator GetQuizes()
    {
        //string name = "milan";
        string quizURL = "http://gocommander.sytes.net/scripts/get_quiz.php";

        WWWForm form = new WWWForm();
        //form.AddField("usernamePost", name);
        form.AddField("usernamePost", username);

        WWW www = new WWW(quizURL, form);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            Quiz = result.Split(';');
        }

        for (int i = 0; i < Quiz.Length - 1; i++)
        {
            string position = GetDataValue(Quiz[i], "Position:");
            string question = GetDataValue(Quiz[i], "Question:");
            string alt1 = GetDataValue(Quiz[i], "Wrong_1:");
            string alt2 = GetDataValue(Quiz[i], "Wrong_2:");
            string alt3 = GetDataValue(Quiz[i], "Wrong_3:");
            string alt4 = GetDataValue(Quiz[i], "Correct:");
            string answer = GetDataValue(Quiz[i], "Answer:");
            string image = GetDataValue(Quiz[i], "Image");

            allQuestionsList.Add(new QuizList(position, question, alt1, alt2, alt3, alt4, answer, image));

            //if (city == "Skåne")
            //{
            //    skaneListQuiz.Add(new QuizList(city, question, alt1, alt2, alt3, alt4, answer));
            //}
            //else
            //{
            //    nationalListQuiz.Add(new QuizList(city, question, alt1, alt2, alt3, alt4, answer));
            //}
        }

        //yield return new WaitForSeconds(1);
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
            //if (isInSkane)
            //{
                GetComponent<TextMesh>().text = allQuestions[randomQuestion];
                //GetComponent<TextMesh>().text = skaneQuestions[randomQuestion];
            //}
            //else
            //{
            //GetComponent<TextMesh>().text = nationalQuestions[randomQuestion];
            //}
        }
    }

    void QuizSystem()
    {
        if (choiceSelected == "y")
        {
            choiceSelected = "n";

            if (allAnswersID[randomQuestion].Contains(selectedAnswer))
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

            //if (isInSkane)
            //{
            //    if (skaneAnswersID[randomQuestion].Contains(selectedAnswer))
            //    {

            //        resultObj.GetComponent<TextMesh>().text = "Correct";
            //        resultObj.GetComponent<TextMesh>().color = Color.green;

            //        StartCoroutine(delayTime());
            //        score += 10;

            //        //print(answeredQuestions);
            //        //print(score);
            //        //print(randomQuestion);
            //    }
            //    else
            //    {
            //        resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
            //        resultObj.GetComponent<TextMesh>().color = Color.red;

            //        StartCoroutine(delayTime());
            //    }
            //}
            //else
            //{
            //    if (nationalAnswersID[randomQuestion].Contains(selectedAnswer))
            //    {

            //        resultObj.GetComponent<TextMesh>().text = "Correct";
            //        resultObj.GetComponent<TextMesh>().color = Color.green;

            //        StartCoroutine(delayTime());
            //        score += 10;

            //        //print(answeredQuestions);
            //        //print(score);
            //        //print(randomQuestion);
            //    }
            //    else
            //    {
            //        resultObj.GetComponent<TextMesh>().text = "Wrong Answer";
            //        resultObj.GetComponent<TextMesh>().color = Color.red;

            //        StartCoroutine(delayTime());
            //    }
            //}
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
        for (int i = 0; i < allQuestionsList.Count; i++)
        {
            allQuestions[i] = allQuestionsList[i].question;
            allAnswersID[i] = allQuestionsList[i].answer;
        }

        //for (int i = 0; i < skaneListQuiz.Count; i++)
        //{
        //    skaneQuestions[i] = skaneListQuiz[i].question;
        //    skaneAnswersID[i] = skaneListQuiz[i].answer;
        //}

        //for (int i = 0; i < nationalListQuiz.Count; i++)
        //{
        //    nationalQuestions[i] = nationalListQuiz[i].question;
        //    nationalAnswersID[i] = nationalListQuiz[i].answer;
        //}
    }

    void GenerateNewQuestion()
    {
        randomQuestion = -1;
        answeredQuestions++;

        resultObj.GetComponent<TextMesh>().text = "";
    }

    void RandomizeQuestion()
    {
        randomQuestion = Random.Range(0, 3);
        LoadImage.loadImage = true;
    }

    //void CheckPlayerPosition()
    //{
    //    //print(GoogleMap.centerLocation.latitude.ToString() + " " + GoogleMap.centerLocation.longitude.ToString());

    //    if (GoogleMap.centerLocation.latitude < 56 && GoogleMap.centerLocation.longitude < 14)
    //    {
    //        //isInSkane = true;
    //    }
    //}

    IEnumerator JsonLocation()
    {
        using (WebClient wc = new WebClient())
        {
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

            yield return new WaitForSeconds(1);
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
