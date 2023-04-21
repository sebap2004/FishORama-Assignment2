using System;
using System.Collections.Generic;

namespace FishORama;

public class Team
{
    public List<Piranha> teamMembers;
    public int TeamScore;
    public int teamNumber;
    
    public delegate void ScoreAdded(int team, int fish);
    public static event ScoreAdded AddedScore;
    
    
    
    public Team(int team)
    {
        teamMembers = new List<Piranha>();
        PiranhaBehaviour.ChickenAte += AddScore;
        teamNumber = team;
    }

    private void AddScore(int fish)
    {
        if (fish == teamNumber)
        {
            TeamScore++;
            AddedScore(teamNumber, fish);
        }
    }
}