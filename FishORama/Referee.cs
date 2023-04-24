using System;
using System.Collections.Generic;
using FishORamaEngineLibrary;

namespace FishORama;

public class Referee
{

    // Cached variable of the two teams and tokenManager.
    private Team _team1;
    private Team _team2;
    private ITokenManager _tokenManager;
    
    // Boolean which determines if the fish are fighting or not.
    private bool _isFighting;
    private Random _random;
    
    // Lists of members for both teams are cached separate from the team object for ease of reading.
    // Assigned in the constructor.
    private List<Piranha> _team1Members;
    private List<Piranha> _team2Members;

    
    // Event that the simulation class subscribes to in order to place the leg in the scene.
    public delegate void PlaceLeg();
    public static event PlaceLeg LegPlace;


    public Referee(Team pTeam1, Team pTeam2)
    {
        _team1 = pTeam1;
        _team2 = pTeam2;
        _team1Members = _team1.teamMembers;
        _team2Members = _team2.teamMembers;
        Team.AddedScore += ScoreAdded;
        Piranha.roundTrigger += Game;
        Piranha.RoundEnd += EndRound;
        _random = new Random();
    }

    // Grabs the token manager from one of the fish and starts the game.
    public void StartGame()
    {
        _tokenManager = _team1.teamMembers[0].tokenManager; 
        Game();
    }

    // Called by an event in the Piranha class. Sets up the fish for the next round.
    void EndRound()
    {
        _isFighting = false;
    }
    
    
    // Checks if the fish are fighting, if they are not fighting then the round is triggered.
    private void Game()
    {
        if (_isFighting) { return; }
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
        if (_team1.TeamScore >= 5)
        {
            foreach (var fish in _team1Members)
            {
                fish.SetFishState(Piranha.FishState.Win);
            }

            Console.WriteLine("Team 1 wins!");
            return;
        }

        if (_team2.TeamScore >= 5)
        {
            foreach (var fish in _team2Members)
            {
                fish.SetFishState(Piranha.FishState.Win);
            }

            Console.WriteLine("Team 2 wins!");
            return;
        }
        if (_tokenManager.ChickenLeg != null)
        {
            _isFighting = true;
            Console.WriteLine("Triggered round");
            int randomNumber = _random.Next(0, 3);
            _team1Members[randomNumber].SetFishState(Piranha.FishState.Chase);
            _team2Members[randomNumber].SetFishState(Piranha.FishState.Chase);
            Console.WriteLine($"Setting fish {randomNumber + 1} to fight");
        }
    }

    
    // Checks if the fish are fighting, if not then make random checks to place the leg in the scene.
    public void Update()
    {
        if (_isFighting)
        {return;}

        if (_random.Next(0, 101) == 1)
        {
            LegPlace();
            Console.WriteLine("Placed leg");
            Console.WriteLine("Placed leg");
        }
    }
}