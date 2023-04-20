using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishORama;

public class PiranhaBehaviour
{
    public delegate void AteChicken(int fishNumber);
    public static event AteChicken ChickenAte;

    public delegate void TriggerNewGame();
    public static event TriggerNewGame roundTrigger;

    public delegate void EndRound();

    public static event EndRound RoundEnd;

    private bool ateAlready;
    private Piranha piranha;
    private FishState currentState;
    private Team team;
    
    
    public PiranhaBehaviour(Piranha pPiranha, Team pTeam)
    {
        piranha = pPiranha;
        team = pTeam;
        currentState = FishState.Idle;
    }
    
    public void Update()
    {
        Movement();
    }

    private void Movement()
    {
        // *** ADD YOUR MOVEMENT/BEHAVIOUR CODE HERE ***
            switch (currentState)
            {
                case (FishState.Idle):
                    // calculate x and y coordinates based on piranha.angle and radius
                    piranha.xPosition = piranha.idlePosition.X + 10 * (float)Math.Cos(piranha.angle);
                    piranha.yPosition = piranha.idlePosition.Y + 10 * (float)Math.Sin(piranha.angle);
                    piranha.angle += 0.05f;
                    if (piranha.tokenManager.ChickenLeg != null)
                    {
                        roundTrigger();
                    }
                    break;
                case (FishState.Chase):


                    Vector2 chickenPosition;
                    Vector2 currentPosition;
                    Vector2 distanceVector;
                    Vector2 directionVector;
                    if (piranha.tokenManager.ChickenLeg != null)
                    {
                        chickenPosition = new(piranha.tokenManager.ChickenLeg.Position.X, piranha.tokenManager.ChickenLeg.Position.Y);
                        currentPosition = new(piranha.xPosition, piranha.yPosition);
                        distanceVector = Vector2.Subtract(chickenPosition, currentPosition);
                        directionVector = Vector2.Normalize(distanceVector);
                        piranha.xPosition += directionVector.X * piranha.speed;
                        piranha.yPosition += directionVector.Y * piranha.speed;

                        if (distanceVector.Length() < 10 && piranha.tokenManager.ChickenLeg != null && !ateAlready)
                        {
                            ateAlready = true;
                            SetFishState(FishState.Return);
                            Console.WriteLine($"Team {piranha.teamNumber} Fish {piranha.fishNumber} got the leg!");
                            ChickenAte(piranha.fishNumber);
                            piranha.tokenManager.RemoveChickenLeg();
                        }
                    }
                    else
                    {
                        SetFishState(FishState.Return);
                        Console.WriteLine($"Team {piranha.teamNumber} Fish {piranha.fishNumber} missed the leg!");
                    }
                    
                    break;
                
                case (FishState.Return):
                    currentPosition = new(piranha.xPosition, piranha.yPosition);
                    distanceVector = Vector2.Subtract(piranha.idlePosition, currentPosition);
                    directionVector = Vector2.Normalize(distanceVector);
                    piranha.xPosition += directionVector.X * piranha.speed;
                    piranha.yPosition += directionVector.Y * piranha.speed;

                    if (distanceVector.Length() < 5)
                    {
                        piranha.xPosition = piranha.idlePosition.X;
                        piranha.yPosition = piranha.idlePosition.Y;
                        SetFishState(FishState.Idle);
                        piranha.calcRand = true;
                    }

                    ateAlready = false;
                    break;
            }
    }
    
    public enum FishState
    {
        Idle,
        Chase,
        Return
    }
        
    public void SetFishState(FishState pFishState)
    {
        switch (pFishState)
        {
            case (FishState.Idle):
                currentState = FishState.Idle;
                Console.WriteLine($"Team {piranha.teamNumber} Fish {piranha.fishNumber} Set To Idle");
                RoundEnd();
                break;
            case (FishState.Chase):
                currentState = FishState.Chase;
                Console.WriteLine($"Team {piranha.teamNumber} Fish {piranha.fishNumber} Set To Chase");
                break;
            case (FishState.Return):
                currentState = FishState.Return;
                Console.WriteLine($"Team {piranha.teamNumber} Fish {piranha.fishNumber} Set To Return");
                break;
        }
    }
    
}