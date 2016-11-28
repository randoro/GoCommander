using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NextCommanderController : MonoBehaviour {

    List<string> candidateList = new List<string>();
    public List<Player> voters = new List<Player>();
    int lowestCount = 10;

    string[] polls;

    public void StartVoting()
    {
        int poll = 0;
        voters.Clear();

        StartCoroutine(CheckPolls());
        candidateList.Clear();

        for (int i = 0; i < voters.Count; i++)
        {
            if (voters[i].vote.Contains("CANDIDATE") || voters[i].vote.Contains("NOT"))
            {
                if (voters[i].vote.Contains("CANDIDATE") && voters[i].counter <= lowestCount)
                {
                    lowestCount = voters[i].counter;
                    candidateList.Add(voters[i].name);
                }

                poll++;

                if (poll >= voters.Count)
                {
                    EndVotingAndCompare(candidateList);
                }
            }
        }
    }

    void EndVotingAndCompare(List<string> candidateList)
    {
        if (candidateList.Count < 1)
        {
            string winner = "STOP";
            StartCoroutine(ReturnWinner(winner));
        }
        if (candidateList.Count > 1)
        {
            Random rnd = new Random();
            int index = Random.Range(0, candidateList.Count);
            string winner = candidateList[index];
            StartCoroutine(ReturnWinner(winner));
        }
        else if (candidateList.Count == 1)
        {
            int index = 0;
            string winner = candidateList[index];
            StartCoroutine(ReturnWinner(winner));
        }
    }

    IEnumerator ReturnWinner(string winner)
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";

        WWWForm form = new WWWForm();
        form.AddField("userNamePost", GoogleMap.username);
        form.AddField("userVotePost", winner);
        WWW www = new WWW(votersURL, form);
        yield return www;
    }
    public IEnumerator CheckPolls()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_poll.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        WWW www = new WWW(votersURL, form);
        yield return www;
        string result = www.text;

        if (result != null)
        {
            polls = result.Split(';');
        }

        for (int i = 0; i < polls.Length - 1; i++)
        {
            int id = int.Parse(GetDataValue(polls[i], "ID:"));
            string name = GetDataValue(polls[i], "Username.");
            string groupName = GetDataValue(polls[i], "Groupname:");
            int counter = int.Parse(GetDataValue(polls[i], "Counter:"));
            string vote = GetDataValue(polls[i], "Vote:");

            voters.Add(new Player(vote, counter, id, name, groupName));
        }
    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
