using System;
using System.Collections.Generic;

namespace FishORama;

public class Team
{
    // Necessary team variables are stored, such as its members, score, and number.
    public readonly List<Piranha> teamMembers;
    public int TeamScore;
    public readonly int teamNumber;
    
    // Event which tells the referee a fish has scored. For debugging purposes
    public delegate void ScoreAdded(int team, int fish);
    public static event ScoreAdded AddedScore; 
    
    
    // Constructor where the team list is initialised, team number is assigned and events are subscribed to. 
    public Team(int pTeamNumber)
    {
        teamMembers = new List<Piranha>();
        Piranha.ChickenAte += AddScore; // Subscribes the AddScore method to the chicken eating fish event, which gets called when a fish eats a chicken.
        teamNumber = pTeamNumber;
    }

    // Method that adds a score to the team. Is subscribed to and triggered by the ChickenAte event.
    private void AddScore(int pFishTeamNumber)
    {
        if (pFishTeamNumber == teamNumber)
        {
            TeamScore++;
            AddedScore(teamNumber, pFishTeamNumber);
        }
    }
}