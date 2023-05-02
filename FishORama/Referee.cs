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
    
    private bool _isFighting;   // Boolean which determines if the fish are fighting or not.
    private Random _random; // Random class used for random calculations
    private bool gameEnded; // Boolean which determines if the game has ended
    
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
        _tokenManager = _team1.teamMembers[0].tokenManager; // T
        Game();
    }

    // Called by an event in the Piranha class. Sets up the fish for the next round.
    void EndRound()
    {
        // Checks if either of the teams have one, if so then end the game, else set isFighting to false to prepare for next round.
        if (_team1.TeamScore >= 5)
        {
            foreach (Piranha fish in _team1Members)
            {
                fish.SetFishState(Piranha.FishState.Win);
            }
            gameEnded = true;
            return;
        }

        if (_team2.TeamScore >= 5)
        {
            foreach (Piranha fish in _team2Members)
            {
                fish.SetFishState(Piranha.FishState.Win);
            }
            gameEnded = true;
            return;
        }
        _isFighting = false;
    }
    
    
    // Checks if the fish are fighting or if the game has ended, if they are not fighting then the round is triggered.
    private void Game()
    {
        if (_isFighting) { return; }
        if (gameEnded) {return;}
        RoundTrigger();
    }

    // Prints to the console which fish from which team got the point. For debugging purposes.
    private void ScoreAdded(int pTeamNumber, int fish)
    {
        Console.WriteLine($"Team {pTeamNumber} fish {fish} won a point!");
    }

    
    // Choose a random number between 0 and 2 which corresponds to a certain fish, then makes both of them chase the leg.
    void RoundTrigger()
    {
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

    
    // Checks if the fish are fighting or if the game has ended, if not then make random checks to place the leg in the scene.
    public void Update()
    {
        if (_isFighting)
        {return;}
        if (gameEnded)
        {return;}
        if (_random.Next(0, 101) == 1)
        {
            LegPlace();
            Console.WriteLine("Placed leg");
        }
    }
}