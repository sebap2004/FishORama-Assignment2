using System;
using System.Collections.Generic;
using FishORamaEngineLibrary;

namespace FishORama;

public class Referee
{

    private Team team1;
    private Team team2;

    private bool isCalculating;
    
    private List<Piranha> Team1Members;
    private List<Piranha> Team2Members;

    public Referee(Team pTeam1, Team pTeam2)
    {
        team1 = pTeam1;
        team2 = pTeam2;
        Team1Members = team1.teamMembers;
        Team2Members = team2.teamMembers;
        Team.AddedScore += ScoreAdded;
    }

    public void StartGame()
    {
        Game();
    }

    private void Game()
    {
        isCalculating = true;
        int randomNumber = new Random().Next(0, 3);
        if (isCalculating)
        {
            Team1Members[randomNumber].piranhaBehaviour.SetFishState(PiranhaBehaviour.FishState.Chase);
            Team2Members[randomNumber].piranhaBehaviour.SetFishState(PiranhaBehaviour.FishState.Chase);
            isCalculating = false; 
        }

        Console.WriteLine(randomNumber);
    }

    private void ScoreAdded(int pTeamNumber, int fish)
    {
        Console.WriteLine($"Team {pTeamNumber} fish {fish} won a point!");
    }
}