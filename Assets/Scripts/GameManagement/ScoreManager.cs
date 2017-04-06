using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Score Management (to be completed)
 * */
public class ScoreManager : MonoBehaviour {

    public int scoreToWin;
    private int m_CurrentScore;
    public List<GameObject> playerList;

    void Awake()
    {
        m_CurrentScore = 0;
    }

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void CheckForVictory()
    {

    }

    //Method fired when gaining/losing points/life/whatever (to rename ofc)
    void SomeEventToWinPoints(bool condition, int scoreAmount)
    {
        if (condition)
        {
            m_CurrentScore += scoreAmount;

            if (IsGameWon())
            {
                Victory();
            }
        }

        else
        {
            m_CurrentScore -= scoreAmount;
        }
    }

    public bool IsGameWon()
    {
        return m_CurrentScore == scoreToWin;
    }

    public void Victory()
    {

    }

    public void GameOver()
    {

    }
}
