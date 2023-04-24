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
    
    // Lists of members for both teams are cached separate from the team object for ease of reading.
    // Assigned in the constructor.
    private List<Piranha> Team1Members;
    private List<Piranha> Team2Members;

    
    // Event that the simulation class subscribes to in order to place the leg in the scene.
    public delegate void PlaceLeg();
    public static event PlaceLeg LegPlace;


    public Referee(Team pTeam1, Team pTeam2)
    {
        team1 = pTeam1;
        team2 = pTeam2;
        Team1Members = team1.teamMembers;
        Team2Members = team2.teamMembers;
        Team.AddedScore += ScoreAdded;
        Piranha.roundTrigger += Game;
        Piranha.RoundEnd += EndRound;
        _random = new Random();
    }

    // Grabs the token manager from one of the fish and starts the game.
    public void StartGame()
    {
        _tokenManager = team1.teamMembers[0].tokenManager; 
        Game();
    }

    // Called by an event in the Piranha class. Sets up the fish for the next round.
    void EndRound()
    {
        isFighting = false;
    }
    
    
    // Checks if the fish are fighting, if they are not fighting then the round is triggered.
    private void Game()
    {
        if (isFighting) { return; }
        RoundTrigger();
    }

    // Prints to the console which fish from which team got the point. For debugging purposes.
    private void ScoreAdded(int pTeamNumber, int fish)
    {
        Console.WriteLine($"Team {pTeamNumber} fish {fish} won a point!");
    }

    
    // Checks if either team has won, if not trigger a round, if yes then end the game.
    void RoundTrigger()
    {
        if (team1.TeamScore >= 5)
        {
            foreach (var fish in Team1Members)
            {
                fish.SetFishState(Piranha.FishState.Win);
            }

            Console.WriteLine("Team 1 wins!");
            return;
        }

        if (team2.TeamScore >= 5)
        {
            foreach (var fish in Team2Members)
            {
                fish.SetFishState(Piranha.FishState.Win);
            }

            Console.WriteLine("Team 2 wins!");
            return;
        }
        if (_tokenManager.ChickenLeg != null)
        {
            isFighting = true;
            Console.WriteLine("Triggered round");
            int randomNumber = new Random().Next(0, 3);
            Team1Members[randomNumber].SetFishState(Piranha.FishState.Chase);
            Team2Members[randomNumber].SetFishState(Piranha.FishState.Chase);
            Console.WriteLine($"Setting fish {randomNumber + 1} to fight");
        }
    }

    
    // Checks if the fish are fighting, if not then make random checks to place the leg in the scene.
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