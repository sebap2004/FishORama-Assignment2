using System;
using System.Collections.Generic;

namespace FishORama;

public class Team
{
    public readonly List<FishBehaviour> teamMembers;
    public int TeamScore;
    public readonly int teamNumber;
    
    public delegate void ScoreAdded(int team, int fish);
    public static event ScoreAdded AddedScore;
    
    
    
    public Team(int pTeamNumber)
    {
        teamMembers = new List<FishBehaviour>();
        FishBehaviour.ChickenAte += AddScore;
        teamNumber = pTeamNumber;
    }

    private void AddScore(int pFishTeamNumber)
    {
        if (pFishTeamNumber == teamNumber)
        {
            TeamScore++;
            AddedScore(teamNumber, pFishTeamNumber);
        }
    }
}