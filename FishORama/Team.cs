using System;
using System.Collections.Generic;

namespace FishORama;

public class Team
{
    public readonly List<Piranha> teamMembers;
    public int TeamScore;
    public readonly int teamNumber;
    
    // Event which 
    public delegate void ScoreAdded(int team, int fish);
    public static event ScoreAdded AddedScore; 
    
    
    
    public Team(int pTeamNumber)
    {
        teamMembers = new List<Piranha>();
        Piranha.ChickenAte += AddScore; // Subscribes the AddScore method to the chicken eating fish event, which gets called when a fish eats a chicken.
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