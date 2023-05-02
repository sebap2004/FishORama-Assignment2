using System;
using System.Runtime.InteropServices;
using FishORamaEngineLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishORama;

public class Piranha : Fish
{
    // These are events that are used to communicate with other scripts without having to link with each other.
    
    // Event which gets called when the fish has eaten a chicken, along with its team number that gets sent to the referee.
    public delegate void AteChicken(int teamNumber);
    public static event AteChicken ChickenAte;
    
    // When the chicken leg is in the scene, the fish triggers this event. The referee receives this and then makes 
    // the decision to start the game.
    public delegate void TriggerNewGame();  
    public static event TriggerNewGame roundTrigger;
    
    // This event is triggered when the fish is set back to idle after a round. The referee receives this event to understand
    // that the fish are ready to start another round.
    public delegate void EndRound();
    public static event EndRound RoundEnd;

    private bool _ateAlready; // Checks if the piranha has eaten already to avoid double scoring.
    private FishState _currentState; // Stores the current state of the fish. Used in the movement method to determine behavior.
    private int spinRadius; // The size of the circle that the fish swims in the idle state. Randomised on object creation.
    private int spinSpeed; // The speed that the fish swims in a circle. Randomised on object creation.

    // Constructor that inherits from the fish base class. Passes in the team, sets the fish to idle, and sets the spinSpeed and radius.
    public Piranha(string pTextureID, float pXpos, float pYpos, Screen pScreen, ITokenManager pTokenManager, int pTeamNumber, int pFishNumber, Team pTeam) : base(pTextureID, pXpos, pYpos, pScreen, pTokenManager, pTeamNumber, pFishNumber, pTeam)
    {
        team = pTeam;
        _currentState = FishState.Idle;
        spinRadius = _random.Next(8, 12);
        spinSpeed = _random.Next(2, 8);
    }

    public override void Update()
    {
        // Movement is seperated in a separate class for ease of reading.
        Movement();
    }
    
    // Reusable method used to calculate the distance between two points.
    private Vector2 CalculateDirection(Vector2 pStartVec, Vector2 pEndVec)
    {
        Vector2 distanceVector = Vector2.Subtract(pEndVec, pStartVec);
        return distanceVector;
    }
    
    
    private void Movement()
    {
        // *** ADD YOUR MOVEMENT/BEHAVIOUR CODE HERE ***
            switch (_currentState)
            {
                case (FishState.Idle):
                    // Code for circular movement. If the chicken leg is in the scene, sends the round trigger event.
                    xPosition = idlePosition.X + spinRadius * (float)Math.Cos(angle);
                    yPosition = idlePosition.Y + spinRadius * (float)Math.Sin(angle);
                    angle += spinSpeed * 0.025f;
                    if (tokenManager.ChickenLeg != null)
                    {
                        roundTrigger();
                    }
                    break;
                case (FishState.Chase):
                    
                    // Vector2 positions are initialised.
                    Vector2 chickenPosition;
                    Vector2 currentPosition;
                    Vector2 distanceVector;
                    Vector2 directionVector;
                    
                    // If token manager is present, the fish calculates the position between itself and the leg, and then
                    // sets the direction and moves towards it.
                    if (tokenManager.ChickenLeg != null)
                    {
                        chickenPosition = new Vector2(tokenManager.ChickenLeg.Position.X, tokenManager.ChickenLeg.Position.Y);
                        currentPosition = new Vector2(xPosition, yPosition);
                        distanceVector = CalculateDirection(currentPosition, chickenPosition);
                        directionVector = Vector2.Normalize(distanceVector);
                        xPosition += directionVector.X * speed;
                        yPosition += directionVector.Y * speed;
                        // If its close enough to the leg, it eats the leg and sets the state to return to its original position.
                        if (distanceVector.Length() < 100 && tokenManager.ChickenLeg != null && !_ateAlready)
                        {
                            _ateAlready = true;
                            SetFishState(FishState.Return);
                            ChickenAte(teamNumber);
                            tokenManager.RemoveChickenLeg();
                        }
                    }
                    // If the fish misses the chicken, it sets the state to return and changes the sprite to an angry sprite.
                    else
                    {
                        SetFishState(FishState.Return);
                        textureID = "Piranha2";
                    }
                    
                    break;
                case (FishState.Return):
                    // Calculates the position between itself and its startPosition, and moves back to its original
                    // position.
                    currentPosition = new Vector2(xPosition, yPosition);
                    distanceVector = CalculateDirection(currentPosition, idlePosition);
                    directionVector = Vector2.Normalize(distanceVector);
                    xPosition += directionVector.X * speed;
                    yPosition += directionVector.Y * speed;

                    // If its close enough to its original position, it snaps back to place and sets the state back to
                    // idle.
                    if (distanceVector.Length() < 5)
                    {
                        xPosition = idlePosition.X;
                        yPosition = idlePosition.Y;
                        SetFishState(FishState.Idle);
                    }

                    _ateAlready = false;
                    break;
                case (FishState.Win):
                    // Sets the direction vector to off screen, direction depending which side they're on, and swims
                    // past the losing team.
                    if (team.teamNumber == 1)
                    {
                        directionVector =
                            Vector2.Normalize(CalculateDirection(new Vector2(xPosition,yPosition),
                                new Vector2(xPosition,yPosition) + new Vector2(1200, 0)));
                    }
                    else
                    {
                        directionVector =
                            Vector2.Normalize(CalculateDirection(new Vector2(xPosition,yPosition),
                                new Vector2(xPosition,yPosition) + new Vector2(-1200, 0)));
                    }
                    xPosition += directionVector.X * speed;
                    yPosition += directionVector.Y * speed;
                    break;
            }
    }
    
    
    // Enum used to store each discrete fish state. Stored in the currentState variable.
    public enum FishState
    {
        Idle,
        Chase,
        Return,
        Win
    }
    
    
    // Separate method which sets the state of the fish. With each state in the switch statement, there is code that only runs once.
    // This code changes the state as well as other relevant things.
    public void SetFishState(FishState pFishState)
    {
        switch (pFishState)
        {
            case (FishState.Idle):
                _currentState = FishState.Idle;
                textureID = "Piranha1";
                RoundEnd();
                break;
            case (FishState.Chase):
                _currentState = FishState.Chase;
                speed = _random.Next(4, 6);
                break;
            case (FishState.Return):
                _currentState = FishState.Return;
                break;
            case(FishState.Win):
                _currentState = FishState.Win;
                speed = 4;
                break;
        }
    }
    
}