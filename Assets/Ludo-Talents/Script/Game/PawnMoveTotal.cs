using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PawnMoveTotal : MonoBehaviour
{

    [SerializeField] private List<ScoreTest> scoreTests = new List<ScoreTest>();

    public Text playerScoreText_1;
    public Text playerScoreText_2;
    public Text playerScoreText_3;
    public Text playerScoreText_4;

    int p1, p2, p3, p4;

    private void Awake()
    {
        p1 = int.Parse(playerScoreText_1.text);
        p2 = int.Parse(playerScoreText_2.text);
        p3 = int.Parse(playerScoreText_3.text);
        p4 = int.Parse(playerScoreText_4.text);

       /* scoreTests.Add(new ScoreTest(p1, "p1"));
        scoreTests.Add(new ScoreTest(p2, "p2"));
        scoreTests.Add(new ScoreTest(p3, "p3"));
        scoreTests.Add(new ScoreTest(p4, "p4"));*/

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {

            DataSorts();
        }
    }

    void DataSorts()
    {
        scoreTests.Sort(sortFunc);
    }
    public int sortFunc(ScoreTest a, ScoreTest b)
    {
        if (a.playerScoreText > b.playerScoreText)
        {
            return -1;
        }
        else if (a.playerScoreText < b.playerScoreText)
        {
            return 1;
        }
        return 0;
    }

    void DisplayScore()
    {
        for (int i = 0; i < scoreTests.Count; i++)
        {

            if (scoreTests[i].playername == scoreTests[0].playername)
            {
                // rank 1
            }
            else if (scoreTests[i].playername == scoreTests[1].playername)
            {
                // rank 2
            }
            else if (scoreTests[i].playername == scoreTests[2].playername)
            {
                // rank 3
            }
            else if (scoreTests[i].playername == scoreTests[3].playername)
            {
                // rank 4
            }
        }
    }

}
