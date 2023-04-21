using System;
using System.Collections.Generic;
using FishORamaEngineLibrary;

namespace FishORama;

public class Referee
{

    private Team team1;
    private Team team2;
    private ITokenManager _tokenManager;
    
    private bool isFighting;

    private Random _random;
    
    private List<Piranha> Team1Members;
    private List<Piranha> Team2Members;

    public delegate void PlaceLeg();

    public static event PlaceLeg LegPlace;


    public Referee(Team pTeam1, Team pTeam2)
    {
        team1 = pTeam1;
        team2 = pTeam2;
        Team1Members = team1.teamMembers;
        Team2Members = team2.teamMembers;
        Team.AddedScore += ScoreAdded;
        PiranhaBehaviour.roundTrigger += Game;
        PiranhaBehaviour.RoundEnd += EndRound;
        _random = new Random();
    }

    public void StartGame()
    {
        _tokenManager = team1.teamMembers[0].tokenManager;
        Game();
    }

    void EndRound()
    {
        isFighting = false;
    }
    
    private void Game()
    {
        if (isFighting) { return; }
        RoundTrigger();
    }

    private void ScoreAdded(int pTeamNumber, int fish)
    {
        Console.WriteLine($"Team {pTeamNumber} fish {fish} won a point!");
    }

    void RoundTrigger()
    {
        if (team1.TeamScore >= 5)
        {
            foreach (var fish in Team1Members)
            {
                fish.piranhaBehaviour.SetFishState(PiranhaBehaviour.FishState.Win);
            }

            Console.WriteLine("Team 1 wins!");
            return;
        }

        if (team2.TeamScore >= 5)
        {
            foreach (var fish in Team2Members)
            {
                fish.piranhaBehaviour.SetFishState(PiranhaBehaviour.FishState.Win);
            }

            Console.WriteLine("Team 2 wins!");
            return;
        }
        if (_tokenManager.ChickenLeg != null)
        {
            isFighting = true;
            Console.WriteLine("Triggered round");
            int randomNumber = new Random().Next(0, 3);
            Team1Members[randomNumber].piranhaBehaviour.SetFishState(PiranhaBehaviour.FishState.Chase);
            Team2Members[randomNumber].piranhaBehaviour.SetFishState(PiranhaBehaviour.FishState.Chase);
            Console.WriteLine($"Setting fish {randomNumber + 1} to fight");
        }
    }

    public void Update()
    {
        if (isFighting)
        {return;}

        if (_random.Next(0, 101) == 1)
        {
            LegPlace();
            Console.WriteLine("Placed leg");
            Console.WriteLine("Placed leg");
        }
    }
}