using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NextCommanderController : MonoBehaviour {

    List<string> candidateList = new List<string>();
    public List<Player> voters = new List<Player>();
    int lowestCount = 10;
    public int refreshDelay = 2;
    int setAllPendingToNot = 0;
    public IEnumerator startVoting;

    string[] polls;

    void Start()
    {
        startVoting = StartVoting();
    }

    IEnumerator StartVoting()
    {
        while (true)
        {
            setAllPendingToNot++;

            if (setAllPendingToNot == 7)
            {
                yield return StartCoroutine(SetAllPENDINGtoNOT());
            }

            int poll = 0;
            voters.Clear();

            yield return StartCoroutine(CheckPolls());
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
                    
                    if (poll == voters.Count)
                    {
                        yield return StartCoroutine(EndVotingAndCompare(candidateList));
                    }
                }
                else
                {
                    break;
                }
            }
            yield return new WaitForSeconds(refreshDelay);
        }
    }
    IEnumerator EndVotingAndCompare(List<string> candidateList)
    {
        if (candidateList.Count < 1)
        {
            string votePost = "STOP";
            yield return StartCoroutine(ReturnWinner(null, votePost));
        }
        if (candidateList.Count > 1)
        {
            string votePost = "COMMANDER";
            Random rnd = new Random();
            int index = Random.Range(0, candidateList.Count);
            string winner = candidateList[index];
            yield return StartCoroutine(ReturnWinner(winner, votePost));
        }
        else if (candidateList.Count == 1)
        {
            string votePost = "COMMANDER";
            int index = 0;
            string winner = candidateList[index];
            yield return StartCoroutine(ReturnWinner(winner, votePost));
        }
    }
    IEnumerator SetAllPENDINGtoNOT()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";

        for (int i = 0; i < voters.Count; i++)
        {
            if (voters[i].vote.Contains("PENDING"))
            {
                WWWForm form = new WWWForm();
                form.AddField("usernamePost", voters[i].name);
                form.AddField("userVotePost", "NOT");
                WWW www = new WWW(votersURL, form);
                yield return www;
            }
        }
        //form.AddField("userGroupPost", GoogleMap.groupName);
    }
    IEnumerator ReturnWinner(string winner, string votePost)
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_vote.php";

        WWWForm form = new WWWForm();
        if (winner != null)
        {
            form.AddField("usernamePost", winner);
        }
        form.AddField("userVotePost", votePost);
        form.AddField("userGroupPost", GoogleMap.groupName);
        WWW www = new WWW(votersURL, form);
        setAllPendingToNot = 0;
        yield return www;
        StopCoroutine(startVoting);
    }
    IEnumerator CheckPolls()
    {
        string votersURL = "http://gocommander.sytes.net/scripts/commander_poll.php";

        WWWForm form = new WWWForm();
        form.AddField("userGroupPost", GoogleMap.groupName);
        WWW www = new WWW(votersURL, form);
        yield return www;
        string result = www.text;

        print(result);

        if (result != null)
        {
            polls = result.Split(';');
        }

        for (int i = 0; i < polls.Length - 1; i++)
        {
            int id = int.Parse(GetDataValue(polls[i], "ID:"));
            string name = GetDataValue(polls[i], "Username:");
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
