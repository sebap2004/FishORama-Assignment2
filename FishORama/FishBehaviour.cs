using System;
using System.Runtime.InteropServices;
using FishORamaEngineLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishORama;

public class FishBehaviour : Fish
{
    // These are events that are used to communicate with other scripts without having to link with each other.
    
    // Event which gets called when the fish has eaten a chicken, along with its team number that gets sent to the referee.
    public delegate void AteChicken(int teamNumber);
    public static event AteChicken ChickenAte;
    public delegate void TriggerNewGame();  
    public static event TriggerNewGame roundTrigger;
    public delegate void EndRound();
    public static event EndRound RoundEnd;

    private bool ateAlready;
    private Fish _fish;
    private FishState currentState;
    private Team team;
    
    
    public FishBehaviour(string pTextureID, float pXpos, float pYpos, Screen pScreen, ITokenManager pTokenManager, int pTeamNumber, int pFishNumber, Team pTeam) : base(pTextureID, pXpos, pYpos, pScreen, pTokenManager, pTeamNumber, pFishNumber, pTeam)
    {
        team = pTeam;
        currentState = FishState.Idle;
    }

    public override void Update()
    {
        Movement();
    }
    private Vector2 CalculateDirection(Vector2 startVec, Vector2 endVec)
    {
        Vector2 distanceVector = Vector2.Subtract(endVec, startVec);
        return distanceVector;
    }
    
    
    private void Movement()
    {
        // *** ADD YOUR MOVEMENT/BEHAVIOUR CODE HERE ***
            switch (currentState)
            {
                case (FishState.Idle):
                    // calculate x and y coordinates based on angle and radius
                    xPosition = idlePosition.X + 10 * (float)Math.Cos(angle);
                    yPosition = idlePosition.Y + 10 * (float)Math.Sin(angle);
                    angle += 0.05f;
                    if (tokenManager.ChickenLeg != null)
                    {
                        roundTrigger();
                    }
                    break;
                case (FishState.Chase):


                    Vector2 chickenPosition = new Vector2();
                    Vector2 currentPosition;
                    Vector2 distanceVector;
                    Vector2 directionVector;
                    if (tokenManager.ChickenLeg != null)
                    {
                        chickenPosition = new(tokenManager.ChickenLeg.Position.X, tokenManager.ChickenLeg.Position.Y);
                        currentPosition = new(xPosition, yPosition);
                        distanceVector = CalculateDirection(currentPosition, chickenPosition);
                        directionVector = Vector2.Normalize(distanceVector);
                        xPosition += directionVector.X * speed;
                        yPosition += directionVector.Y * speed;

                        if (distanceVector.Length() < 100 && tokenManager.ChickenLeg != null && !ateAlready)
                        {
                            ateAlready = true;
                            SetFishState(FishState.Return);
                            Console.WriteLine($"Team {teamNumber} Fish {fishNumber} got the leg!");
                            ChickenAte(team.teamNumber);
                            tokenManager.RemoveChickenLeg();
                        }
                    }
                    else
                    {
                        SetFishState(FishState.Return);
                        textureID = "Piranha2";
                        Console.WriteLine($"Team {teamNumber} Fish {fishNumber} missed the leg!");
                    }
                    
                    break;
                
                case (FishState.Return):
                    currentPosition = new(xPosition, yPosition);
                    distanceVector = CalculateDirection(currentPosition, idlePosition);
                    directionVector = Vector2.Normalize(distanceVector);
                    xPosition += directionVector.X * speed;
                    yPosition += directionVector.Y * speed;

                    if (distanceVector.Length() < 5)
                    {
                        xPosition = idlePosition.X;
                        yPosition = idlePosition.Y;
                        SetFishState(FishState.Idle);
                        calcRand = true;
                    }

                    ateAlready = false;
                    break;
                case (FishState.Win):
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
    
    public enum FishState
    {
        Idle,
        Chase,
        Return,
        Win
    }
        
    public void SetFishState(FishState pFishState)
    {
        switch (pFishState)
        {
            case (FishState.Idle):
                currentState = FishState.Idle;
                textureID = "Piranha1";
                RoundEnd();
                break;
            case (FishState.Chase):
                currentState = FishState.Chase;
                speed = new Random().Next(4, 6);
                break;
            case (FishState.Return):
                currentState = FishState.Return;
                break;
            case(FishState.Win):
                currentState = FishState.Win;
                speed = 4;
                break;
        }
    }
    
}